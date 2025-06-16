using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class JobsRepository : IJobsRepository
    {
        // ロガーインスタンス
        private readonly ILogger<JobsRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly IJobsApiAccess _jobsApiAccess;
        private readonly IJobStatusResponseMapper _jobStatusResponseMapper;
        private const string ParamName = "jobs";

        public JobsRepository(ILogger<JobsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IJobsApiAccess jobsApiAccess, IJobStatusResponseMapper jobStatusResponseMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _jobsApiAccess = jobsApiAccess;
            _jobStatusResponseMapper = jobStatusResponseMapper;
        }

        public async Task<JobStatusResponseModel?> GetJobStatusAsync(string jobId)
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

            return await GetJobStatusAsync(location, accountId, jobId, accessToken);
        }


        /// <summary>
        /// 指定されたロケーション・アカウントID・ジョブID・アクセストークンでジョブのステータスを取得します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="jobId">ジョブID</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>ジョブのステータス情報モデル</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<JobStatusResponseModel?> GetJobStatusAsync(string location, string accountId, string jobId, string? accessToken = null)
        {
            try
            {
                // アクセストークンが未指定の場合は取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // APIからジョブステータスを取得
                var apiResponse = await _jobsApiAccess.GetJobStatusAsync(location, accountId, jobId, accessToken);

                if (apiResponse == null)
                {
                    _logger.LogWarning("Failed to get job status. location={Location}, accountId={AccountId}, jobId={JobId}", location, accountId, jobId);
                    return null;
                }

                // レスポンスをドメインモデルにマッピング
                var result = _jobStatusResponseMapper.MapFrom(apiResponse);
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, jobId={JobId}", location, accountId, jobId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, jobId={JobId}", location, accountId, jobId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting job status: location={Location}, accountId={AccountId}, jobId={JobId}", location, accountId, jobId);
                throw;
            }
        }
    }
}
