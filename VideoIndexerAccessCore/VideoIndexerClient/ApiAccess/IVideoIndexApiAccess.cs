using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

/// <summary>
/// ビデオアイテムAPIへのアクセスを提供するインターフェース
/// </summary>
public interface IVideoItemApiAccess
{
    /// <summary>
    /// ビデオアイテムのJSONを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <returns>ビデオアイテムのJSONレスポンス</returns>
    Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId, string accessToken);

    /// <summary>
    /// ビデオインデックスのJSONを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <param name="language">言語</param>
    /// <param name="reTranslate">再翻訳するかどうか</param>
    /// <param name="includeStreamingUrls">ストリーミングURLを含めるかどうか</param>
    /// <param name="includedInsights">含めるインサイト</param>
    /// <param name="excludedInsights">除外するインサイト</param>
    /// <returns>ビデオインデックスのJSONレスポンス</returns>
    Task<string> GetVideoIndexJsonAsync(string location, string accountId, string videoId, string? accessToken = null, string? language = null, bool? reTranslate = null, bool? includeStreamingUrls = null, string? includedInsights = null, string? excludedInsights = null);

    /// <summary>
    /// ビデオインデックスを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <param name="language">言語</param>
    /// <param name="reTranslate">再翻訳するかどうか</param>
    /// <param name="includeStreamingUrls">ストリーミングURLを含めるかどうか</param>
    /// <param name="includedInsights">含めるインサイト</param>
    /// <param name="excludedInsights">除外するインサイト</param>
    /// <returns>ビデオインデックス</returns>
    Task<VideoItemApiModel> GetVideoIndexAsync(string location, string accountId, string videoId, string? accessToken = null, string? language = null, bool? reTranslate = null, bool? includeStreamingUrls = null, string? includedInsights = null, string? excludedInsights = null);

    /// <summary>
    /// ビデオアイテムを取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <returns>ビデオアイテム</returns>
    VideoItemApiModel GetVideoItem(string location, string accountId, string videoId, string accessToken);

    /// <summary>
    /// ビデオアイテムを非同期で取得する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <returns>ビデオアイテム</returns>
    Task<VideoItemApiModel> GetVideoItemAsync(string location, string accountId, string videoId, string accessToken);
}
