using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IWidgetsApiAccess
{
    /// <summary>
    /// 外部公開用メソッド。Insights Widgetの情報を取得します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">インサイトを取得する対象のビデオID</param>
    /// <param name="widgetType">（任意）表示するウィジェットの種類（People / Sentiments / Keywords / Search）</param>
    /// <param name="allowEdit">（任意）ウィジェットが編集可能かどうか</param>
    /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
    /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
    Task<ApiVideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(string location, string accountId, string videoId, string? widgetType = null, bool? allowEdit = null, string? accessToken = null);

    /// <summary>
    /// Insights Widget API にアクセスし JSON レスポンスを取得します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">インサイトを取得する対象のビデオID</param>
    /// <param name="widgetType">（任意）表示するウィジェットの種類（People / Sentiments / Keywords / Search）</param>
    /// <param name="allowEdit">（任意）ウィジェットが編集可能かどうか</param>
    /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
    /// <returns>APIから取得した生のJSONレスポンス文字列</returns>
    Task<string> FetchVideoInsightsJsonAsync(string location, string accountId, string videoId, string? widgetType, bool? allowEdit, string? accessToken);

    /// <summary>
    /// JSONレスポンスをオブジェクトに変換します。
    /// </summary>
    /// <param name="json">APIから取得したJSON文字列</param>
    /// <returns>デシリアライズされた <see cref="ApiVideoInsightsWidgetResponseModel"/> オブジェクト。失敗時は null</returns>
    ApiVideoInsightsWidgetResponseModel? ParseVideoInsightsJson(string json);

    /// <summary>
    /// 外部公開用メソッド。Video Player Widget の情報を取得します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">対象のビデオID</param>
    /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
    /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
    Task<ApiVideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(
        string location,
        string accountId,
        string videoId,
        string? accessToken = null);
}