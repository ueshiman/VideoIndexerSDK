using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient.Model;
using VideoIndexPoc.VideoIndexerClient.Parser;

namespace VideoIndexPoc.VideoIndexerClient.ApiAccess
{
    public class VideoListAccess : IVideoListAccess
    {
        private readonly IVideoListParser _videoListParser;


        public VideoListAccess(IVideoListParser videoListParser)
        {
            _videoListParser = videoListParser;
        }

        public async Task<string> GetVideoListJsonAsync(string location, string accountId, string accessToken)
        {
            using HttpClient client = new HttpClient();
            // アクセストークンをクエリパラメータに追加
            // https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Videos
            var response = await client.GetAsync($"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Videos?accessToken={accessToken}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Failed to get video list");
            }
        }

        public IEnumerable<VideoListItem> GetVideoList(string location, string accountId, string accessToken)
        {
            var videoListJson = GetVideoListJsonAsync(location, accountId, accessToken).Result;
            return _videoListParser.ParseVideoList(videoListJson);
        }

        public async Task<IEnumerable<VideoListItem>> GetVideoListAsync(string location, string accountId, string accessToken)
        {
            var videoListJson = await GetVideoListJsonAsync(location, accountId, accessToken);
            return _videoListParser.ParseVideoList(videoListJson);
        }
    }
}
