using System.Threading.Tasks;

namespace VideoIndexPoc.VideoIndexerClient.ApiAccess;

public interface IVideoDownload
{
    Task<string>GetVideoDownloadUrl(string accountId, string videoId, string accessToken, string region);
}