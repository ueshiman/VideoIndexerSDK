using System.Collections.Generic;
using System.Threading.Tasks;
using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.VideoIndexerClient.ApiAccess;

public interface IVideoListAccess
{
    Task<string> GetVideoListJsonAsync(string location, string accountId, string accessToken);
    IEnumerable<VideoListItem> GetVideoList(string location, string accountId, string accessToken);
    Task<IEnumerable<VideoListItem>> GetVideoListAsync(string location, string accountId, string accessToken);
}