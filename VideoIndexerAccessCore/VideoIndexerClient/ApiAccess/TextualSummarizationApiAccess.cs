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

        // Get Video Summary

        /// <summary>
        /// Video Indexer API を使用して動画の要約情報を取得します。
        /// Get Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
        /// </summary>
        /// <param name="location">API リージョン（例: "trial" や "japaneast"）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象の動画 ID</param>
        /// <param name="summaryId">取得するサマリー ID（GUID）</param>
        /// <param name="accessToken">（オプション）アクセストークン。URL クエリに付加されます</param>
        /// <returns>動画要約情報を格納した ApiVideoSummaryResponseModel オブジェクト。失敗時は null。</returns>
        public async Task<ApiVideoSummaryResponseModel?> GetVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        {
            try
            {
                var json = await FetchVideoSummaryJsonAsync(location, accountId, videoId, summaryId, accessToken);
                return ParseVideoSummaryResponseJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while fetching video summary.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON from video summary.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving video summary.");
            }

            return null;
        }

        /// <summary>
        /// API にアクセスして JSON 文字列を取得します。
        /// Get Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント GUID</param>
        /// <param name="videoId">動画 ID</param>
        /// <param name="summaryId">サマリー ID</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>取得した JSON 文字列</returns>
        private async Task<string> FetchVideoSummaryJsonAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        {
            var baseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Summaries/Textual/{summaryId}";
            var maskedUrl = baseUrl;

            if (!string.IsNullOrEmpty(accessToken))
            {
                baseUrl += $"?accessToken={Uri.EscapeDataString(accessToken)}";
                maskedUrl += $"?accessToken=***";
            }

            _logger.LogInformation("Sending Get Video Summary request to: {maskedUrl}", maskedUrl);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(baseUrl);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列をパースして ApiVideoSummaryModel オブジェクトに変換します。
        /// Get Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
        /// </summary>
        /// <param name="json">動画要約の JSON データ</param>
        /// <returns>ApiVideoSummaryModel オブジェクト</returns>
        private ApiVideoSummaryResponseModel? ParseVideoSummaryResponseJson(string json)
        {
            return JsonSerializer.Deserialize<ApiVideoSummaryResponseModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        // List Video Summaries

        /// <summary>
        /// 動画に紐づくすべてのテキスト要約メタ情報をリスト形式で取得します。
        /// </summary>
        /// <summary>
        /// 動画に紐づくすべてのテキスト要約メタ情報をリスト形式で取得します。
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "trial"、"japaneast"）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象の動画の ID</param>
        /// <param name="pageNumber">取得するページ番号（0 から始まる。省略時は 0）</param>
        /// <param name="pageSize">1ページあたりの最大件数（最大20、デフォルト20）</param>
        /// <param name="state">フィルタ対象の状態（例: "Processed", "Failed" など）</param>
        /// <param name="accessToken">アクセストークン（クエリ文字列に追加。省略可）</param>
        /// <returns>TextualSummarizationContractPage（要約リスト、ページ情報を含む）。失敗時は null。</returns>
        public async Task<ApiTextualSummarizationContractPageModel?> ListVideoSummariesAsync(string location, string accountId, string videoId, int? pageNumber = null, int? pageSize = null, string[]? state = null, string? accessToken = null)
        {
            try
            {
                var json = await FetchVideoSummariesJsonAsync(location, accountId, videoId, pageNumber, pageSize, state, accessToken);
                return ParseVideoSummariesJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while listing video summaries.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON from video summaries list.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while listing video summaries.");
            }

            return null;
        }

        ///// <summary>
        ///// 動画のテキスト要約（summaryId 指定）の JSON を API から取得します。
        ///// </summary>
        ///// <param name="location">Azure のリージョン名</param>
        ///// <param name="accountId">アカウント GUID</param>
        ///// <param name="videoId">動画 ID</param>
        ///// <param name="summaryId">取得するサマリー ID</param>
        ///// <param name="accessToken">アクセストークン（省略可能）</param>
        ///// <returns>取得した JSON 文字列</returns>
        //private async Task<string> FetchVideoSummaryJsonAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        //{
        //    var baseUrl = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/{videoId}/Summaries/Textual/{summaryId}";
        //    if (!string.IsNullOrEmpty(accessToken))
        //    {
        //        baseUrl += $"?accessToken={Uri.EscapeDataString(accessToken)}";
        //    }

        //    var response = await _httpClient.GetAsync(baseUrl);
        //    response.EnsureSuccessStatusCode();
        //    return await response.Content.ReadAsStringAsync();
        //}

        ///// <summary>
        ///// JSON 文字列を ApiVideoSummaryModel にパースします。
        ///// </summary>
        ///// <param name="json">API から取得した JSON 文字列</param>
        ///// <returns>ApiVideoSummaryModel オブジェクト。失敗時は null。</returns>
        //private ApiVideoSummaryModel? ParseVideoSummaryJson(string json)
        //{
        //    return JsonSerializer.Deserialize<ApiVideoSummaryModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //}

        /// <summary>
        /// テキスト要約の一覧 JSON を取得するために Video Indexer API を呼び出します。
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント GUID</param>
        /// <param name="videoId">動画 ID</param>
        /// <param name="pageNumber">ページ番号</param>
        /// <param name="pageSize">ページあたりの件数</param>
        /// <param name="state">状態フィルター</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <returns>API から返される JSON 文字列</returns>
        private async Task<string> FetchVideoSummariesJsonAsync(string location, string accountId, string videoId, int? pageNumber, int? pageSize, string[]? state, string? accessToken)
        {
            var query = new List<string>();
            if (pageNumber.HasValue) query.Add($"pageNumber={pageNumber.Value}");
            if (pageSize.HasValue) query.Add($"pageSize={pageSize.Value}");
            if (state != null)
            {
                foreach (var s in state)
                {
                    query.Add($"state={Uri.EscapeDataString(s)}");
                }
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                query.Add($"accessToken={Uri.EscapeDataString(accessToken)}");
            }

            var queryString = query.Count > 0 ? "?" + string.Join("&", query) : string.Empty;
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Summaries/Textual{queryString}";
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(url);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// テキスト要約一覧の JSON を ApiTextualSummarizationContractPageModel にパースします。
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>ApiTextualSummarizationContractPageModel オブジェクト</returns>
        private ApiTextualSummarizationContractPageModel? ParseVideoSummariesJson(string json)
        {
            return JsonSerializer.Deserialize<ApiTextualSummarizationContractPageModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
