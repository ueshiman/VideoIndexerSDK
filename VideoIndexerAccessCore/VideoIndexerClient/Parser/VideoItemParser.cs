using System.Text.Json;
using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.Parser
{
    public class VideoItemParser : IVideoItemParser
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly ILogger<VideoItemParser> _logger;

        public VideoItemParser(ILogger<VideoItemParser> logger)
        {
            _logger = logger;
        }

        public VideoItemApiModel ParseVideoItem(string jsonResponse)
        {
            _logger.LogInformation($"Video item json : {jsonResponse}");

            try
            {
                var videoItem = JsonSerializer.Deserialize<VideoItemApiModel>(jsonResponse, Options);
                return videoItem ?? new VideoItemApiModel();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error parsing video item");
                throw;
            }
        }
    }
}
