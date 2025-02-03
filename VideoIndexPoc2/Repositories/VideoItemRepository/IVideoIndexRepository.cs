using System.Threading.Tasks;
using VideoInexerAccessCore.VideoIndexerClient.Model;

namespace VideoIndexPoc2.Repositories.VideoItemRepository;

/// <summary>
/// ビデオインデックスリポジトリのインターフェース
/// </summary>
public interface IVideoIndexRepository
{
    /// <summary>
    /// ビデオアイテムのJSONを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <returns>ビデオアイテムのJSONレスポンス</returns>
    Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId);

    /// <summary>
    /// ビデオアイテムを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <returns>ビデオアイテム</returns>
    Task<VideoItem> GetVideoItemAsync(string location, string accountId, string videoId);
}
