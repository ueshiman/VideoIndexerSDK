using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// Account情報アクセス
    /// </summary>
    public class AccountApiAccess : IAccounApitAccess
    {
        private const string ParamName = "JsonSerializer.Deserialize<ApiTrialAccountModel>(jsonResponseBody)";
        private static ApiAccountModel? _account;
        private readonly ILogger<AccountApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public string DefaultHttpClientName => _apiResourceConfigurations.DefaultHttpClientName;
        public string HttpClientName { get; set; }
        // アクセストークンは都度都度再生成
        //private string _armAccessToken;
        private readonly HttpClient _client;

        /// <summary>
        /// コンストラクタ。ILogger、IDurableHttpClient、IApiResourceConfigurations、IAccountTokenProviderDynamic のインスタンスを受け取ります。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="durableHttpClient">耐久性のある HTTP クライアント</param>
        /// <param name="apiResourceConfigurations">API リソースの設定</param>
        /// <param name="accountTokenProvider">アカウントトークンプロバイダー</param>
        public AccountApiAccess(ILogger<AccountApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations, IAccountTokenProviderDynamic accountTokenProvider)
        {
            _logger = logger;
            _accountTokenProvider = accountTokenProvider;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
            HttpClientName = _apiResourceConfigurations.DefaultHttpClientName ?? DefaultHttpClientName;
            //_armAccessToken = _accountTokenProvider.GetArmAccessToken();
            _client = _durableHttpClient?.HttpClient ?? new HttpClient(); // IHttpClientFactoryからHttpClientが取れない場合は、直接生成
        }

        /// <summary>
        /// Video indexer APIでアカウント情報の取得
        /// 事前取得をして設定ファイルで持つのではなくAPIで取得する
        /// </summary>
        /// <param name="accountName">アカウント名</param>
        /// <returns>アカウント情報</returns>
        public ApiAccountModel? GetAccount(string? accountName)
        {
            if (_account is not null)
            {
                // キャッシュがあればそれを返す
                return _account;
            }

            accountName ??= _apiResourceConfigurations.ViAccountName; // デフォルトのアカウント名を設定

            _logger.LogInformation("Getting account {accountName}.", accountName);
            try
            {
                // リクエストURIの設定
                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{accountName}?api-version={_apiResourceConfigurations.ApiVersion}";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountTokenProvider.GetArmAccessToken());

                var result = _client.GetAsync(requestUri).Result;

                result.VerifyStatus(System.Net.HttpStatusCode.OK);
                var jsonResponseBody = result.Content.ReadAsStringAsync().Result;
                ApiAccountModel? account = JsonSerializer.Deserialize<ApiAccountModel>(jsonResponseBody) ?? throw new ArgumentNullException(ParamName);
                //if (account == null) throw new ArgumentNullException(nameof(account));
                VerifyValidAccount(account, accountName);
                _logger.LogInformation("[ApiTrialAccountModel Details] Id:{account!.properties!.id}, location: {account.location}", account!.properties!.id, account.location);
                _account = account;
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the account.");
                throw;
            }
        }

        /// <summary>
        /// Video indexer APIでアカウント情報の取得
        /// 事前取得をして設定ファイルで持つのではなくAPIで取得する
        /// </summary>
        /// <param name="accountName">アカウント名</param>
        /// <returns>アカウント情報</returns>
        public async Task<ApiAccountModel?> GetAccountAsync(string? accountName = null)
        {
            if (_account is not null)
            {
                // キャッシュがあればそれを返す
                return _account;
            }

            accountName ??= _apiResourceConfigurations.ViAccountName; // デフォルトのアカウント名を設定

            _logger.LogInformation("Getting account {accountName}.", accountName);
            try
            {
                // リクエストURIの設定
                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{accountName}?api-version={_apiResourceConfigurations.ApiVersion}";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountTokenProvider.GetArmAccessToken());

                var result = await _client.GetAsync(requestUri);
                //var result = _client.GetAsync(requestUri).Result;

                result.VerifyStatus(System.Net.HttpStatusCode.OK);
                var jsonResponseBody = await result.Content.ReadAsStringAsync();
                ApiAccountModel? account = JsonSerializer.Deserialize<ApiAccountModel>(jsonResponseBody) ?? throw new ArgumentNullException(paramName: ParamName);
                VerifyValidAccount(account, accountName);
                _logger.LogInformation("[ApiTrialAccountModel Details] Id:{account!.properties!.id}, Location: {account.location}", account!.properties?.id, account.location);
                _account = account;
                return account;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the account.");
                throw;
            }
        }

        /// <summary>
        /// アカウントが有効かどうかを検証します。
        /// </summary>
        /// <param name="account">アカウント情報</param>
        /// <param name="accountName">アカウント名</param>
        private void VerifyValidAccount(ApiAccountModel account, string? accountName)
        {
            if (string.IsNullOrWhiteSpace(account?.location) || account.properties == null || string.IsNullOrWhiteSpace(account.properties.id))
            {
                _logger.LogError("{nameof(accountName)} {accountName} not found. Check {nameof(_apiResourceConfigurations.SubscriptionId)}, {nameof(_apiResourceConfigurations.ResourceGroup)}, {nameof(accountName)} are valid."
                    , nameof(accountName), accountName, nameof(_apiResourceConfigurations.SubscriptionId), nameof(_apiResourceConfigurations.ResourceGroup), nameof(accountName));
                //throw new Exception($"ApiTrialAccountModel {accountName} not found.");
                account.properties ??= new ApiAccountProperties();
                account.properties.id = string.IsNullOrEmpty(account.properties.id) ? _apiResourceConfigurations.ViAccountId : account.properties.id;
            }
        }
    }
}
