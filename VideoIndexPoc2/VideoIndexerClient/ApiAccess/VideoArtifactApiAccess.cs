using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.VideoIndexerClient.Configuration;
using VideoIndexPoc2.VideoIndexerClient.Parser;
using VideoIndexPoc2.VideoIndexerClient.HttpAccess;

namespace VideoIndexPoc2.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// ビデオアーティファクトAPIアクセス
    /// </summary>
    public class VideoArtifactApiAccess : IVideoArtifactApiAccess
    {
        private readonly ILogger<VideoArtifactApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IVideoItemParser _videoItemParser;
        private readonly string _baseUrl;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="durableHttpClient">耐久性のあるHTTPクライアント</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        /// <param name="videoItemParser">ビデオアイテムパーサー</param>
        public VideoArtifactApiAccess(ILogger<VideoArtifactApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations, IVideoItemParser videoItemParser)
        {
            _logger = logger;
            _videoItemParser = videoItemParser;
            _apiResourceConfigurations = apiResourceConfigurations;
            _durableHttpClient = durableHttpClient;
            _baseUrl = _apiResourceConfigurations.ApiEndpoint;
        }

        /// <summary>
        /// 指定されたビデオのアーティファクトURLを非同期で取得します。
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <param name="artifactType">アーティファクトの種類</param>
        /// <returns>ビデオのアーティファクトURL</returns>
        public async Task<string> GetVideoArtifactUrlAsync(string location, string accountId, string videoId, string? accessToken = null, string? artifactType = null)
        {
            _logger.LogInformation("Requesting artifact URL for video {VideoId} in account {AccountId} with location {Location}.", videoId, accountId, location);

            var url = $"{_baseUrl}/{location}/Accounts/{accountId}/Videos/{videoId}/ArtifactUrl";

            var uriBuilder = new UriBuilder(url);
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrEmpty(artifactType))
            {
                query["type"] = artifactType;
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                query["accessToken"] = accessToken;
            }

            uriBuilder.Query = query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully retrieved artifact URL for video {VideoId} in account {AccountId}.", videoId, accountId);
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError("Failed to get artifact URL for video {VideoId} in account {AccountId}. Status code: {StatusCode}, Reason: {ReasonPhrase}", videoId, accountId, response.StatusCode, response.ReasonPhrase);
                throw new Exception("Failed to get video artifact URL");
            }
        }
    }
}
