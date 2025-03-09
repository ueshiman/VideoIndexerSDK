using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class PromptContentApiAccess
    {
        private readonly ILogger<ProjectsApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public PromptContentApiAccess(ILogger<ProjectsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        // Create Prompt Content

        /// <summary>
        /// API からプロンプトコンテンツの JSON データを取得します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="modelName">使用する LLM モデル名（オプション）</param>
        /// <param name="promptStyle">プロンプトのスタイル（オプション）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>API から取得した JSON 文字列</returns>
        private async Task<string> FetchPromptContentJsonAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PromptContent";
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(modelName)) queryParams.Add($"modelName={modelName}");
                if (!string.IsNullOrEmpty(promptStyle)) queryParams.Add($"promptStyle={promptStyle}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");
                if (queryParams.Count > 0) requestUrl += "?" + string.Join("&", queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null."); response.EnsureSuccessStatusCode();
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"API request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON データをパースし、オブジェクトに変換します。
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiPromptCreateResponseModel オブジェクト、エラー時は null</returns>
        private ApiPromptCreateResponseModel? ParsePromptContentJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiPromptCreateResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API を呼び出してプロンプトコンテンツのデータを取得します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="modelName">使用する LLM モデル名（オプション）</param>
        /// <param name="promptStyle">プロンプトのスタイル（オプション）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>プロンプトコンテンツのデータ、エラー時は null</returns>
        public async Task<ApiPromptCreateResponseModel?> GetPromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null)
        {
            var json = await FetchPromptContentJsonAsync(location, accountId, videoId, modelName, promptStyle, accessToken);
            return ParsePromptContentJson(json);
        }
    }

    // Get Prompt Content


}

