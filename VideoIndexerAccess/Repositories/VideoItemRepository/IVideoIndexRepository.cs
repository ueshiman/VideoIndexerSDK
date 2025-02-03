using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

/// <summary>
/// ビデオインデックスリポジトリのインターフェース
/// </summary>
public interface IVideoIndexRepository
{
    /// <summary>
    /// ビデオアイテムのJSONを非同期で取得する
    /// </summary>
    /// <param name="videoId">ビデオ ID</param>
    /// <returns>ビデオアイテムのJSONレスポンス</returns>
    Task<string> GetVideoItemJsonAsync(string videoId);

    /// <summary>
    /// ビデオアイテムを非同期で取得する
    /// </summary>
    /// <param name="videoId">ビデオ ID</param>
    /// <returns>ビデオアイテム</returns>

    Task<VideoItemDataModel> GetVideoItemAsync(string videoId);

}
