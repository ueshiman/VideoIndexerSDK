using System;
using System.Collections.Generic;
using System.Text.Json;
using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient.Parser
{
    public class VideoListParser : IVideoListParser
    {
        public IEnumerable<VideoListItem> ParseVideoList(string jsonResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var videoList = JsonSerializer.Deserialize<VideoListJsonModel>(jsonResponse, options);
                return videoList?.results ?? [];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
