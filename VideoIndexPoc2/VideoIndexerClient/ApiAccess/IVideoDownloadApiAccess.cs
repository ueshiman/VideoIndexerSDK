using System.Threading.Tasks;

namespace VideoIndexPoc2.VideoIndexerClient.ApiAccess;

public interface IVideoDownloadApiAccess
{
    Task<string>GetVideoDownloadUrl(string accountId, string videoId, string accessToken, string region);
}