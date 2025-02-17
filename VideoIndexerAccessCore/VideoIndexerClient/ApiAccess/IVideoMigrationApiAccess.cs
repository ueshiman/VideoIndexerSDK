using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IVideoMigrationApiAccess
{
    /// <summary>
    /// Web API にリクエストを送信し、レスポンスを文字列で取得する。
    /// </summary>
    Task<string> FetchJsonResponseAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// ビデオの移行情報を取得する。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="videoId">ビデオID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ビデオの移行状況</returns>
    Task<ApiVideoMigrationModel?> GetVideoMigrationAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// ビデオ移行の一覧を取得する。
    /// </summary>
    Task<ApiVideoMigrationsListModel?> GetVideoMigrationsAsync(string location, string accountId, int? pageSize = null, int? skip = null, List<string>? states = null, string? accessToken = null);

    /// <summary>
    /// WebAPIからビデオ移行データを取得する。
    /// </summary>
    Task<string> FetchVideoMigrationsJsonAsync(string location, string accountId, int? pageSize = null, int? skip = null, List<string>? states = null, string? accessToken = null);

    /// <summary>
    /// JSONレスポンスを解析する。
    /// </summary>
    ApiVideoMigrationModel? ParseJsonResponse(string json);
}