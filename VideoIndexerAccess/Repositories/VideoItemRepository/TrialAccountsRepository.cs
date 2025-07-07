using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class TrialAccountsRepository : ITrialAccountsRepository
    {
        // ロガーインスタンス
        private readonly ILogger<TrialAccountsRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API設定
        private readonly ITrialAccountsApiAccess _trialAccountsApiAccess;

        private const string ParamName = "trialAccounts";

        // マッパーインターフェース
        private readonly ITrialAccountMapper _trialAccountMapper;
        private readonly ITrialAccountWithTokenMapper _accountWithTokenMapper;

        public TrialAccountsRepository(ILogger<TrialAccountsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, ITrialAccountsApiAccess trialAccountsApiAccess, ITrialAccountMapper trialAccountMapper, ITrialAccountWithTokenMapper accountWithTokenMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _trialAccountsApiAccess = trialAccountsApiAccess;
            _trialAccountMapper = trialAccountMapper;
            _accountWithTokenMapper = accountWithTokenMapper;
        }

        //public async Task<TrialAccountModel[]> GetAccountAsync(string location, string accountId, bool? includeUsage = null, bool? includeStatistics = null, string? accessToken = null)
        //{
        //    ApiTrialAccountModel[] result = await _trialAccountsApiAccess.GetAccountAsync(location, accountId, includeUsage, includeStatistics, accessToken);
        //    return _trialAccountMapper.MapFrom(result);
        //}



        /// <summary>
        /// 指定された条件に基づいてトライアルアカウント情報を非同期で取得します。
        /// </summary>
        /// <param name="includeUsage">使用状況情報を含めるかどうかを指定します。</param>
        /// <param name="includeStatistics">統計情報を含めるかどうかを指定します。</param>
        /// <returns>トライアルアカウント情報の配列。</returns>
        public async Task<TrialAccountModel[]> GetAccountAsync(bool? includeUsage = null, bool? includeStatistics = null)
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

            // アカウント情報を取得
            return await GetAccountAsync(location!, accountId!, includeUsage, includeStatistics, accessToken);
        }

        /// <summary>
        /// 指定されたロケーションと条件に基づいてトライアルアカウント情報を非同期で取得します。
        /// </summary>
        /// <param name="location">APIの呼び出し先リージョン。</param>
        /// <param name="includeUsage">使用状況情報を含めるかどうかを指定します。</param>
        /// <param name="includeStatistics">統計情報を含めるかどうかを指定します。</param>
        /// <param name="accessToken">アクセストークン（省略可能）。</param>
        /// <returns>トライアルアカウント情報の配列。</returns>
        public async Task<TrialAccountModel[]> GetAccountAsync(string location, string accountId, bool? includeUsage = null, bool? includeStatistics = null, string? accessToken = null)
        {
            var result = await _trialAccountsApiAccess.GetAccountsAsync(location, accountId, includeUsage, includeStatistics, accessToken);
            return result.Select(_trialAccountMapper.MapFrom).ToArray();
        }

        public async Task<TrialAccountWithTokenModel[]> GetAccountsAsync(bool? generateAccessTokens = null)
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

            // トライアルアカウント情報を取得
            return await GetAccountsAsync(location!, generateAccessTokens, null, accessToken);
        }

        /// <summary>
        /// 指定されたロケーションと条件に基づいてトライアルアカウント情報を非同期で取得します。
        /// </summary>
        /// <param name="location">APIの呼び出し先リージョン。</param>
        /// <param name="generateAccessTokens">各アカウントに対してアクセストークンを生成するかどうか。</param>
        /// <param name="allowEdit">アクセストークンに書き込み権限（Contributor）を含めるかどうか。</param>
        /// <param name="accessToken">アクセストークン（省略可能）。</param>
        /// <returns>トライアルアカウント情報の配列。</returns>
        public async Task<TrialAccountWithTokenModel[]> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? accessToken = null)
        {
            ApiTrialAccountWithTokenModel[]? result = await _trialAccountsApiAccess.GetAccountsWithTokenAsync(location, generateAccessTokens, allowEdit, accessToken);
            return result is null ? [] : result.Select(_accountWithTokenMapper.MapFrom).ToArray();
        }
    }
}
