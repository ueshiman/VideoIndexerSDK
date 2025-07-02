using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class ProjectsRepository
    {
        // ロガーインスタンス
        private readonly ILogger<ProjectsRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // IProjectsApiAccess インターフェースのインスタンス
        private readonly IProjectsApiAccess _projectsApiAccess;
        // プロジェクトレンダー操作のマッピングを行うインターフェース
        private readonly IProjectRenderOperationMapper _projectRenderOperationMapper;
        // プロジェクトモデルのマッピングを行うインターフェース
        private readonly IProjectMapper _projectMapper;

        // ビデオ時間範囲のマッピングを行うインターフェース
        private readonly IVideoTimeRangeMapper _videoTimeRangeMapper;

        // プロジェクト検索結果のマッピングを行うインターフェース
        private readonly IProjectSearchResultMapper _projectSearchResultMapper;
        // プロジェクトレンダーレスポンスのマッピングを行うインターフェース
        private readonly IProjectRenderResponseMapper _projectRenderResponseMapper;
        // プロジェクト更新リクエストのマッピングを行うインターフェース
        private readonly IProjectUpdateRequestMapper _projectUpdateRequestMapper;
        // プロジェクト更新レスポンスのマッピングを行うインターフェース
        private readonly IProjectUpdateResponseMapper _projectUpdateResponseMapper;

        private const string ParamName = "projects";


        public ProjectsRepository(ILogger<ProjectsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IProjectsApiAccess projectsApiAccess, IProjectRenderOperationMapper projectRenderOperationMapper, IProjectMapper projectMapper, IVideoTimeRangeMapper videoTimeRangeMapper, IProjectSearchResultMapper projectSearchResultMapper, IProjectRenderResponseMapper projectRenderResponseMapper, IProjectUpdateRequestMapper projectUpdateRequestMapper, IProjectUpdateResponseMapper projectUpdateResponseMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _projectsApiAccess = projectsApiAccess;
            _projectRenderOperationMapper = projectRenderOperationMapper;
            _projectMapper = projectMapper;
            _videoTimeRangeMapper = videoTimeRangeMapper;
            _projectSearchResultMapper = projectSearchResultMapper;
            _projectRenderResponseMapper = projectRenderResponseMapper;
            _projectUpdateRequestMapper = projectUpdateRequestMapper;
            _projectUpdateResponseMapper = projectUpdateResponseMapper;
        }

        /// <summary>
        /// プロジェクトのレンダー操作をキャンセルします。
        /// アカウント情報を取得し、APIを呼び出してキャンセル処理を行います。
        /// </summary>
        /// <param name="projectId">プロジェクトID</param>
        /// <returns>キャンセル操作の結果モデル</returns>
        public async Task<ProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 詳細なキャンセルAPI呼び出し
            return await CancelProjectRenderOperationAsync(location!, accountId!, projectId, accessToken);
        }

        /// <summary>
        /// プロジェクトのレンダー操作をキャンセルします。
        /// </summary>
        /// <param name="location">リージョン名</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>キャンセル操作の結果モデル</returns>
        public async Task<ProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                ApiProjectRenderOperationModel? apiResponse = await _projectsApiAccess.CancelProjectRenderOperationAsync(location, accountId, projectId, accessToken);
                if (apiResponse is null)
                {
                    _logger.LogWarning("Failed to get project render operation. location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                    return null;
                }

                return _projectRenderOperationMapper.MapFrom(apiResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: Location={Location}, AccountId={AccountId}, ProjectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: Location={Location}, AccountId={AccountId}, ProjectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while cancelling project render operation. Location={Location}, AccountId={AccountId}, ProjectId={ProjectId}", location, accountId, projectId);
                throw;
            }
        }

        /// <summary>
        /// プロジェクトの作成とレンダー操作の進捗を非同期で逐次返却します。
        /// プロジェクト作成後、レンダー操作の状態をポーリングし、進捗ごとに BuildProjectResponseModel を yield return します。
        /// </summary>
        /// <param name="request">プロジェクト作成およびレンダー操作のリクエストモデル</param>
        /// <returns>進捗ごとに BuildProjectResponseModel を返す非同期イテレータ</returns>
        public async IAsyncEnumerable<BuildProjectResponseModel> BuildProjectAsync(BuildProjectRequestModel request)
        {
            ProjectModel? createResponse;

            try
            {
                createResponse = await CreateProjectAsync(request.CreateProjectRequest);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error:");
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred");
                throw;
            }

            if (createResponse is null)
            {
                _logger.LogWarning("Failed to create project.");
                yield return new BuildProjectResponseModel
                {
                    Status = BuildProjectResponseModelStatus.Failure,
                    Project = null,
                    RenderOperation = null
                };
            }


            _logger.LogInformation("Project created successfully. ProjectId={ProjectId}", createResponse!.Id);
            // プロジェクトのレンダー操作を開始

            BuildProjectResponseModel renderOperation = new()
            {
                Status = BuildProjectResponseModelStatus.Started,
                Project = null,
                RenderOperation = null,
            };
            // レンダー操作の結果を待機
            do
            {
                await Task.Delay(request.DelayMilliSecond); // 5秒待機
                // レンダー操作の状態を取得
                renderOperation.RenderOperation = await GetProjectRenderOperationAsync(createResponse.Id);
                if (renderOperation.RenderOperation is null)
                {
                    _logger.LogWarning("Failed to get project render operation status. ProjectId={ProjectId}", createResponse.Id);
                    yield return new BuildProjectResponseModel
                    {
                        Status = BuildProjectResponseModelStatus.Failure,
                        Project = createResponse,
                        RenderOperation = null
                    };
                    yield break;
                }

                yield return new BuildProjectResponseModel
                {
                    RenderOperation = renderOperation.RenderOperation,
                    Project = null,
                    Status = BuildProjectResponseModelStatus.InProgress,
                };
            } while (renderOperation.Status == BuildProjectResponseModelStatus.Started || renderOperation.Status == BuildProjectResponseModelStatus.InProgress);
        }



        /// <summary>
        /// Video Indexer API で新しいプロジェクトを作成します。
        /// アカウント情報を取得し、APIを呼び出してプロジェクト作成処理を行います。
        /// </summary>
        /// <param name="request">プロジェクト作成リクエストモデル</param>
        /// <returns>作成されたプロジェクトの情報（ProjectModel）、失敗時はnull</returns>
        public async Task<ProjectModel?> CreateProjectAsync(CreateProjectRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: "accountId");
            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();
            // プロジェクト作成API呼び出し
            return await CreateProjectAsync(location!, accountId, request, accessToken);
        }



        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクト作成リクエスト・アクセストークンでVideo Indexer APIにプロジェクト作成リクエストを送信し、
        /// 結果をProjectModelとして返します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="request">プロジェクト作成リクエストモデル</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>作成されたプロジェクトの情報（ProjectModel）、失敗時はnull</returns>
        public async Task<ProjectModel?> CreateProjectAsync(string location, string accountId, CreateProjectRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                ApiProjectModel? apiResponse = await _projectsApiAccess.CreateProjectAsync(location, accountId, request.ProjectName, request.VideoRanges.Select(_videoTimeRangeMapper.MapToApiVideoTimeRangeModel).ToList(), accessToken);

                if (apiResponse is null)
                {
                    _logger.LogWarning("Failed to create project. location={Location}, accountId={AccountId}, projectName={ProjectName}", location, accountId, request.ProjectName);
                    return null;
                }

                return _projectMapper.MapFrom(apiResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: Location={Location}, AccountId={AccountId}, ProjectName={ProjectName}", location, accountId, request.ProjectName);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: Location={Location}, AccountId={AccountId}, ProjectName={ProjectName}", location, accountId, request.ProjectName);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating project. Location={Location}, AccountId={AccountId}, ProjectName={ProjectName}", location, accountId, request.ProjectName);
                throw;
            }
        }


        /// <summary>
        /// Video Indexer API でプロジェクトを削除します。
        /// アカウント情報を取得し、APIを呼び出してプロジェクト削除処理を行います。
        /// </summary>
        /// <param name="projectId">削除するプロジェクトのID</param>
        /// <returns>削除が成功した場合はtrue、それ以外はfalse</returns>
        public async Task<bool> DeleteProjectAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: "accountId");
            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();
            // プロジェクト削除API呼び出し
            return await DeleteProjectAsync(location!, accountId, projectId, accessToken);
        }


        /// <summary>
        /// Video Indexer API でプロジェクトを削除します。
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンでAPIを呼び出し、
        /// プロジェクトの削除を実行します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="projectId">削除するプロジェクトのID</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>削除が成功した場合はtrue、それ以外はfalse</returns>
        public async Task<bool> DeleteProjectAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.DeleteProjectAsync(location, accountId, projectId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "DeleteProjectAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "DeleteProjectAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteProjectAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からレンダリング済みプロジェクトのダウンロードURLを取得します。
        /// Download Project Rendered File Url
        /// </summary>
        /// <param name="projectId">ダウンロードURLを取得するプロジェクトのID</param>
        /// <returns>ダウンロードURL（取得できない場合はnull）</returns>
        public async Task<string?> GetProjectRenderedFileDownloadUrlAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: "accountId");
            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();
            // Video Indexer API からレンダリング済みプロジェクトのダウンロードURLを取得します。
            return await GetProjectRenderedFileDownloadUrlAsync(location!, accountId, projectId, accessToken);
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からレンダリング済みプロジェクトのダウンロードURLを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="projectId">ダウンロードURLを取得するプロジェクトのID</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>ダウンロードURL（取得できない場合はnull）</returns>
        public async Task<string?> GetProjectRenderedFileDownloadUrlAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectRenderedFileDownloadUrlAsync(location, accountId, projectId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "DeleteProjectAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectRenderedFileDownloadUrlAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectRenderedFileDownloadUrlAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からプロジェクトのキャプションデータを取得します。
        /// </summary>
        /// <param name="request">
        /// プロジェクトキャプション取得リクエストモデル。
        /// <list type="bullet">
        /// <item><description>ProjectId: キャプションを取得するプロジェクトのID</description></item>
        /// <item><description>IndexId: （オプション）動画ID</description></item>
        /// <item><description>Format: （オプション）キャプションのフォーマット（Vtt / Ttml / Srt / Txt / Csv）</description></item>
        /// <item><description>Language: （オプション）キャプションの言語</description></item>
        /// <item><description>IncludeAudioEffects: （オプション）オーディオエフェクトを含めるか</description></item>
        /// <item><description>IncludeSpeakers: （オプション）スピーカー情報を含めるか</description></item>
        /// </list>
        /// </param>
        /// <returns>
        /// キャプションデータの文字列。指定されたフォーマット・言語・オプションに従って取得されます。
        /// エラーが発生した場合は null を返します。
        /// </returns>
        public async Task<string?> GetProjectCaptionsAsync(GetProjectCaptionsRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: "accountId");
            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();
            // Video Indexer API からプロジェクトのキャプションデータを取得します。
            return await GetProjectCaptionsAsync(location!, accountId, request, accessToken);
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からプロジェクトのキャプションデータを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン（例: "japaneast" など）。</param>
        /// <param name="accountId">アカウントの一意の識別子（GUID形式）。</param>
        /// <param name="request">
        /// プロジェクトキャプション取得リクエストモデル。
        /// <list type="bullet">
        /// <item><description>ProjectId: キャプションを取得するプロジェクトのID</description></item>
        /// <item><description>IndexId: （オプション）動画ID</description></item>
        /// <item><description>Format: （オプション）キャプションのフォーマット（Vtt / Ttml / Srt / Txt / Csv）</description></item>
        /// <item><description>Language: （オプション）キャプションの言語</description></item>
        /// <item><description>IncludeAudioEffects: （オプション）オーディオエフェクトを含めるか</description></item>
        /// <item><description>IncludeSpeakers: （オプション）スピーカー情報を含めるか</description></item>
        /// </list>
        /// </param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。指定しない場合は自動取得。</param>
        /// <returns>
        /// キャプションデータの文字列。指定されたフォーマット・言語・オプションに従って取得されます。
        /// エラーが発生した場合は null を返します。
        /// </returns>
        public async Task<string?> GetProjectCaptionsAsync(string location, string accountId, GetProjectCaptionsRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectCaptionsAsync(location, accountId, request.ProjectId, request.IndexId, request.Format, request.Language, request.IncludeAudioEffects, request.IncludeSpeakers, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectCaptionsAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectCaptionsAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectCaptionsAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・インデックス取得リクエスト・アクセストークンで
        /// Video Indexer API からプロジェクトのインデックス情報を取得します。
        /// </summary>
        /// <param name="request">プロジェクトインデックス取得リクエストモデル</param>
        /// <returns>取得したプロジェクトのインデックス情報（ApiProjectIndexModel）</returns>
        public async Task<ApiProjectIndexModel> GetProjectIndexAsync(GetProjectIndexRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのインデックス情報を取得
            return await GetProjectIndexAsync(location!, accountId, request, accessToken);
        }


        // 既存のメソッドを以下のように置き換え
        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・インデックス取得リクエスト・アクセストークンで
        /// Video Indexer API からプロジェクトのインデックス情報を取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="request">プロジェクトインデックス取得リクエストモデル</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>取得したプロジェクトのインデックス情報（ApiProjectIndexModel）</returns>
        public async Task<ApiProjectIndexModel> GetProjectIndexAsync(string location, string accountId, GetProjectIndexRequestModel request, string? accessToken = null)

        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectIndexAsync(location, accountId, request.ProjectId, request.Language, request.ReTranslate, request.IncludedInsights, request.ExcludedInsights, request.IncludeSummarizedInsights, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectIndexAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectIndexAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectIndexAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, request.ProjectId);
                throw;
            }
        }

        /// <summary>
        /// Video Indexer API からプロジェクトのインサイトウィジェットURLを取得します。
        /// アカウント情報を取得し、APIを呼び出してウィジェットURLを取得します。
        /// </summary>
        /// <param name="request">インサイトウィジェット取得リクエストモデル（ProjectId, WidgetTypeを含む）</param>
        /// <returns>インサイトウィジェットのURL文字列</returns>
        public async Task<string> GetProjectInsightsWidgetAsync(GetProjectInsightsWidgetRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのインサイトウィジェットURLを取得
            return await GetProjectInsightsWidgetAsync(location!, accountId, request, accessToken);
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・ウィジェットタイプ・アクセストークンで
        /// Video Indexer API からプロジェクトのインサイトウィジェットURLを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="request">インサイトウィジェット取得リクエストモデル（ProjectId, WidgetTypeを含む）</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>インサイトウィジェットのURL文字列</returns>
        public async Task<string> GetProjectInsightsWidgetAsync(string location, string accountId, GetProjectInsightsWidgetRequestModel request, string? accessToken = null)
        {
            try
            {
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                return await _projectsApiAccess.GetProjectInsightsWidgetAsync(location, accountId, request.ProjectId, request.WidgetType, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectInsightsWidgetAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}, widgetType={WidgetType}", location, accountId, request.ProjectId, request.WidgetType);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectInsightsWidgetAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}, widgetType={WidgetType}", location, accountId, request.ProjectId, request.WidgetType);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectInsightsWidgetAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}, widgetType={WidgetType}", location, accountId, request.ProjectId, request.WidgetType);
                throw;
            }
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からプロジェクトのプレイヤーウィジェットURLを取得します。
        /// </summary>
        /// <param name="projectId">プレイヤーウィジェットURLを取得するプロジェクトのID</param>
        /// <returns>プレイヤーウィジェットのURL文字列</returns>
        public async Task<string> GetProjectPlayerWidgetAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのプレイヤーウィジェットURLを取得
            return await GetProjectPlayerWidgetAsync(location!, accountId, projectId, accessToken);
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からプロジェクトのプレイヤーウィジェットURLを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="projectId">プレイヤーウィジェットURLを取得するプロジェクトのID</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>プレイヤーウィジェットのURL文字列</returns>
        public async Task<string> GetProjectPlayerWidgetAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectPlayerWidgetAsync(location, accountId, projectId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectPlayerWidgetAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectPlayerWidgetAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectPlayerWidgetAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
        }


        /// <summary>
        /// アカウント情報とプロジェクトIDからVideo Indexer APIのレンダー操作情報を取得します。
        /// </summary>
        /// <param name="projectId">レンダー操作情報を取得するプロジェクトのID</param>
        /// <returns>取得したレンダー操作情報（ProjectRenderOperationModel）</returns>
        public async Task<ProjectRenderOperationModel?> GetProjectRenderOperationAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのレンダー操作情報を取得します。
            var apiResult = await GetProjectRenderOperationAsync(location!, accountId, projectId, accessToken);
            return _projectRenderOperationMapper.MapFrom(apiResult);
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
        /// Video Indexer API からプロジェクトのレンダー操作情報を取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="projectId">レンダー操作情報を取得するプロジェクトのID</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>取得したレンダー操作情報（ApiProjectRenderOperationModel）</returns>
        public async Task<ApiProjectRenderOperationModel> GetProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectRenderOperationAsync(location, accountId, projectId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectRenderOperationAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectRenderOperationAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectRenderOperationAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, projectId);
                throw;
            }
        }

        /// <summary>
        /// レンダー操作の状態からリトライ可能かどうかを判定します。
        /// </summary>
        /// <param name="operationModel">レンダー操作モデル</param>
        /// <returns>リトライ可能な状態の場合は true、それ以外は false</returns>
        public bool IsRetry(ApiProjectRenderOperationModel operationModel)
        {
            return operationModel.state switch
            {
                "Pending" => true,
                "InProgress" => true,
                "Cancelling" => true,
                "Canceled" => false,
                "Succeeded" => false,
                "Failed" => false,
                _ => false
            };
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
        /// Video Indexer API からプロジェクトのサムネイル画像データ（バイナリストリーム）を取得します。
        /// </summary>
        /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
        /// <returns>サムネイル画像のバイナリストリーム</returns>
        public async Task<Stream> GetProjectThumbnailBitsAsync(ProjectThumbnailRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのサムネイル画像データを取得します。
            return await GetProjectThumbnailBitsAsync(location!, accountId, request, accessToken);
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
        /// Video Indexer API からプロジェクトのサムネイル画像のURLを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>サムネイル画像のバイナリストリーム</returns>
        public async Task<Stream> GetProjectThumbnailBitsAsync(string location, string accountId, ProjectThumbnailRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectThumbnailBitsAsync(location, accountId, request.ProjectId, request.ThumbnailId, request.Format, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
        /// Video Indexer API からプロジェクトのサムネイル画像データ（バイナリストリーム）を取得します。
        /// </summary>
        /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
        /// <returns>サムネイル画像のURL文字列</returns>
        public async Task<string> GetProjectThumbnailUrlAsync(ProjectThumbnailRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのサムネイル画像データを取得します。
            return await GetProjectThumbnailUrlAsync(location!, accountId, request, accessToken);
        }


        /// <summary>
        /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
        /// Video Indexer API からプロジェクトのサムネイル画像のURLを取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>サムネイル画像のURL文字列</returns>
        public async Task<string> GetProjectThumbnailUrlAsync(string location, string accountId, ProjectThumbnailRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                return await _projectsApiAccess.GetProjectThumbnailUrlAsync(location, accountId, request.ProjectId, request.ThumbnailId, request.Format, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectThumbnailBitsAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}, thumbnailId={ThumbnailId}", location, accountId, request.ProjectId, request.ThumbnailId);
                throw;
            }
        }

        /// <summary>
        /// 指定された条件でプロジェクト一覧を取得します。
        /// </summary>
        /// <param name="request">プロジェクト一覧取得リクエストモデル。</param>
        /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
        public async Task<ProjectSearchResultModel> GetProjectsAsync(ProjectsRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクト一覧を取得します。
            return await GetProjectsAsync(location!, accountId, request, accessToken);
        }

        /// <summary>
        /// 指定された条件でプロジェクト一覧を取得します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="request">プロジェクト一覧取得リクエストモデル。</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
        /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
        public async Task<ProjectSearchResultModel> GetProjectsAsync(string location, string accountId, ProjectsRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                ApiProjectSearchResultModel apiResponse = await _projectsApiAccess.GetProjectsAsync(location, accountId, request.CreatedAfter, request.CreatedBefore, request.PageSize, request.Skip, accessToken);
                return _projectSearchResultMapper.MapFrom(apiResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "GetProjectsAsync: Argument error location={Location}, accountId={AccountId}, createdAfter={CreatedAfter}, createdBefore={CreatedBefore}, pageSize={PageSize}, skip={Skip}", location, accountId, request.CreatedAfter, request.CreatedBefore, request.PageSize, request.Skip);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "GetProjectsAsync: API request failed location={Location}, accountId={AccountId}, createdAfter={CreatedAfter}, createdBefore={CreatedBefore}, pageSize={PageSize}, skip={Skip}", location, accountId, request.CreatedAfter, request.CreatedBefore, request.PageSize, request.Skip);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetProjectsAsync: Unexpected error location={Location}, accountId={AccountId}, createdAfter={CreatedAfter}, createdBefore={CreatedBefore}, pageSize={PageSize}, skip={Skip}", location, accountId, request.CreatedAfter, request.CreatedBefore, request.PageSize, request.Skip);
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトのレンダリングを開始します。
        /// </summary>
        /// <param name="request">レンダリングリクエストモデル。</param>
        /// <returns>レンダリング結果のレスポンスモデル。</returns>
        public async Task<ProjectRenderResponseModel> RenderProjectAsync(RenderProjectRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトのレンダリングを開始します。
            return await RenderProjectAsync(location!, accountId, request, accessToken);
        }

        /// <summary>
        /// 指定されたプロジェクトのレンダリングを開始します。
        /// </summary>
        /// <param name="location">API呼び出しのAzureリージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="request">レンダリングリクエストモデル。</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
        /// <returns>レンダリング結果のレスポンスモデル。</returns>
        public async Task<ProjectRenderResponseModel> RenderProjectAsync(string location, string accountId, RenderProjectRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                ApiProjectRenderResponseModel responseModel = await _projectsApiAccess.RenderProjectAsync(location, accountId, request.ProjectId, accessToken, request.SendCompletionEmail);
                return _projectRenderResponseMapper.MapFrom(responseModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "RenderProjectAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}, sendCompletionEmail={SendCompletionEmail}", location, accountId, request.ProjectId, request.SendCompletionEmail);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "RenderProjectAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}, sendCompletionEmail={SendCompletionEmail}", location, accountId, request.ProjectId, request.SendCompletionEmail);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RenderProjectAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}, sendCompletionEmail={SendCompletionEmail}", location, accountId, request.ProjectId, request.SendCompletionEmail);
                throw;
            }
        }

        /// <summary>
        /// 指定された検索条件でプロジェクトを検索します。
        /// </summary>
        /// <param name="request">検索条件を含むリクエストモデル。</param>
        /// <returns>検索結果のレスポンスモデル。</returns>
        public async Task<ProjectSearchResultModel> SearchProjectsAsync(SearchProjectsRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトを検索します。
            return await SearchProjectsAsync(location!, accountId, request, accessToken);
        }

        /// <summary>
        /// 指定された検索条件でプロジェクトを検索します。
        /// </summary>
        /// <param name="location">Azureリージョン。</param>
        /// <param name="accountId">アカウントのGUID。</param>
        /// <param name="request">検索条件を含むリクエストモデル。</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
        /// <returns>検索結果のレスポンスモデル。</returns>
        public async Task<ProjectSearchResultModel> SearchProjectsAsync(string location, string accountId, SearchProjectsRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // API呼び出し
                ApiProjectSearchResultModel resultModel = await _projectsApiAccess.SearchProjectsAsync(location, accountId, request.Query, request.SourceLanguage, request.PageSize, request.Skip, accessToken);
                return _projectSearchResultMapper.MapFrom(resultModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "SearchProjectsAsync: Argument error location={Location}, accountId={AccountId}, query={Query}, sourceLanguage={SourceLanguage}, pageSize={PageSize}, skip={Skip}", location, accountId, request.Query, request.SourceLanguage, request.PageSize, request.Skip);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "SearchProjectsAsync: API request failed location={Location}, accountId={AccountId}, query={Query}, sourceLanguage={SourceLanguage}, pageSize={PageSize}, skip={Skip}", location, accountId, request.Query, request.SourceLanguage, request.PageSize, request.Skip);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchProjectsAsync: Unexpected error location={Location}, accountId={AccountId}, query={Query}, sourceLanguage={SourceLanguage}, pageSize={PageSize}, skip={Skip}", location, accountId, request.Query, request.SourceLanguage, request.PageSize, request.Skip);
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトの情報を更新します。
        /// </summary>
        /// <param name="updateRequest">更新するプロジェクトのリクエストモデル。</param>
        /// <returns>更新されたプロジェクトのレスポンスモデル。</returns>
        public async Task<ProjectUpdateResponseModel> UpdateProjectAsync(ProjectUpdateRequestModel updateRequest)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string accountId = account.properties?.id ?? throw new ArgumentNullException(paramName: ParamName);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // Video Indexer API からプロジェクトの情報を更新します。
            ApiProjectUpdateResponseModel apiProjectUpdateResponseModel = await UpdateProjectAsync(location!, accountId, updateRequest, accessToken);
            return _projectUpdateResponseMapper.MapFrom(apiProjectUpdateResponseModel);
        }

        /// <summary>
        /// 指定されたプロジェクトの情報を更新します。
        /// </summary>
        /// <param name="location">Azureリージョン。</param>
        /// <param name="accountId">アカウントのGUID。</param>
        /// <param name="updateRequest">更新するプロジェクトのリクエストモデル。</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
        /// <returns>更新されたプロジェクトのレスポンスモデル。</returns>
        public async Task<ApiProjectUpdateResponseModel> UpdateProjectAsync(string location, string accountId, ProjectUpdateRequestModel updateRequest, string? accessToken = null)
        {
            try
            {
                // アクセストークンがなければ取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                ApiProjectUpdateRequestModel apiModel = _projectUpdateRequestMapper.MapToApiProjectUpdateRequestModel(updateRequest);

                // API呼び出し
                return await _projectsApiAccess.UpdateProjectAsync(location, accountId, updateRequest.ProjectId, apiModel, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "UpdateProjectAsync: Argument error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, updateRequest.ProjectId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "UpdateProjectAsync: API request failed location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, updateRequest.ProjectId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateProjectAsync: Unexpected error location={Location}, accountId={AccountId}, projectId={ProjectId}", location, accountId, updateRequest.ProjectId);
                throw;
            }
        }
    }
}
