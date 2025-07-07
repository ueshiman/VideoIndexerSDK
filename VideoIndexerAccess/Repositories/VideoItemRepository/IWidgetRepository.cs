using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IWidgetRepository
{
    Task<string?> GetVideoInsightsWidgetUrl(GetVideoInsightsWidgetResponseModel requestModel);

    /// <summary>
    /// Insights Widget のURLを取得します。
    /// Video Indexer API から Insights Widget のリダイレクトURLを取得し返却します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="requestModel"></param>
    /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
    /// <returns>リダイレクトされた Insights Widget のURL。失敗時は null</returns>
    Task<string?> GetVideoInsightsWidgetUrl(string location, string accountId, GetVideoInsightsWidgetResponseModel requestModel, string? accessToken = null);

    /// <summary>
    /// Insights Widget の情報を取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を行い、Insights Widget API から情報を取得します。
    /// </summary>
    /// <param name="requestModel">ウィジェット情報取得用リクエストモデル</param>
    /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
    Task<VideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(GetVideoInsightsWidgetResponseModel requestModel);

    /// <summary>
    /// Insights Widget の情報を取得します。
    /// </summary>
    /// <param name="requestModel">ウィジェット情報取得用リクエストモデル</param>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
    /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
    Task<VideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(string location, string accountId, GetVideoInsightsWidgetResponseModel requestModel, string? accessToken = null);

    /// <summary>
    /// Video Player Widget のURLを取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を行い、Video Player Widget API からリダイレクトURLを取得します。
    /// </summary>
    /// <param name="videoId">対象のビデオID</param>
    /// <returns>リダイレクトされた Player Widget のURL。失敗時は null</returns>
    Task<string?> GetVideoPlayerWidgetUrl(string videoId);

    /// <summary>
    /// Video Player Widget のリダイレクトURLを取得します。
    /// Video Indexer API から Player Widget のリダイレクトURLを取得し返却します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">対象のビデオID</param>
    /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
    /// <returns>リダイレクトされた Player Widget のURL。失敗時は null</returns>
    Task<string?> GetVideoPlayerWidgetUrl(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// Video Player Widget の情報を取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を行い、Video Player Widget API から情報を取得します。
    /// </summary>
    /// <param name="videoId">対象のビデオID</param>
    /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
    Task<VideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(string videoId);

    /// <summary>
    /// Video Player Widget の情報を取得します。
    /// </summary>
    /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">対象のビデオID</param>
    /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
    /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
    Task<VideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(string location, string accountId, string videoId, string? accessToken = null);
}