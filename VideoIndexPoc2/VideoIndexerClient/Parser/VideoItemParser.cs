using System;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient.Parser
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
