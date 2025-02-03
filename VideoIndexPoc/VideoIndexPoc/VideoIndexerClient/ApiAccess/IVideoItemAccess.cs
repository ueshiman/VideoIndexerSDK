using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.VideoIndexerClient.ApiAccess;

public interface IVideoItemAccess
{
    Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId, string accessToken);
    VideoItem GetVideoItem(string location, string accountId, string videoId, string accessToken);
    Task<VideoItem> GetVideoItemAsync(string location, string accountId, string videoId, string accessToken);
}