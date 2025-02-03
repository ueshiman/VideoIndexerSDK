namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IVideoDownloadApiAccess
{
    Task<string> GetVideoDownloadUrl(string region, string accountId, string videoId, string accessToken);
}