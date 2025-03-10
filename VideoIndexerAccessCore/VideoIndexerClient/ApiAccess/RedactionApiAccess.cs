using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
    /// API から JSON データを取得します。
    /// </summary>
    private async Task<string> FetchRedactVideoJsonAsync(string location, string accountId, string videoId, RedactVideoRequest request, string? accessToken = null)
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
                if (response is null) throw new HttpRequestException("The response was null."); response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Redact video request failed: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// JSON をパースして ApiRedactVideoResponseModel オブジェクトに変換します。
        /// </summary>
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
        /// </summary>
        public async Task<ApiRedactVideoResponseModel?> RedactVideoAsync(string location, string accountId, string videoId, RedactVideoRequest request, string? accessToken = null)
        {
            var json = await FetchRedactVideoJsonAsync(location, accountId, videoId, request, accessToken);
            return ParseRedactVideoJson(json);
        }
    }

public class RedactVideoRequest
{
    public FaceRedaction faces { get; set; } = new FaceRedaction();
}

public class FaceRedaction
{
    public string blurringKind { get; set; } = "HighBlur";
    public FaceFilter filter { get; set; } = new FaceFilter();
}

public class FaceFilter
{
    public List<int> ids { get; set; } = new List<int>();
    public string scope { get; set; } = "Exclude";
}

public class ApiRedactVideoResponseModel
{
    public string? errorType { get; set; }
    public string? message { get; set; }
}

}
