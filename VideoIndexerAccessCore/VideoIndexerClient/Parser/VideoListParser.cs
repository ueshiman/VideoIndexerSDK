using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.Parser
{
    public class VideoListParser : IVideoListParser
    {
        public IEnumerable<ApiVideoListItemModel> ParseVideoList(string jsonResponse)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var videoList = JsonSerializer.Deserialize<ApiVideoListJsonModel>(jsonResponse, options);
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
