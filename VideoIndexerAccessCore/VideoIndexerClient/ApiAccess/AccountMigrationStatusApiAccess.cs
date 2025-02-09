using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    // アカウント移行ステータスを取得するためのクラス
    public class AccountMigrationStatusApiAccess : IAccountMigrationStatusApiAccess
    {
        //private readonly string _paramName = "JsonSerializer.Deserialize<Account>(jsonResponseBody)";
        private readonly ILogger<AccountMigrationStatusApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // コンストラクタ


        // アカウント移行ステータスを非同期で取得するメソッド
        public AccountMigrationStatusApiAccess(ILogger<AccountMigrationStatusApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

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

        // JSONレスポンスをデシリアライズするメソッド
        public ApiAccountMigrationStatusModel? DeserializeResponse(string jsonString)
        {
            return JsonSerializer.Deserialize<ApiAccountMigrationStatusModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // アカウント移行ステータスのJSONを非同期で取得するメソッド
        public async Task<string> GetAccountMigrationStatusJsonAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/AMSAssetsMigration";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    url += "?accessToken=" + Uri.EscapeDataString(accessToken);
                }

                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Add("Authorization", $"Bearer {accessToken}");
                }

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                HttpResponseMessage response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

                _logger.LogError("Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}", response.StatusCode, await response.Content.ReadAsStringAsync());
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {await response.Content.ReadAsStringAsync()}");

                return await response.Content.ReadAsStringAsync();
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
