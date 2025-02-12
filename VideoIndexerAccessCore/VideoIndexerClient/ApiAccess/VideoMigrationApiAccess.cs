using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
/// <summary>
/// ビデオの移行状況を取得するクライアント。
/// </summary>
public class VideoMigrationApiAccess
    {
        private readonly ILogger<VideoMigrationApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// クライアントを初期化する。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="durableHttpClient"></param>
        /// <param name="apiResourceConfigurations"></param>
        public VideoMigrationApiAccess(ILogger<VideoMigrationApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// Web API にリクエストを送信し、レスポンスを文字列で取得する。
        /// </summary>
        private async Task<string> FetchJsonResponseAsync(string location, string accountId, string videoId, string accessToken = null)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/VideoAMSAssetMigrations/{videoId}";
            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUrl += "?accessToken=" + accessToken;
            }

            _logger.LogInformation("Sending request to {RequestUrl}", requestUrl);

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var httpClient = _durableHttpClient.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request);
                _logger.LogInformation("Received response with status code {StatusCode}", response.StatusCode);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Request failed. Status Code: {StatusCode}, Response: {ResponseBody}", response.StatusCode, errorBody);
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorBody}");
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending the request");
                throw;
            }
        }

        /// <summary>
        /// ビデオの移行情報を取得する。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="videoId">ビデオID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ビデオの移行状況</returns>
        public async Task<ApiVideoMigrationModel?> GetVideoMigrationAsync(string location, string accountId, string videoId, string accessToken = null)
        {
            var responseBody = await FetchJsonResponseAsync(location, accountId, videoId, accessToken);
            _logger.LogDebug("Response body: {ResponseBody}", responseBody);

            return ParseJsonResponse(responseBody);
        }

        /// <summary>
        /// JSONレスポンスを解析する。
        /// </summary>
        private ApiVideoMigrationModel? ParseJsonResponse(string json)
        {
            return JsonSerializer.Deserialize<ApiVideoMigrationModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

}
