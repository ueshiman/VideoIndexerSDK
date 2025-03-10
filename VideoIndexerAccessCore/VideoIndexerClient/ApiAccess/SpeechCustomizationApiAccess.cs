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
    public class SpeechCustomizationApiAccess
    {
        private readonly ILogger<SpeechCustomizationApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public SpeechCustomizationApiAccess(ILogger<SpeechCustomizationApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// API からスピーチデータセット作成の JSON データを取得します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>API から取得した JSON 文字列</returns>
        public async Task<string> FetchCreateSpeechDatasetJsonAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = content
                };
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(httpRequest);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null."); response.EnsureSuccessStatusCode();
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Create speech dataset request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON をパースして ApiSpeechDatasetResponseModel オブジェクトに変換します。
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiSpeechDatasetResponseModel オブジェクト、エラー時は null</returns>
        public ApiSpeechDatasetResponseModel? ParseSpeechDatasetJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiSpeechDatasetResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API を呼び出してスピーチデータセットを作成します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>作成したスピーチデータセット情報、エラー時は null</returns>
        public async Task<ApiSpeechDatasetResponseModel?> CreateSpeechDatasetAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null)
        {
            var json = await FetchCreateSpeechDatasetJsonAsync(location, accountId, request, accessToken);
            return ParseSpeechDatasetJson(json);
        }
    }
}
