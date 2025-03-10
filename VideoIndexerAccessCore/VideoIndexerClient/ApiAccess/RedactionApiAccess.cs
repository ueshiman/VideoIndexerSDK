using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class RedactionApiAccess
    {
        private readonly ILogger<RedactionApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// API からビデオ編集 (Redact) の JSON データを取得します。
        /// Redact Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="request">ビデオの編集リクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>API から取得した JSON 文字列</returns>
        private async Task<string> FetchRedactVideoJsonAsync(string location, string accountId, string videoId, ApiRedactVideoRequestModel request, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/redact";
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
                response.EnsureSuccessStatusCode();
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redact video request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON をパースして ApiRedactVideoResponseModel オブジェクトに変換します。
        /// Redact Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiRedactVideoResponseModel オブジェクト、エラー時は null</returns>
        private ApiRedactVideoResponseModel? ParseRedactVideoJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiRedactVideoResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API を呼び出してビデオの編集 (Redact) を開始します。
        /// Redact Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="request">ビデオの編集リクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>編集リクエストのステータス、エラー時は null</returns>
        public async Task<ApiRedactVideoResponseModel?> RedactVideoAsync(string location, string accountId, string videoId, ApiRedactVideoRequestModel request, string? accessToken = null)
        {
            var json = await FetchRedactVideoJsonAsync(location, accountId, videoId, request, accessToken);
            return ParseRedactVideoJson(json);
        }
    }
}