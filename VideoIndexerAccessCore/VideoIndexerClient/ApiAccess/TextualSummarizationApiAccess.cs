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
    public class TextualSummarizationApiAccess
    {

        private readonly ILogger<TextualSummarizationApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // Create Video Summary

        /// <summary>
        /// 指定されたビデオのテキスト要約を取得する非同期メソッド。
        /// Create Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <param name="location">API のリージョン名 (例: "trial")</param>
        /// <param name="accountId">Azure Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="videoId">対象のビデオ ID (GUID 形式)</param>
        /// <param name="accessToken">API のアクセストークン (オプション、null の場合は未指定)</param>
        /// <param name="length">要約の長さ (Short, Medium, Long のいずれか)</param>
        /// <param name="style">要約のスタイル (Neutral, Casual, Formal のいずれか)</param>
        /// <param name="includedFrames">含めるフレーム (None, Keyframes のいずれか)</param>
        /// <returns>ビデオ要約のレスポンスモデル `ApiVideoSummaryModel` を返す。失敗時は null。</returns>
        public async Task<ApiVideoSummaryModel?> GetVideoSummaryAsync(
            string location, string accountId, string videoId, string? accessToken = null,
            string? length = null, string? style = null, string? includedFrames = null)
        {
            try
            {
                string url = BuildApiUrl(out string maskedUrl, location, accountId, videoId, accessToken, length, style, includedFrames);
                string jsonResponse = await FetchApiResponseAsync(url, maskedUrl);
                return ParseVideoSummaryJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while fetching video summary.");
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while processing video summary response.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching video summary.");
                return null;
            }
        }

        /// <summary>
        /// API のエンドポイント URL を構築する。
        /// Create Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <returns>構築された API URL (クエリパラメータ含む)</returns>
        public string BuildApiUrl(out string maskedUrl, string location, string accountId, string videoId, string? accessToken, string? length, string? style, string? includedFrames)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Summaries/Textual";
            maskedUrl = url;

            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(length)) queryParams.Add($"length={length}");
            if (!string.IsNullOrEmpty(style)) queryParams.Add($"style={style}");
            if (!string.IsNullOrEmpty(includedFrames)) queryParams.Add($"includedFrames={includedFrames}");

            if (queryParams.Count > 0) maskedUrl += "?" + string.Join("&", queryParams);

            if (!string.IsNullOrEmpty(accessToken))
            {
                queryParams.Add($"accessToken={accessToken}");
                maskedUrl += $"{(queryParams.Count > 0 ? "&" : "?")}accessToken=***";
            }

            if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);
            return url;
        }

        /// <summary>
        /// 指定した URL へ GET リクエストを送り、JSON レスポンスを取得する。
        /// Create Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <param name="url">リクエストを送信する API の URL</param>
        /// <param name="maskedUrl"></param>
        /// <returns>API から取得した JSON レスポンス文字列</returns>
        public async Task<string> FetchApiResponseAsync(string url, string maskedUrl)
        {
            _logger.LogInformation("Sending request to Video Indexer API: {maskedUrl}", maskedUrl);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON レスポンスを `ApiVideoSummaryModel` オブジェクトに変換する。
        /// Create Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースされた `ApiVideoSummaryModel` オブジェクト。失敗時は null。</returns>
        public ApiVideoSummaryModel? ParseVideoSummaryJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiVideoSummaryModel>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response from Video Indexer API.");
                return null;
            }
        }

        // Delete Video Summary

        /// <summary>
        /// ビデオのテキスト要約を削除する非同期メソッド。
        /// </summary>
        /// <param name="location">API のリージョン名 (例: "trial")</param>
        /// <param name="accountId">Azure Video Indexer のアカウント ID</param>
        /// <param name="videoId">対象ビデオの ID</param>
        /// <param name="summaryId">削除する要約の ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>成功した場合 true、失敗した場合 false</returns>
        public async Task<bool> DeleteVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Summaries/Textual/{summaryId}";
                string maskedUrl = url;
                if (!string.IsNullOrEmpty(accessToken))
                {
                    url += $"?accessToken={accessToken}";
                    maskedUrl += $"?accessToken=***";
                }

                _logger.LogInformation("Sending DELETE request to: {maskedUrl}", maskedUrl);
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.DeleteAsync(url);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted video summary: {SummaryId}", summaryId);
                    return true;
                }

                _logger.LogWarning("Failed to delete video summary. Status code: {StatusCode}", response.StatusCode);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while deleting video summary.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting video summary.");
                return false;
            }
        }
    }
}
