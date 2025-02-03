using System.Web;
using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Parser;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// API Access レイヤーなのでAPIと一対一の機能を提供
    /// </summary>
    public class VideoIndexApiAccess : IVideoItemApiAccess
    {
        private readonly ILogger<VideoIndexApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IVideoItemParser _videoItemParser;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// コンストラクタ。ILogger、HttpClient、IVideoItemParser のインスタンスを受け取ります。
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="durableHttpClient">耐久性のあるHTTPクライアント</param>
        /// <param name="videoItemParser">ビデオアイテムのパーサー</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        public VideoIndexApiAccess(ILogger<VideoIndexApiAccess> logger, IDurableHttpClient? durableHttpClient, IVideoItemParser videoItemParser, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _videoItemParser = videoItemParser;
            _apiResourceConfigurations = apiResourceConfigurations;
            _durableHttpClient = durableHttpClient;
        }

        /// <summary>
        /// 指定されたビデオアイテムの JSON データを非同期で取得します。
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Index
        /// </summary>
        /// <param name="location">API のロケーション</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>ビデオアイテムの JSON データ</returns>
        public async Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId, string accessToken)
        {
            try
            {
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

                _logger.LogInformation("Requesting video item JSON for video {VideoId} in account {AccountId} with location {Location}.", videoId, accountId, location);

                // アクセストークンをクエリパラメータに追加
                var response = await httpClient.GetAsync($"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/{videoId}/Index?accessToken={accessToken}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully retrieved video item JSON for video {VideoId} in account {AccountId}.", videoId, accountId);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    _logger.LogError("Failed to get video item JSON for video {VideoId} in account {AccountId}. Status code: {StatusCode}, Reason: {ReasonPhrase}", videoId, accountId, response.StatusCode, response.ReasonPhrase);
                    throw new HttpRequestException($"Failed to get video item JSON: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting video item JSON.");
                throw;
            }
        }

        /// <summary>
        /// 指定されたビデオインデックスの JSON データを非同期で取得します。
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Index
        /// </summary>
        /// <param name="location">API のロケーション</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <param name="language">言語</param>
        /// <param name="reTranslate">再翻訳するかどうか</param>
        /// <param name="includeStreamingUrls">ストリーミングURLを含めるかどうか</param>
        /// <param name="includedInsights">含めるインサイト</param>
        /// <param name="excludedInsights">除外するインサイト</param>
        /// <returns>ビデオインデックスの JSON データ</returns>
        public async Task<string> GetVideoIndexJsonAsync(string location, string accountId, string videoId, string? accessToken = null, string? language = null, bool? reTranslate = null, bool? includeStreamingUrls = null, string? includedInsights = null, string? excludedInsights = null)
        {
            _logger.LogInformation("Requesting video index JSON for video {VideoId} in account {AccountId} with location {Location}.", videoId, accountId, location);

            var queryParams = new Dictionary<string, string?>
                {
                    { "language", language ?? "" },
                    { "reTranslate", reTranslate?.ToString().ToLower() ?? "" },
                    { "includeStreamingUrls", includeStreamingUrls?.ToString().ToLower() ?? "" },
                    { "includedInsights", includedInsights ?? "" },
                    { "excludedInsights", excludedInsights ?? "" },
                    { "accessToken", accessToken ?? "" },
                };

            // クエリが空の項目は除外
            queryParams = queryParams.Where(kv => !string.IsNullOrEmpty(kv.Value)).ToDictionary(kv => kv.Key, kv => kv.Value);

            var builder = new UriBuilder($"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/{videoId}/Index");
            var query = HttpUtility.ParseQueryString(builder.Query);

            foreach (var param in queryParams) query[param.Key] = param.Value;
            builder.Query = query.ToString();

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, builder.ToString());
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully retrieved video index JSON for video {VideoId} in account {AccountId}.", videoId, accountId);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError("Failed to get video index JSON for video {VideoId} in account {AccountId}. Status code: {StatusCode}, Reason: {ReasonPhrase}", videoId, accountId, response.StatusCode, response.ReasonPhrase);
                throw new HttpRequestException($"Failed to get video index JSON: {response.ReasonPhrase}");
            }
        }

        /// <summary>
        /// 指定されたビデオインデックスを非同期で取得します。
        /// </summary>
        /// <param name="location">API のロケーション</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <param name="language">言語</param>
        /// <param name="reTranslate">再翻訳するかどうか</param>
        /// <param name="includeStreamingUrls">ストリーミングURLを含めるかどうか</param>
        /// <param name="includedInsights">含めるインサイト</param>
        /// <param name="excludedInsights">除外するインサイト</param>
        /// <returns>ビデオインデックス</returns>
        public async Task<VideoItemApiModel> GetVideoIndexAsync(string location, string accountId, string videoId, string? accessToken = null, string? language = null, bool? reTranslate = null, bool? includeStreamingUrls = null, string? includedInsights = null, string? excludedInsights = null)
        {
            var videoIndexJson = await GetVideoIndexJsonAsync(location, accountId, videoId, accessToken, language, reTranslate, includeStreamingUrls, includedInsights, excludedInsights);
            return _videoItemParser.ParseVideoItem(videoIndexJson);
        }

        /// <summary>
        /// 指定されたビデオアイテムを同期的に取得します。
        /// </summary>
        /// <param name="location">API のロケーション</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>ビデオアイテム</returns>
        public VideoItemApiModel GetVideoItem(string location, string accountId, string videoId, string accessToken)
        {
            var videoListJson = GetVideoItemJsonAsync(location, accountId, videoId, accessToken).GetAwaiter().GetResult();
            return _videoItemParser.ParseVideoItem(videoListJson);
        }

        /// <summary>
        /// 指定されたビデオアイテムを非同期で取得します。
        /// </summary>
        /// <param name="location">API のロケーション</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>ビデオアイテム</returns>
        public async Task<VideoItemApiModel> GetVideoItemAsync(string location, string accountId, string videoId, string accessToken)
        {
            var videoListJson = await GetVideoItemJsonAsync(location, accountId, videoId, accessToken);
            return _videoItemParser.ParseVideoItem(videoListJson);
        }
    }
}
