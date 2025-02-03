using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient.Model;
using Microsoft.Extensions.Logging;
namespace VideoIndexPoc.VideoIndexerClient.Parser
{
    public class VideoItemParser : IVideoItemParser
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public VideoItem ParseVideoItem(string jsonResponse)
        {
            App.Logger.LogInformation($"Video item json : {jsonResponse}");

            try
            {
                var videoItem = JsonSerializer.Deserialize<VideoItem>(jsonResponse, Options);
                return videoItem ?? new VideoItem();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
