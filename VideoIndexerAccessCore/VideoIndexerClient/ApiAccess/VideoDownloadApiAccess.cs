using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// ビデオダウンロードAPIアクセス
    /// </summary>
    public class VideoDownloadApiAccess : IVideoDownloadApiAccess
    {
        private readonly ILogger<VideoDownloadApiAccess> _logger;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IDurableHttpClient? _durableHttpClient;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        /// <param name="durableHttpClient">耐久性のあるHTTPクライアント</param>
        public VideoDownloadApiAccess(ILogger<VideoDownloadApiAccess> logger, IApiResourceConfigurations apiResourceConfigurations, IDurableHttpClient? durableHttpClient)
        {
            _logger = logger;
            _apiResourceConfigurations = apiResourceConfigurations;
            _durableHttpClient = durableHttpClient;
        }

        /// <summary>
        /// 指定されたビデオのダウンロードURLを取得します。
        /// </summary>
        /// <param name="region">リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>ビデオのダウンロードURL</returns>
        public async Task<string> GetVideoDownloadUrl(string region, string accountId, string videoId, string accessToken)
        {
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            _logger.LogInformation("Requesting download URL for video {VideoId} in account {AccountId} with region {Region}.", videoId, accountId, region);

            var response = await httpClient.GetAsync($"{_apiResourceConfigurations.ApiEndpoint}/{region}/Accounts/{accountId}/Videos/{videoId}/SourceFile/DownloadUrl?accessToken={accessToken}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully retrieved download URL for video {VideoId} in account {AccountId}.", videoId, accountId);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError("Failed to get download URL for video {VideoId} in account {AccountId}. Status code: {StatusCode}, Reason: {ReasonPhrase}", videoId, accountId, response.StatusCode, response.ReasonPhrase);
                throw new Exception("Failed to get video Url");
            }
        }

        /// <summary>
        /// 指定されたビデオのサムネイルを取得します。
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="thumbnailId">サムネイル ID</param>
        /// <param name="format">サムネイル形式 (Jpeg / Base64)</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>サムネイルのデータ</returns>
        public async Task<string> GetVideoThumbnail(string location, string accountId, string videoId, string thumbnailId, string? format = null, string? accessToken = null)
        {
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            _logger.LogInformation("Requesting thumbnail {ThumbnailId} for video {VideoId} in account {AccountId} with location {Location}.", thumbnailId, videoId, accountId, location);

            var url = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/{videoId}/Thumbnails/{thumbnailId}";
            if (!string.IsNullOrEmpty(format) || !string.IsNullOrEmpty(accessToken))
            {
                url += "?";
                if (!string.IsNullOrEmpty(format)) url += $"format={format}&";
                if (!string.IsNullOrEmpty(accessToken)) url += $"accessToken={accessToken}";
                url = url.TrimEnd('&');
            }

            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully retrieved thumbnail {ThumbnailId} for video {VideoId} in account {AccountId}.", thumbnailId, videoId, accountId);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError("Failed to get thumbnail {ThumbnailId} for video {VideoId} in account {AccountId}. Status code: {StatusCode}, Reason: {ReasonPhrase}", thumbnailId, videoId, accountId, response.StatusCode, response.ReasonPhrase);
                throw new Exception("Failed to get video thumbnail");
            }
        }
    }
}
