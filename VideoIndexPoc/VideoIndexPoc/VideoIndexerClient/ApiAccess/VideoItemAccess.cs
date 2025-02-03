using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient.Model;
using VideoIndexPoc.VideoIndexerClient.Parser;


namespace VideoIndexPoc.VideoIndexerClient.ApiAccess
{
    public class VideoItemAccess : IVideoItemAccess
    {
        private readonly IVideoItemParser _videoItemParser;


        public VideoItemAccess(IVideoItemParser videoItemParser)
        {
            _videoItemParser = videoItemParser;
        }

        public async Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId, string accessToken)
        {
            using HttpClient client = new HttpClient();
            // アクセストークンをクエリパラメータに追加
            // Get Video Index
            // Get-Video-Index
            var response = await client.GetAsync($"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos/{videoId}/Index?accessToken={accessToken}");


            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Failed to get video list");
            }
        }

        public VideoItem GetVideoItem(string location, string accountId, string videoId, string accessToken)
        {
            var videoListJson = GetVideoItemJsonAsync(location, accountId, videoId, accessToken).Result;
            return _videoItemParser.ParseVideoItem(videoListJson);
        }

        public async Task<VideoItem> GetVideoItemAsync(string location, string accountId, string videoId, string accessToken)
        {
            var videoListJson = await GetVideoItemJsonAsync(location, accountId, videoId, accessToken);
            return _videoItemParser.ParseVideoItem(videoListJson);
        }

    }
}
