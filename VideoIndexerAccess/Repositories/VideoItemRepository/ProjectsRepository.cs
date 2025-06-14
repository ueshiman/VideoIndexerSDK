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
        private const string ParamName = "projects";


        public ProjectsRepository(ILogger<ProjectsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IProjectsApiAccess projectsApiAccess, IProjectRenderOperationMapper projectRenderOperationMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _projectsApiAccess = projectsApiAccess;
            _projectRenderOperationMapper = projectRenderOperationMapper;
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
    }
}
