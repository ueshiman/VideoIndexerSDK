using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.VideoIndexerClient.Authorization;
using VideoIndexPoc2.VideoIndexerClient.HttpAccess;
using VideoIndexPoc2.VideoIndexerClient.Model;
using VideoIndexPoc2.VideoIndexerClient.Configuration;

namespace VideoIndexPoc2.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// Account情報アクセス
    /// </summary>
    public class AccountApiAccess : IAccounApitAccess
    {
        private Account? _account;
        private readonly ILogger<AccountApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public string? DefaultHttpClientName => _apiResourceConfigurations?.DefaultHttpClientName;
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
        public Account? GetAccount(string? accountName)
        {
            if (_account is not null)
            {
                // キャッシュがあればそれを返す
                return _account;
            }

            accountName ??= _apiResourceConfigurations.ViAccountName; // デフォルトのアカウント名を設定

            _logger.LogInformation($"Getting account {accountName}.");
            try
            {
                // リクエストURIの設定
                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{accountName}?api-version={_apiResourceConfigurations.ApiVersion}";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountTokenProvider.GetArmAccessToken());

                var result = _client.GetAsync(requestUri).Result;

                result.VerifyStatus(System.Net.HttpStatusCode.OK);
                var jsonResponseBody = result.Content.ReadAsStringAsync().Result;
                Account? account = JsonSerializer.Deserialize<Account>(jsonResponseBody);
                VerifyValidAccount(account, accountName);
                _logger.LogInformation($"[Account Details] Id:{account!.Properties.Id}, Location: {account.Location}");
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
        public async Task<Account?> GetAccountAsync(string? accountName = null)
        {
            if (_account is not null)
            {
                // キャッシュがあればそれを返す
                return _account;
            }

            accountName ??= _apiResourceConfigurations.ViAccountName; // デフォルトのアカウント名を設定

            _logger.LogInformation($"Getting account {accountName}.");
            try
            {
                // リクエストURIの設定
                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{accountName}?api-version={_apiResourceConfigurations.ApiVersion}";
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accountTokenProvider.GetArmAccessToken());

                var result = await _client.GetAsync(requestUri);

                result.VerifyStatus(System.Net.HttpStatusCode.OK);
                var jsonResponseBody = await result.Content.ReadAsStringAsync();
                Account? account = JsonSerializer.Deserialize<Account>(jsonResponseBody);
                VerifyValidAccount(account, accountName);
                _logger.LogInformation($"[Account Details] Id:{account!.Properties.Id}, Location: {account.Location}");
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
        private void VerifyValidAccount(Account account, string? accountName)
        {
            if (string.IsNullOrWhiteSpace(account?.Location) || account.Properties == null || string.IsNullOrWhiteSpace(account.Properties.Id))
            {
                _logger.LogError($"{nameof(accountName)} {accountName} not found. Check {nameof(_apiResourceConfigurations.SubscriptionId)}, {nameof(_apiResourceConfigurations.ResourceGroup)}, {nameof(accountName)} are valid.");
                throw new Exception($"Account {accountName} not found.");
            }
        }
    }
}
