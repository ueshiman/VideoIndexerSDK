using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IVideoListRepository
{
    /// <summary>
    /// ビデオリストのJSONを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <returns>ビデオリストのJSONレスポンス</returns>
    Task<string> GetVideoListJsonAsync(string? location, string? accountId);

    /// <summary>
    /// ビデオリストを非同期で取得し、パースして返す
    /// </summary>
    /// <param name="createdAfter">指定された日付以降に作成されたビデオをフィルタリング</param>
    /// <param name="createdBefore">指定された日付以前に作成されたビデオをフィルタリング</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップするレコード数</param>
    /// <param name="partitions">パーティションでビデオをフィルタリング</param>
    /// <returns>ビデオリスト</returns>
    Task<IEnumerable<VideoListDataModel>> ListVideosAsync(DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null);

    /// <summary>
    /// 指定された条件でビデオを検索する
    /// </summary>
    /// <param name="sourceLanguage">ソース言語</param>
    /// <param name="hasSourceVideoFile">ソースビデオファイルの有無</param>
    /// <param name="sourceVideoId">ソースビデオID</param>
    /// <param name="state">ビデオの状態</param>
    /// <param name="privacy">プライバシーレベル</param>
    /// <param name="id">ビデオID</param>
    /// <param name="partition">パーティション</param>
    /// <param name="externalId">外部ID</param>
    /// <param name="owner">所有者</param>
    /// <param name="face">顔</param>
    /// <param name="animatedCharacter">アニメキャラクター</param>
    /// <param name="query">クエリ</param>
    /// <param name="textScope">テキストスコープ</param>
    /// <param name="language">言語</param>
    /// <param name="createdAfter">作成日（以降）</param>
    /// <param name="createdBefore">作成日（以前）</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップ数</param>
    /// <returns>検索結果のビデオリストのJSONレスポンス</returns>
    Task<string> SearchVideosAsync(string? sourceLanguage = null, bool? hasSourceVideoFile = null, string? sourceVideoId = null, string[]? state = null, string[]? privacy = null, string[]? id = null, string[]? partition = null, string[]? externalId = null, string[]? owner = null, string[]? face = null, string[]? animatedCharacter = null, string[]? query = null, string[]? textScope = null, string[]? language = null, DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null);
}