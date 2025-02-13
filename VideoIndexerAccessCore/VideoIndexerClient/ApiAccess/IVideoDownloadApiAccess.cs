namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IVideoDownloadApiAccess
{
    Task<string> GetVideoDownloadUrl(string region, string accountId, string videoId, string accessToken);
    Task<string> GetVideoThumbnailUrl(string location, string accountId, string videoId, string thumbnailId, string? format = null, string? accessToken = null);

}