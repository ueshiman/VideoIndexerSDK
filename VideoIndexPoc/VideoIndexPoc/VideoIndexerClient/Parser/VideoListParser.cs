using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexPoc.Windows;
using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.VideoIndexerClient.Parser
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
