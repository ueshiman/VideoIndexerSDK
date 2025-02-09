using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class ProjectMigrationApiAccess
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly ILogger<ProjectMigrationApiAccess> _logger;

        /// <summary>
        /// ProjectMigrationClient クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ProjectMigrationApiAccess(string baseUrl, HttpClient httpClient, ILogger<ProjectMigrationApiAccess> logger)
        {
            _baseUrl = baseUrl;
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// プロジェクトのマイグレーション情報を取得します。
        /// </summary>
        public async Task<ApiProjectMigrationModel> GetProjectMigrationAsync(string location, string accountId, string projectId, string accessToken = null)
        {
            var json = await FetchProjectMigrationJsonAsync(location, accountId, projectId, accessToken);
            return ParseProjectMigrationJson(json);
        }

        /// <summary>
        /// API からプロジェクトのマイグレーションデータを JSON 文字列として取得します。
        /// </summary>
        public async Task<string> FetchProjectMigrationJsonAsync(string location, string accountId, string projectId, string accessToken)
        {
            var requestUrl = $"{_baseUrl}/{location}/Accounts/{accountId}/ProjectAMSAssetMigrations/{projectId}";

            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUrl += $"?accessToken={accessToken}";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"API call failed. Status code: {response.StatusCode}, Error message: {errorResponse}");
                    throw new Exception($"API call failed. Status code: {response.StatusCode}, Error message: {errorResponse}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Network error occurred. Failed to connect to API.");
                throw new HttpRequestException("Network error occurred. Failed to connect to API.", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving project migration information.");
                throw new Exception("An unexpected error occurred while retrieving project migration information.", ex);
            }
        }

        /// <summary>
        /// 取得した JSON 文字列を ProjectMigrationResponse オブジェクトに変換します。
        /// </summary>
        public ApiProjectMigrationModel ParseProjectMigrationJson(string json)
        {
            var result = JsonSerializer.Deserialize<ApiProjectMigrationModel>(json);
            if (result == null)
            {
                throw new InvalidOperationException("Failed to deserialize JSON response.");
            }
            return result;
        }
    }
}

