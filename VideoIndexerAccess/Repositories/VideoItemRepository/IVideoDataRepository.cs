namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IVideoDataRepository
{
    /// <summary>
    /// ビデオのダウンロードURLを取得する
    /// </summary>
    /// <param name="videoId">ビデオID</param>
    /// <returns>ビデオのダウンロードURL</returns>
    Task<string> GetVideoDownloadUrl( string videoId);
}