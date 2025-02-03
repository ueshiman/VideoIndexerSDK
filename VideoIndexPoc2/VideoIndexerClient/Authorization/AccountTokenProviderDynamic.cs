using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.VideoIndexerClient.Configuration;
using VideoIndexPoc2.VideoIndexerClient.HttpAccess;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization
{

    /// <summary>
    /// アカウントトークンを提供するクラス
    /// </summary>
    public class AccountTokenProviderDynamic : IAccountTokenProviderDynamic
    {
        private readonly IAuthorizationSecret _authorizationSecret;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly ILogger<AccountTokenProviderDynamic> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="authorizationSecret">認証シークレット</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        /// <param name="durableHttpClient">耐久性のあるHTTPクライアント</param>
        public AccountTokenProviderDynamic(ILogger<AccountTokenProviderDynamic> logger, IAuthorizationSecret authorizationSecret, IApiResourceConfigurations apiResourceConfigurations, IDurableHttpClient? durableHttpClient)
        {
            _logger = logger;
            _authorizationSecret = authorizationSecret;
            _apiResourceConfigurations = apiResourceConfigurations;
            _durableHttpClient = durableHttpClient;
        }

        /// <summary>
        /// ARMアクセス トークンを非同期で取得する
        /// </summary>
        /// <param name="ct">キャンセレーション トークン</param>
        /// <returns>ARMアクセス トークン</returns>
        public async Task<string> GetArmAccessTokenAsync(CancellationToken ct = default)
        {
            var credentials = GetTokenCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { $"{_apiResourceConfigurations.AzureResource}/.default" });
            var tokenRequestResult = await credentials.GetTokenAsync(tokenRequestContext, ct);
            return tokenRequestResult.Token;
        }

        /// <summary>
        /// ARMアクセス トークンを取得する
        /// </summary>
        /// <param name="ct">キャンセレーション トークン</param>
        /// <returns>ARMアクセス トークン</returns>
        public string GetArmAccessToken(CancellationToken ct = default)
        {
            var credentials = GetTokenCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { $"{_apiResourceConfigurations.AzureResource}/.default" });
            var tokenRequestResult = credentials.GetToken(tokenRequestContext, ct);
            return tokenRequestResult.Token;
        }

        /// <summary>
        /// アカウントアクセス トークンを取得する
        /// </summary>
        /// <param name="armAccessToken">ARMアクセス トークン</param>
        /// <param name="permission">アクセス許可</param>
        /// <param name="scope">スコープ</param>
        /// <param name="ct">キャンセレーション トークン</param>
        /// <returns>アカウントアクセス トークン</returns>
        public string GetAccountAccessToken(string armAccessToken, ArmAccessTokenPermission permission = ArmAccessTokenPermission.Contributor, ArmAccessTokenScope scope = ArmAccessTokenScope.Account, CancellationToken ct = default)
        {
            var accessTokenRequest = new AccessTokenRequest
            {
                PermissionType = permission,
                Scope = scope
            };

            try
            {
                var jsonRequestBody = JsonSerializer.Serialize(accessTokenRequest);
                _logger.LogInformation($"Getting Account access token: {jsonRequestBody}");
                var httpContent = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations?.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{_apiResourceConfigurations?.ViAccountName}/generateAccessToken?api-version={_apiResourceConfigurations?.ApiVersion}";
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", armAccessToken);

                var result = httpClient.PostAsync(requestUri, httpContent, ct).Result;
                result.EnsureSuccessStatusCode();
                var jsonResponseBody = result.Content.ReadAsStringAsync(ct).Result;
                _logger.LogInformation($"Got Account access token: {scope} , {permission}");
                return JsonSerializer.Deserialize<GenerateAccessTokenResponse>(jsonResponseBody)?.AccessToken!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAccountAccessTokenAsync");
                throw;
            }
        }

        /// <summary>
        /// アカウントアクセス トークンを非同期で取得する
        /// </summary>
        /// <param name="armAccessToken">ARMアクセス トークン</param>
        /// <param name="permission">アクセス許可</param>
        /// <param name="scope">スコープ</param>
        /// <param name="ct">キャンセレーション トークン</param>
        /// <returns>アカウントアクセス トークン</returns>
        public async Task<string> GetAccountAccessTokenAsync(string armAccessToken, ArmAccessTokenPermission permission = ArmAccessTokenPermission.Contributor, ArmAccessTokenScope scope = ArmAccessTokenScope.Account, CancellationToken ct = default)
        {
            var accessTokenRequest = new AccessTokenRequest
            {
                PermissionType = permission,
                Scope = scope
            };

            try
            {
                var jsonRequestBody = JsonSerializer.Serialize(accessTokenRequest);
                _logger.LogInformation($"Getting Account access token: {jsonRequestBody}");
                var httpContent = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                var requestUri = $"{_apiResourceConfigurations.AzureResource}/subscriptions/{_apiResourceConfigurations.SubscriptionId}/resourcegroups/{_apiResourceConfigurations?.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{_apiResourceConfigurations?.ViAccountName}/generateAccessToken?api-version={_apiResourceConfigurations?.ApiVersion}";
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", armAccessToken);

                var result = await httpClient.PostAsync(requestUri, httpContent, ct);
                result.EnsureSuccessStatusCode();
                var jsonResponseBody = await result.Content.ReadAsStringAsync(ct);
                _logger.LogInformation($"Got Account access token: {scope} , {permission}");
                return JsonSerializer.Deserialize<GenerateAccessTokenResponse>(jsonResponseBody)?.AccessToken!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAccountAccessTokenAsync");
                throw;
            }
        }

        /// <summary>
        /// トークン資格情報を取得する
        /// </summary>
        /// <returns>トークン資格情報</returns>
        private TokenCredential GetTokenCredential()
        {
            if (!string.IsNullOrEmpty(_authorizationSecret.ClientId) && !string.IsNullOrEmpty(_authorizationSecret.ClientSecret))
            {
                return new ClientSecretCredential(_authorizationSecret.TenantId, _authorizationSecret.ClientId, _authorizationSecret.ClientSecret);
            }
            else
            {
                var credentialOptions = _authorizationSecret.TenantId == null ? new DefaultAzureCredentialOptions() : new DefaultAzureCredentialOptions
                {
                    VisualStudioTenantId = _authorizationSecret.TenantId,
                    VisualStudioCodeTenantId = _authorizationSecret.TenantId,
                    SharedTokenCacheTenantId = _authorizationSecret.TenantId,
                    InteractiveBrowserTenantId = _authorizationSecret.TenantId
                };

                return new DefaultAzureCredential(credentialOptions);
            }
        }
    }
}
