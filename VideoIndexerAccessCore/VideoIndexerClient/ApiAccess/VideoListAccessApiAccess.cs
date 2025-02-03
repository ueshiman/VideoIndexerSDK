using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.Parser;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class VideoListAccessApiAccess : IVideoListApiAccess
    {
        private readonly ILogger<VideoListAccessApiAccess> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IAccountTokenProviderDynamic _accountTokenProviderDynamic;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IVideoListParser _videoListParser;


        public VideoListAccessApiAccess(ILogger<VideoListAccessApiAccess> logger, IHttpClientFactory httpClientFactory, IAccounApitAccess accountAccess, IAccountTokenProviderDynamic accountTokenProviderDynamic, IApiResourceConfigurations apiResourceConfigurations, IVideoListParser videoListParser)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _accountAccess = accountAccess;
            _accountTokenProviderDynamic = accountTokenProviderDynamic;
            _apiResourceConfigurations = apiResourceConfigurations;
            _videoListParser = videoListParser;
        }

        /// <summary>
        /// VideoIndexer APIからビデオリストを取得する
        /// 引数をWeb APIと一致させている
        /// </summary>
        /// <param name="location"></param>
        /// <param name="accountId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> GetVideoListJsonAsync(string? location = null, string? accountId = null, string? accessToken = null)
        {
            HttpClient client = _httpClientFactory.CreateClient(_apiResourceConfigurations.DefaultHttpClientName) ?? new HttpClient();
            // アクセストークンをクエリパラメータに追加
            // https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Videos
            _logger.LogInformation($"Getting video list for location: {location}, accountId: {accountId}, accessToken: {(string.IsNullOrEmpty(accessToken) ? "Not Provided" : "Provided")}");

            var response = await client.GetAsync($"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos?accessToken={accessToken}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Successfully retrieved video list: {jsonResponse}");
                return jsonResponse;
            }
            else
            {
                _logger.LogError("Failed to get video list. Status code: {StatusCode}", response.StatusCode);
                throw new Exception("Failed to get video list");
            }
        }

        //public IEnumerable<VideoListItem> ListVideos(string? location, string? accountId, string? accessToken)
        //{
        //    var videoListJson = GetVideoListJsonAsync(location, accountId, accessToken).Result;
        //    return _videoListParser.ParseVideoList(videoListJson);
        //}

        public async Task<IEnumerable<ApiVideoListItemModel>> ListVideosAsync(string? location, string? accountId, string? accessToken)
        {
            var videoListJson = await GetVideoListJsonAsync(location, accountId, accessToken);
            return _videoListParser.ParseVideoList(videoListJson);
        }

        /// <summary>
        /// 指定された条件でビデオを検索します。
        /// GET Search Videos
        /// https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/Search[?sourceLanguage][&hasSourceVideoFile][&sourceVideoId][&state][&privacy][&id][&partition][&externalId][&owner][&face][&animatedcharacter][&query][&textScope][&language][&createdAfter][&createdBefore][&pageSize][&skip][&accessToken]
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="sourceLanguage">ソース言語</param>
        /// <param name="hasSourceVideoFile">ソースビデオファイルの有無</param>
        /// <param name="sourceVideoId">ソースビデオID</param>
        /// <param name="state">ビデオの状態</param>
        /// <param name="privacy">プライバシーレベル</param>
        /// <param name="id">ビデオID</param>
        /// <param name="partition">パーティション</param>
        /// <param name="externalId">外部ID</param>
        /// <param name="owner">所有者</param>
        /// <param name="face">顔</param>
        /// <param name="animatedCharacter">アニメキャラクター</param>
        /// <param name="query">クエリ</param>
        /// <param name="textScope">テキストスコープ</param>
        /// <param name="language">言語</param>
        /// <param name="createdAfter">作成日（以降）</param>
        /// <param name="createdBefore">作成日（以前）</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップ数</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <returns>検索結果のビデオリスト</returns>
        public async Task<string> SearchVideosAsync(string location, string accountId, string? sourceLanguage = null, bool? hasSourceVideoFile = null, string? sourceVideoId = null, string[]? state = null, string[]? privacy = null, string[]? id = null, string[]? partition = null, string[]? externalId = null, string[]? owner = null, string[]? face = null, string[]? animatedCharacter = null, string[]? query = null, string[]? textScope = null, string[]? language = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string? accessToken = null)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            _logger.LogInformation($"Searching videos in account {accountId} with parameters: location={location}, sourceLanguage={sourceLanguage}, hasSourceVideoFile={hasSourceVideoFile}, sourceVideoId={sourceVideoId}, state={string.Join(",", state ?? Array.Empty<string>())}, privacy={string.Join(",", privacy ?? Array.Empty<string>())}, id={string.Join(",", id ?? Array.Empty<string>())}, partition={string.Join(",", partition ?? Array.Empty<string>())}, externalId={string.Join(",", externalId ?? Array.Empty<string>())}, owner={string.Join(",", owner ?? Array.Empty<string>())}, face={string.Join(",", face ?? Array.Empty<string>())}, animatedCharacter={string.Join(",", animatedCharacter ?? Array.Empty<string>())}, query={string.Join(",", query ?? Array.Empty<string>())}, textScope={string.Join(",", textScope ?? Array.Empty<string>())}, language={string.Join(",", language ?? Array.Empty<string>())}, createdAfter={createdAfter}, createdBefore={createdBefore}, pageSize={pageSize}, skip={skip}, accessToken={(string.IsNullOrEmpty(accessToken) ? "Not Provided" : "Provided")}");

            var url = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/Search?{(string.IsNullOrEmpty(sourceLanguage) ? "" : $"sourceLanguage={sourceLanguage}&")}{(hasSourceVideoFile.HasValue ? $"hasSourceVideoFile={hasSourceVideoFile.Value}&" : "")}{(string.IsNullOrEmpty(sourceVideoId) ? "" : $"sourceVideoId={sourceVideoId}&")}{(state != null && state.Length > 0 ? $"state={string.Join(",", state)}&" : "")}{(privacy != null && privacy.Length > 0 ? $"privacy={string.Join(",", privacy)}&" : "")}{(id != null && id.Length > 0 ? $"id={string.Join(",", id)}&" : "")}{(partition != null && partition.Length > 0 ? $"partition={string.Join(",", partition)}&" : "")}{(externalId != null && externalId.Length > 0 ? $"externalId={string.Join(",", externalId)}&" : "")}{(owner != null && owner.Length > 0 ? $"owner={string.Join(",", owner)}&" : "")}{(face != null && face.Length > 0 ? $"face={string.Join(",", face)}&" : "")}{(animatedCharacter != null && animatedCharacter.Length > 0 ? $"animatedcharacter={string.Join(",", animatedCharacter)}&" : "")}{(query != null && query.Length > 0 ? $"query={string.Join(",", query)}&" : "")}{(textScope != null && textScope.Length > 0 ? $"textScope={string.Join(",", textScope)}&" : "")}{(language != null && language.Length > 0 ? $"language={string.Join(",", language)}&" : "")}{(string.IsNullOrEmpty(createdAfter) ? "" : $"createdAfter={createdAfter}&")}{(string.IsNullOrEmpty(createdBefore) ? "" : $"createdBefore={createdBefore}&")}{(pageSize.HasValue ? $"pageSize={pageSize.Value}&" : "")}{(skip.HasValue ? $"skip={skip.Value}&" : "")}{(string.IsNullOrEmpty(accessToken) ? "" : $"accessToken={accessToken}&")}".TrimEnd('&');

            url = url.TrimEnd('&');

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Successfully searched videos: {jsonResponse}");
                return jsonResponse;
            }
            else
            {
                _logger.LogError($"Failed to search videos in account {accountId}. Status code: {response.StatusCode}");
                throw new Exception("Failed to search videos");
            }
        }

        /// <summary>
        /// VideoIndexer APIからビデオリストを取得する
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="accessToken">アクセス トークン</param>
        /// <param name="createdAfter">指定された日付以降に作成されたビデオをフィルタリング</param>
        /// <param name="createdBefore">指定された日付以前に作成されたビデオをフィルタリング</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップするレコード数</param>
        /// <param name="partitions">パーティションでビデオをフィルタリング</param>
        /// <returns>ビデオリストのJSONレスポンス</returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> ListVideosAsyncJson(string location, string accountId, string? accessToken = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null)
        {
            HttpClient client = _httpClientFactory.CreateClient();

            _logger.LogInformation($"Listing videos in account {accountId} with parameters: location={location}, createdAfter={createdAfter}, createdBefore={createdBefore}, pageSize={pageSize}, skip={skip}, partitions={string.Join(",", partitions ?? Array.Empty<string>())}, accessToken={(string.IsNullOrEmpty(accessToken) ? "Not Provided" : "Provided")}");

            var url = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos?{(string.IsNullOrEmpty(createdAfter) ? "" : $"createdAfter={createdAfter}&")}{(string.IsNullOrEmpty(createdBefore) ? "" : $"createdBefore={createdBefore}&")}{(pageSize.HasValue ? $"pageSize={pageSize.Value}&" : "")}{(skip.HasValue ? $"skip={skip.Value}&" : "")}{(partitions != null && partitions.Length > 0 ? $"partitions={string.Join(",", partitions)}&" : "")}{(string.IsNullOrEmpty(accessToken) ? "" : $"accessToken={accessToken}&")}".TrimEnd('&');

            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Successfully listed videos: {jsonResponse}");
                return jsonResponse;
            }
            else
            {
                _logger.LogError($"Failed to list videos in account {accountId}. Status code: {response.StatusCode}");
                throw new Exception("Failed to list videos");
            }
        }

        public async Task<IEnumerable<ApiVideoListItemModel>> ListVideosAsync(string location, string accountId, string? accessToken = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null)
        {
            var videoListJson = await ListVideosAsyncJson(location, accountId, accessToken: accessToken, createdAfter: createdAfter, createdBefore: createdBefore, pageSize: pageSize, skip: skip, partitions: partitions);
            return _videoListParser.ParseVideoList(videoListJson);
        }

    }
}
