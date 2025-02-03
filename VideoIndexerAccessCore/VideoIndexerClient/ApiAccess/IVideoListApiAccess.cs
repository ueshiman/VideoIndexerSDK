using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IVideoListApiAccess
{
    /// <summary>
    /// 非同期でビデオリストをJSON形式で取得する
    /// </summary>
    /// <param name="location">ビデオの場所</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <returns>ビデオリストのJSON文字列</returns>
    Task<string> GetVideoListJsonAsync(string? location, string? accountId, string? accessToken = null);

    ///// <summary>
    ///// ビデオリストを列挙する
    ///// </summary>
    ///// <param name="location">ビデオの場所</param>
    ///// <param name="accountId">アカウントID</param>
    ///// <param name="accessToken">アクセストークン</param>
    ///// <returns>ビデオリスト</returns>
    //IEnumerable<VideoListItem> ListVideos(string? location, string? accountId, string? accessToken);

    ///// <summary>
    ///// 非同期でビデオリストを列挙する
    ///// </summary>
    ///// <param name="location">ビデオの場所</param>
    ///// <param name="accountId">アカウントID</param>
    ///// <param name="accessToken">アクセストークン</param>
    ///// <returns>ビデオリスト</returns>
    //Task<IEnumerable<VideoListItem>> ListVideosAsync(string? location, string? accountId, string? accessToken);

    /// <summary>
    /// 非同期でビデオを検索する
    /// </summary>
    /// <param name="location">ビデオの場所</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="sourceLanguage">ソース言語</param>
    /// <param name="hasSourceVideoFile">ソースビデオファイルの有無</param>
    /// <param name="sourceVideoId">ソースビデオID</param>
    /// <param name="state">状態</param>
    /// <param name="privacy">プライバシー</param>
    /// <param name="id">ID</param>
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
    /// <param name="skip">スキップ</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <returns>検索結果のJSON文字列</returns>
    Task<string> SearchVideosAsync(string location, string accountId, string? sourceLanguage = null, bool? hasSourceVideoFile = null, string? sourceVideoId = null, string[]? state = null, string[]? privacy = null, string[]? id = null, string[]? partition = null, string[]? externalId = null, string[]? owner = null, string[]? face = null, string[]? animatedCharacter = null, string[]? query = null, string[]? textScope = null, string[]? language = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string? accessToken = null);

    /// <summary>
    /// 非同期でビデオリストをJSON形式で取得する（フィルタ付き）
    /// </summary>
    /// <param name="location">ビデオの場所</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <param name="createdAfter">作成日（以降）</param>
    /// <param name="createdBefore">作成日（以前）</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップ</param>
    /// <param name="partitions">パーティション</param>
    /// <returns>ビデオリストのJSON文字列</returns>
    Task<string> ListVideosAsyncJson(string location, string accountId, string? accessToken = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null);

    /// <summary>
    /// 非同期でビデオリストを列挙する（フィルタ付き）
    /// </summary>
    /// <param name="location">ビデオの場所</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <param name="createdAfter">作成日（以降）</param>
    /// <param name="createdBefore">作成日（以前）</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップ</param>
    /// <param name="partitions">パーティション</param>
    /// <returns>ビデオリスト</returns>
    Task<IEnumerable<ApiVideoListItemModel>> ListVideosAsync(string location, string accountId, string? accessToken = null, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null);
}
