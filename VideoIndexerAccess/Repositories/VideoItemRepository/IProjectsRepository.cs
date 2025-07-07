using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IProjectsRepository
{
    /// <summary>
    /// プロジェクトのレンダー操作をキャンセルします。
    /// アカウント情報を取得し、APIを呼び出してキャンセル処理を行います。
    /// </summary>
    /// <param name="projectId">プロジェクトID</param>
    /// <returns>キャンセル操作の結果モデル</returns>
    Task<ProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string projectId);

    /// <summary>
    /// プロジェクトのレンダー操作をキャンセルします。
    /// </summary>
    /// <param name="location">リージョン名</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>キャンセル操作の結果モデル</returns>
    Task<ProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// プロジェクトの作成とレンダー操作の進捗を非同期で逐次返却します。
    /// プロジェクト作成後、レンダー操作の状態をポーリングし、進捗ごとに BuildProjectResponseModel を yield return します。
    /// </summary>
    /// <param name="request">プロジェクト作成およびレンダー操作のリクエストモデル</param>
    /// <returns>進捗ごとに BuildProjectResponseModel を返す非同期イテレータ</returns>
    IAsyncEnumerable<BuildProjectResponseModel> BuildProjectAsync(BuildProjectRequestModel request);

    /// <summary>
    /// Video Indexer API で新しいプロジェクトを作成します。
    /// アカウント情報を取得し、APIを呼び出してプロジェクト作成処理を行います。
    /// </summary>
    /// <param name="request">プロジェクト作成リクエストモデル</param>
    /// <returns>作成されたプロジェクトの情報（ProjectModel）、失敗時はnull</returns>
    Task<ProjectModel?> CreateProjectAsync(CreateProjectRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクト作成リクエスト・アクセストークンでVideo Indexer APIにプロジェクト作成リクエストを送信し、
    /// 結果をProjectModelとして返します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="request">プロジェクト作成リクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>作成されたプロジェクトの情報（ProjectModel）、失敗時はnull</returns>
    Task<ProjectModel?> CreateProjectAsync(string location, string accountId, CreateProjectRequestModel request, string? accessToken = null);

    /// <summary>
    /// Video Indexer API でプロジェクトを削除します。
    /// アカウント情報を取得し、APIを呼び出してプロジェクト削除処理を行います。
    /// </summary>
    /// <param name="projectId">削除するプロジェクトのID</param>
    /// <returns>削除が成功した場合はtrue、それ以外はfalse</returns>
    Task<bool> DeleteProjectAsync(string projectId);

    /// <summary>
    /// Video Indexer API でプロジェクトを削除します。
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンでAPIを呼び出し、
    /// プロジェクトの削除を実行します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="projectId">削除するプロジェクトのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>削除が成功した場合はtrue、それ以外はfalse</returns>
    Task<bool> DeleteProjectAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からレンダリング済みプロジェクトのダウンロードURLを取得します。
    /// Download Project Rendered File Url
    /// </summary>
    /// <param name="projectId">ダウンロードURLを取得するプロジェクトのID</param>
    /// <returns>ダウンロードURL（取得できない場合はnull）</returns>
    Task<string?> GetProjectRenderedFileDownloadUrlAsync(string projectId);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からレンダリング済みプロジェクトのダウンロードURLを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="projectId">ダウンロードURLを取得するプロジェクトのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>ダウンロードURL（取得できない場合はnull）</returns>
    Task<string?> GetProjectRenderedFileDownloadUrlAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からプロジェクトのキャプションデータを取得します。
    /// </summary>
    /// <param name="request">
    /// プロジェクトキャプション取得リクエストモデル。
    /// <list type="bullet">
    /// <item><description>ProjectId: キャプションを取得するプロジェクトのID</description></item>
    /// <item><description>IndexId: （オプション）動画ID</description></item>
    /// <item><description>Format: （オプション）キャプションのフォーマット（Vtt / Ttml / Srt / Txt / Csv）</description></item>
    /// <item><description>Language: （オプション）キャプションの言語</description></item>
    /// <item><description>IncludeAudioEffects: （オプション）オーディオエフェクトを含めるか</description></item>
    /// <item><description>IncludeSpeakers: （オプション）スピーカー情報を含めるか</description></item>
    /// </list>
    /// </param>
    /// <returns>
    /// キャプションデータの文字列。指定されたフォーマット・言語・オプションに従って取得されます。
    /// エラーが発生した場合は null を返します。
    /// </returns>
    Task<string?> GetProjectCaptionsAsync(GetProjectCaptionsRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からプロジェクトのキャプションデータを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン（例: "japaneast" など）。</param>
    /// <param name="accountId">アカウントの一意の識別子（GUID形式）。</param>
    /// <param name="request">
    /// プロジェクトキャプション取得リクエストモデル。
    /// <list type="bullet">
    /// <item><description>ProjectId: キャプションを取得するプロジェクトのID</description></item>
    /// <item><description>IndexId: （オプション）動画ID</description></item>
    /// <item><description>Format: （オプション）キャプションのフォーマット（Vtt / Ttml / Srt / Txt / Csv）</description></item>
    /// <item><description>Language: （オプション）キャプションの言語</description></item>
    /// <item><description>IncludeAudioEffects: （オプション）オーディオエフェクトを含めるか</description></item>
    /// <item><description>IncludeSpeakers: （オプション）スピーカー情報を含めるか</description></item>
    /// </list>
    /// </param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）。指定しない場合は自動取得。</param>
    /// <returns>
    /// キャプションデータの文字列。指定されたフォーマット・言語・オプションに従って取得されます。
    /// エラーが発生した場合は null を返します。
    /// </returns>
    Task<string?> GetProjectCaptionsAsync(string location, string accountId, GetProjectCaptionsRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・インデックス取得リクエスト・アクセストークンで
    /// Video Indexer API からプロジェクトのインデックス情報を取得します。
    /// </summary>
    /// <param name="request">プロジェクトインデックス取得リクエストモデル</param>
    /// <returns>取得したプロジェクトのインデックス情報（ApiProjectIndexModel）</returns>
    Task<ApiProjectIndexModel> GetProjectIndexAsync(GetProjectIndexRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・インデックス取得リクエスト・アクセストークンで
    /// Video Indexer API からプロジェクトのインデックス情報を取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="request">プロジェクトインデックス取得リクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>取得したプロジェクトのインデックス情報（ApiProjectIndexModel）</returns>
    Task<ApiProjectIndexModel> GetProjectIndexAsync(string location, string accountId, GetProjectIndexRequestModel request, string? accessToken = null);

    /// <summary>
    /// Video Indexer API からプロジェクトのインサイトウィジェットURLを取得します。
    /// アカウント情報を取得し、APIを呼び出してウィジェットURLを取得します。
    /// </summary>
    /// <param name="request">インサイトウィジェット取得リクエストモデル（ProjectId, WidgetTypeを含む）</param>
    /// <returns>インサイトウィジェットのURL文字列</returns>
    Task<string> GetProjectInsightsWidgetAsync(GetProjectInsightsWidgetRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・ウィジェットタイプ・アクセストークンで
    /// Video Indexer API からプロジェクトのインサイトウィジェットURLを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="request">インサイトウィジェット取得リクエストモデル（ProjectId, WidgetTypeを含む）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>インサイトウィジェットのURL文字列</returns>
    Task<string> GetProjectInsightsWidgetAsync(string location, string accountId, GetProjectInsightsWidgetRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からプロジェクトのプレイヤーウィジェットURLを取得します。
    /// </summary>
    /// <param name="projectId">プレイヤーウィジェットURLを取得するプロジェクトのID</param>
    /// <returns>プレイヤーウィジェットのURL文字列</returns>
    Task<string> GetProjectPlayerWidgetAsync(string projectId);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からプロジェクトのプレイヤーウィジェットURLを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="projectId">プレイヤーウィジェットURLを取得するプロジェクトのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>プレイヤーウィジェットのURL文字列</returns>
    Task<string> GetProjectPlayerWidgetAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// アカウント情報とプロジェクトIDからVideo Indexer APIのレンダー操作情報を取得します。
    /// </summary>
    /// <param name="projectId">レンダー操作情報を取得するプロジェクトのID</param>
    /// <returns>取得したレンダー操作情報（ProjectRenderOperationModel）</returns>
    Task<ProjectRenderOperationModel?> GetProjectRenderOperationAsync(string projectId);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・アクセストークンで
    /// Video Indexer API からプロジェクトのレンダー操作情報を取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="projectId">レンダー操作情報を取得するプロジェクトのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>取得したレンダー操作情報（ApiProjectRenderOperationModel）</returns>
    Task<ApiProjectRenderOperationModel> GetProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// レンダー操作の状態からリトライ可能かどうかを判定します。
    /// </summary>
    /// <param name="operationModel">レンダー操作モデル</param>
    /// <returns>リトライ可能な状態の場合は true、それ以外は false</returns>
    bool IsRetry(ApiProjectRenderOperationModel operationModel);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
    /// Video Indexer API からプロジェクトのサムネイル画像データ（バイナリストリーム）を取得します。
    /// </summary>
    /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
    /// <returns>サムネイル画像のバイナリストリーム</returns>
    Task<Stream> GetProjectThumbnailBitsAsync(ProjectThumbnailRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
    /// Video Indexer API からプロジェクトのサムネイル画像のURLを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>サムネイル画像のバイナリストリーム</returns>
    Task<Stream> GetProjectThumbnailBitsAsync(string location, string accountId, ProjectThumbnailRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
    /// Video Indexer API からプロジェクトのサムネイル画像データ（バイナリストリーム）を取得します。
    /// </summary>
    /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
    /// <returns>サムネイル画像のURL文字列</returns>
    Task<string> GetProjectThumbnailUrlAsync(ProjectThumbnailRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・プロジェクトID・サムネイルID・フォーマット・アクセストークンで
    /// Video Indexer API からプロジェクトのサムネイル画像のURLを取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン</param>
    /// <param name="accountId">アカウントの一意の識別子</param>
    /// <param name="request">サムネイル取得リクエストモデル（ProjectId, ThumbnailId, Formatを含む）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
    /// <returns>サムネイル画像のURL文字列</returns>
    Task<string> GetProjectThumbnailUrlAsync(string location, string accountId, ProjectThumbnailRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された条件でプロジェクト一覧を取得します。
    /// </summary>
    /// <param name="request">プロジェクト一覧取得リクエストモデル。</param>
    /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
    Task<ProjectSearchResultModel> GetProjectsAsync(ProjectsRequestModel request);

    /// <summary>
    /// 指定された条件でプロジェクト一覧を取得します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="request">プロジェクト一覧取得リクエストモデル。</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
    /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
    Task<ProjectSearchResultModel> GetProjectsAsync(string location, string accountId, ProjectsRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのレンダリングを開始します。
    /// </summary>
    /// <param name="request">レンダリングリクエストモデル。</param>
    /// <returns>レンダリング結果のレスポンスモデル。</returns>
    Task<ProjectRenderResponseModel> RenderProjectAsync(RenderProjectRequestModel request);

    /// <summary>
    /// 指定されたプロジェクトのレンダリングを開始します。
    /// </summary>
    /// <param name="location">API呼び出しのAzureリージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="request">レンダリングリクエストモデル。</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
    /// <returns>レンダリング結果のレスポンスモデル。</returns>
    Task<ProjectRenderResponseModel> RenderProjectAsync(string location, string accountId, RenderProjectRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された検索条件でプロジェクトを検索します。
    /// </summary>
    /// <param name="request">検索条件を含むリクエストモデル。</param>
    /// <returns>検索結果のレスポンスモデル。</returns>
    Task<ProjectSearchResultModel> SearchProjectsAsync(SearchProjectsRequestModel request);

    /// <summary>
    /// 指定された検索条件でプロジェクトを検索します。
    /// </summary>
    /// <param name="location">Azureリージョン。</param>
    /// <param name="accountId">アカウントのGUID。</param>
    /// <param name="request">検索条件を含むリクエストモデル。</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
    /// <returns>検索結果のレスポンスモデル。</returns>
    Task<ProjectSearchResultModel> SearchProjectsAsync(string location, string accountId, SearchProjectsRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトの情報を更新します。
    /// </summary>
    /// <param name="updateRequest">更新するプロジェクトのリクエストモデル。</param>
    /// <returns>更新されたプロジェクトのレスポンスモデル。</returns>
    Task<ProjectUpdateResponseModel> UpdateProjectAsync(ProjectUpdateRequestModel updateRequest);

    /// <summary>
    /// 指定されたプロジェクトの情報を更新します。
    /// </summary>
    /// <param name="location">Azureリージョン。</param>
    /// <param name="accountId">アカウントのGUID。</param>
    /// <param name="updateRequest">更新するプロジェクトのリクエストモデル。</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可能）。</param>
    /// <returns>更新されたプロジェクトのレスポンスモデル。</returns>
    Task<ApiProjectUpdateResponseModel> UpdateProjectAsync(string location, string accountId, ProjectUpdateRequestModel updateRequest, string? accessToken = null);
}