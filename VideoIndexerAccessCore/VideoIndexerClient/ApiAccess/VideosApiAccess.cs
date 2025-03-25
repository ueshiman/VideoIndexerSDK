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
    public class VideosApiAccess
    {
        private readonly ILogger<VideosApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // Delete Video

        /// <summary>
        /// 指定された動画を Video Indexer から削除します。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
        /// <param name="accountId">Video Indexer アカウントの一意な GUID 文字列</param>
        /// <param name="videoId">削除対象のビデオの ID</param>
        /// <param name="accessToken">アクセストークン（省略可能。指定しない場合は認証されないリクエストとなります）</param>
        /// <returns>
        /// 削除結果を示す <see cref="DeleteVideoResult"/> オブジェクト。
        /// 削除に成功した場合は null（NoContent）、一部失敗した場合は失敗アセット情報を含むオブジェクトを返します。
        /// </returns>
        public async Task<ApiDeleteVideoResultModel?> DeleteVideoAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            var json = await FetchDeleteVideoJsonAsync(location, accountId, videoId, accessToken);
            return ParseDeleteVideoJson(json);
        }

        /// <summary>
        /// Video Indexer API に DELETE リクエストを送信し、JSONレスポンスを取得します。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="location">Azure リージョン名</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">削除する動画の ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>
        /// API のレスポンス本文（JSON文字列）。
        /// 削除に成功し NoContent の場合は null を返します。
        /// </returns>
        private async Task<string?> FetchDeleteVideoJsonAsync(string location, string accountId, string videoId, string? accessToken)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}";

                if (!string.IsNullOrWhiteSpace(accessToken))
                    url += $"?accessToken={Uri.EscapeDataString(accessToken)}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                // 任意のGUIDでリクエストIDを送信（省略可能）
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("Video deleted successfully.");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Video deletion response: {Json}", json);
                return json;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                return null;
            }
        }

        /// <summary>
        /// JSON文字列を ApiDeleteVideoResultModel オブジェクトにパースします。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="json">API から返された JSON 文字列</param>
        /// <returns>
        /// JSON をデシリアライズした <see cref="ApiDeleteVideoResultModel"/> オブジェクト。
        /// JSON が null またはパースできない場合は null を返します。
        /// </returns>
        private ApiDeleteVideoResultModel? ParseDeleteVideoJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<ApiDeleteVideoResultModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error.");
                return null;
            }
        }

        // Delete Video Source File


    }
}

