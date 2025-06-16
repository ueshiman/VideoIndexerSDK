using Azure.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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

        private readonly IProjectsApiAccess _projectsApiAccess;
        private readonly IProjectRenderOperationMapper _projectRenderOperationMapper;
        private readonly IProjectMapper _projectMapper;
        private readonly IVideoTimeRangeMapper _videoTimeRangeMapper;


        private const string ParamName = "projects";


        public ProjectsRepository(ILogger<ProjectsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IProjectsApiAccess projectsApiAccess, IProjectRenderOperationMapper projectRenderOperationMapper, IProjectMapper projectMapper, IVideoTimeRangeMapper videoTimeRangeMapper)
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

        public async Task<string> GetProjectInsightsWidgetAsync(string location, string accountId, string projectId, string? widgetType = null, string? accessToken = null)
        {
            throw new NotImplementedException();
        }

    }
}
