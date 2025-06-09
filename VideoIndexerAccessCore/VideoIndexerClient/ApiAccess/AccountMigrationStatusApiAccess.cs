using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// アカウント移行ステータスを取得するためのクラス
    /// Get ApiTrialAccountModel Migration Status
    /// Try it Gets the account asset’s migration status.
    /// </summary>
    public class AccountMigrationStatusApiAccess : IAccountMigrationStatusApiAccess
    {
        private readonly ILogger<AccountMigrationStatusApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// AccountMigrationStatusApiAccess クラスの新しいインスタンスを初期化します。
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Migration-Status
        /// </summary>
        /// <param name="logger">ロガーインスタンス</param>
        /// <param name="durableHttpClient">HTTP クライアント</param>
        /// <param name="apiResourceConfigurations">API リソースの設定</param>
        public AccountMigrationStatusApiAccess(ILogger<AccountMigrationStatusApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// アカウント移行ステータスを非同期で取得するメソッド
        /// Get Account Migration Status
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Migration-Status
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <returns>アカウント移行ステータスモデル</returns>
        public async Task<ApiAccountMigrationStatusModel?> GetAccountMigrationStatusAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var jsonString = await GetAccountMigrationStatusJsonAsync(location, accountId, accessToken);
                return DeserializeResponse(jsonString);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Request error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// JSONレスポンスをデシリアライズするメソッド
        /// Get Account Migration Status
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Migration-Status
        /// </summary>
        /// <param name="jsonString">JSON文字列</param>
        /// <returns>アカウント移行ステータスモデル</returns>
        public ApiAccountMigrationStatusModel? DeserializeResponse(string jsonString)
        {
            return JsonSerializer.Deserialize<ApiAccountMigrationStatusModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        /// <summary>
        /// アカウント移行ステータスのJSONを非同期で取得するメソッド
        /// Get Account Migration Status
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Migration-Status
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <returns>JSON文字列</returns>
        public async Task<string> GetAccountMigrationStatusJsonAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/AMSAssetsMigration";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    url += "?accessToken=" + Uri.EscapeDataString(accessToken);
                }

                using HttpRequestMessage request = new(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                }

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                _logger.LogError("Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}", response.StatusCode, await response.Content.ReadAsStringAsync());
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP Request error: {ex.Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {ex.Message}", ex.Message);
                throw;
            }
        }
    }
}
