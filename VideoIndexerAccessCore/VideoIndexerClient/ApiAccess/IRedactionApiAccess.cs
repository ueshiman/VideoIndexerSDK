using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IRedactionApiAccess
{
    /// <summary>
    /// API でビデオ編集 (Redact) をRequestします。
    /// Redact Video
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="request">ビデオの編集リクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>pe成功時はture</returns>
    Task<bool> FetchRedactVideoJsonAsync(string location, string accountId, string videoId, ApiRedactVideoRequestModel request, string? accessToken = null);

    /// <summary>
    /// 取得した JSON をパースして ApiRedactVideoResponseModel オブジェクトに変換します。
    /// Redact Video
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースした ApiRedactVideoResponseModel オブジェクト、エラー時は null</returns>
    ApiRedactVideoResponseModel? ParseRedactVideoJson(string json);

    /// <summary>
    /// API を呼び出してビデオの編集 (Redact) を開始します。
    /// Redact Video
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Redact-Video
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="request">ビデオの編集リクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>pe成功時はture</returns>
    Task<bool> RedactVideoAsync(string location, string accountId, string videoId, ApiRedactVideoRequestModel request, string? accessToken = null);
}