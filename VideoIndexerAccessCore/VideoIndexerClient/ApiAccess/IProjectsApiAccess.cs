using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IProjectsApiAccess
{
    /// <summary>
    /// Video Indexer API でプロジェクトのレンダー操作をキャンセルします。
    /// Cancel Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">プロジェクトの一意の識別子。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>キャンセルが成功した場合は ApiProjectRenderOperationModel オブジェクト、それ以外は null を返します。</returns>
    Task<ApiProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に POST リクエストを送信し、レンダー操作をキャンセルします。
    /// Cancel Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
    /// </summary>
    /// <returns>JSON レスポンスを文字列として返します。</returns>
    Task<string> SendPostRequestForCancelRenderAsync(string location, string accountId, string projectId, string? accessToken);

    /// <summary>
    /// JSON 文字列を ApiProjectRenderOperationModel オブジェクトにパースします。
    /// Cancel Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
    /// </summary>
    /// <param name="json">パースする JSON 文字列。</param>
    /// <returns>パースに成功した場合は ApiProjectRenderOperationModel オブジェクト、それ以外は null を返します。</returns>
    ApiProjectRenderOperationModel? ParseRenderOperationJson(string json);

    /// <summary>
    /// Video Indexer API で新しいプロジェクトを作成します。
    /// Create Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectName">作成するプロジェクトの名前。</param>
    /// <param name="videoRanges">プロジェクトに含めるビデオ範囲リスト。各ビデオの ID と時間範囲を含む。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
    /// <returns>作成されたプロジェクトの情報を含む <see cref="ApiProjectModel"/> オブジェクト、それ以外は null を返します。</returns>
    Task<ApiProjectModel?> CreateProjectAsync(string location, string accountId, string projectName, List<ApiVideoTimeRangeModel> videoRanges, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に POST リクエストを送信し、新しいプロジェクトを作成します。
    /// Create Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectName">作成するプロジェクトの名前。</param>
    /// <param name="videoRanges">プロジェクトに含めるビデオ範囲リスト。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>API からの JSON レスポンスを文字列として返します。</returns>
    Task<string> SendPostRequestForCreateProjectAsync(string location, string accountId, string projectName, List<ApiVideoTimeRangeModel> videoRanges, string? accessToken);

    /// <summary>
    /// JSON 文字列を <see cref="ApiProjectModel"/> オブジェクトにパースします。
    /// Create Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
    /// </summary>
    /// <param name="json">パースする JSON 文字列。API から返されたレスポンス。</param>
    /// <returns>パースに成功した場合は <see cref="ApiProjectModel"/> オブジェクト、それ以外は null を返します。</returns>
    ApiProjectModel? ParseProjectJson(string json);

    /// <summary>
    /// Video Indexer API でプロジェクトを削除します。
    /// Delete Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Project
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">削除するプロジェクトの ID。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
    /// <returns>削除が成功した場合は true、それ以外は false を返します。</returns>
    Task<bool> DeleteProjectAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に DELETE リクエストを送信し、プロジェクトを削除します。
    /// Delete Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Project
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">削除するプロジェクトの ID。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    Task SendDeleteRequestForProjectAsync(string location, string accountId, string projectId, string? accessToken);

    /// <summary>
    /// Video Indexer API からレンダリングされたプロジェクトのダウンロード URL を取得します。
    /// Download Project Rendered File Url
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">ダウンロード URL を取得するプロジェクトの ID。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
    /// <returns>取得したダウンロード URL を含む文字列、それ以外は null を返します。</returns>
    Task<string?> GetProjectRenderedFileDownloadUrlAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に GET リクエストを送信し、レンダリングされたプロジェクトのダウンロード URL を取得します。
    /// Download Project Rendered File Url
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
    /// </summary>
    Task<string> SendGetRequestForRenderedFileUrlAsync(string location, string accountId, string projectId, string? accessToken);

    /// <summary>
    /// JSON 文字列を解析し、ダウンロード URL を取得します。
    /// Download Project Rendered File Url
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
    /// </summary>
    /// <param name="json">パースする JSON 文字列。</param>
    /// <returns>ダウンロード URL を含む文字列、それ以外は null を返します。</returns>
    string? ParseDownloadUrlJson(string json);

    /// <summary>
    /// Video Indexer API を使用してプロジェクトのキャプションを取得します。
    /// 指定されたパラメータに基づき、特定の形式や言語でキャプションを取得できます。
    /// Get Project Captions
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Captions
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">キャプションを取得するプロジェクトの ID。</param>
    /// <param name="indexId">オプションの動画 ID。</param>
    /// <param name="format">キャプションのフォーマット (Vtt / Ttml / Srt / Txt / Csv)。</param>
    /// <param name="language">キャプションの言語。</param>
    /// <param name="includeAudioEffects">オーディオエフェクトを含めるかどうか。</param>
    /// <param name="includeSpeakers">スピーカー情報を含めるかどうか。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>
    /// キャプションデータの文字列。指定されたフォーマットとオプションに従って取得されます。
    /// エラーが発生した場合は null を返します。
    /// </returns>
    Task<string?> GetProjectCaptionsAsync(string location, string accountId, string projectId, string? indexId = null, string? format = null, string? language = null, bool? includeAudioEffects = null, bool? includeSpeakers = null, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に GET リクエストを送信し、プロジェクトのキャプションを取得します。
    /// 指定されたパラメータを基に API へリクエストを行い、キャプションデータを取得します。
    /// Get Project Captions
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Captions
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="projectId">キャプションを取得するプロジェクトの ID。</param>
    /// <param name="indexId">オプションの動画 ID。</param>
    /// <param name="format">キャプションのフォーマット (Vtt / Ttml / Srt / Txt / Csv)。</param>
    /// <param name="language">キャプションの言語。</param>
    /// <param name="includeAudioEffects">オーディオエフェクトを含めるかどうか。</param>
    /// <param name="includeSpeakers">スピーカー情報を含めるかどうか。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>
    /// API からのレスポンスを文字列として返します。
    /// 成功した場合はキャプションデータ、失敗した場合は例外がスローされます。
    /// </returns>
    Task<string> SendGetRequestForProjectCaptionsAsync(string location, string accountId, string projectId, string? indexId, string? format, string? language, bool? includeAudioEffects, bool? includeSpeakers, string? accessToken);

    /// <summary>
    /// 指定されたプロジェクトのインデックスを取得する。
    /// Get Project Index
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
    /// </summary>
    /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
    /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
    /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
    /// <param name="language">(オプション) 動画インサイトを翻訳する言語コード。</param>
    /// <param name="reTranslate">(オプション) 既存の翻訳を上書きするかどうかのフラグ。</param>
    /// <param name="includedInsights">(オプション) 含めるインサイトの種類（カンマ区切り）。</param>
    /// <param name="excludedInsights">(オプション) 除外するインサイトの種類（カンマ区切り）。</param>
    /// <param name="includeSummarizedInsights">(オプション) SummarizedInsights を含めるかどうかのフラグ。</param>
    /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
    /// <returns>取得したプロジェクトのインデックス情報を表す <see cref="ApiProjectIndexModel"/> オブジェクト。</returns>
    Task<ApiProjectIndexModel> GetProjectIndexAsync(string location, string accountId, string projectId, string? language = null, bool? reTranslate = null, string? includedInsights = null, string? excludedInsights = null, bool? includeSummarizedInsights = null, string? accessToken = null);

    /// <summary>
    /// API へリクエストを送信し、JSON レスポンスを取得する。
    /// Get Project Index
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
    /// </summary>
    /// <returns>API のレスポンス（JSON 形式）を文字列として返す。</returns>
    Task<string> FetchProjectIndexJsonAsync(string location, string accountId, string projectId, string? language, bool? reTranslate, string? includedInsights, string? excludedInsights, bool? includeSummarizedInsights, string? accessToken);

    /// <summary>
    /// JSON レスポンスをパースし、オブジェクトに変換する。
    /// Get Project Index
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
    /// </summary>
    /// <param name="json">API から取得した JSON データ。</param>
    /// <returns>パースされた <see cref="ApiProjectIndexModel"/> オブジェクト。</returns>
    ApiProjectIndexModel ParseProjectIndexJson(string json);

    /// <summary>
    /// 指定されたプロジェクトのインサイトウィジェットのURLを取得する。
    /// Get Project Insights Widget
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Insights-Widget
    /// </summary>
    /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
    /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
    /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
    /// <param name="widgetType">(オプション) 取得するウィジェットの種類（People, Sentiments, Keywords, Search）。</param>
    /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
    /// <returns>
    /// インサイトウィジェットのURLを表す文字列。
    /// response.Headers.Locationか、response.Contentか不明、要精査 todo
    /// </returns>
    Task<string> GetProjectInsightsWidgetAsync(string location, string accountId, string projectId, string? widgetType = null, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのプレイヤーウィジェットのURLを取得する。
    /// Get Project Player Widget
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Player-Widget
    /// </summary>
    /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
    /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
    /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
    /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
    /// <returns>
    /// プレイヤーウィジェットのURLを表す文字列。
    /// response.Headers.Locationか、response.Contentか不明、要精査 todo
    /// </returns>
    Task<string> GetProjectPlayerWidgetAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのレンダー操作のステータスを取得する。
    /// Get Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">対象のアカウントID (GUID)</param>
    /// <param name="projectId">対象のプロジェクトID</param>
    /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
    /// <returns>レンダー操作の状態を含むモデル</returns>
    Task<ApiProjectRenderOperationModel> GetProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのレンダー操作のJSONデータを取得する。
    /// Get Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">対象のアカウントID (GUID)</param>
    /// <param name="projectId">対象のプロジェクトID</param>
    /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
    /// <returns>APIから取得したJSON文字列 (レンダー操作のステータスを含む)</returns>
    /// <exception cref="HttpRequestException">HTTPリクエストエラーが発生した場合</exception>
    Task<string> FetchProjectRenderOperationJsonAsync(string location, string accountId, string projectId, string? accessToken);

    /// <summary>
    /// JSONデータを ApiProjectRenderOperationModel にパースする。
    /// Get Project Render Operation
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
    /// </summary>
    /// <param name="json">JSON 文字列</param>
    /// <returns>パースされた ApiProjectRenderOperationModel オブジェクト</returns>
    ApiProjectRenderOperationModel ParseProjectRenderOperationJson(string json);

    /// <summary>
    /// 指定されたプロジェクトのサムネイル画像データ（バイナリストリーム）を取得します。
    /// Get Project Thumbnail Bits
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Thumbnail
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">対象のアカウントID (GUID)</param>
    /// <param name="projectId">対象のプロジェクトID</param>
    /// <param name="thumbnailId">取得するサムネイルのID (GUID)</param>
    /// <param name="format">オプション: サムネイルのフォーマット (Jpeg / Base64)</param>
    /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
    /// <returns>サムネイル画像のバイナリデータ（ストリーム）</returns>
    Task<Stream> GetProjectThumbnailBitsAsync(string location, string accountId, string projectId, string thumbnailId, string? format = null, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのサムネイルのURLを取得する。
    /// Get Project Thumbnail
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Thumbnail
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">対象のアカウントID (GUID)</param>
    /// <param name="projectId">対象のプロジェクトID</param>
    /// <param name="thumbnailId">取得するサムネイルのID (GUID)</param>
    /// <param name="format">オプション: サムネイルのフォーマット (Jpeg / Base64)</param>
    /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
    /// <returns>サムネイルのURL</returns>
    Task<string> GetProjectThumbnailUrlAsync(string location, string accountId, string projectId, string thumbnailId, string? format = null, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトのサムネイルのURLを取得する。
    /// Get Project Thumbnail
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Thumbnail
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">対象のアカウントID (GUID)</param>
    /// <param name="projectId">対象のプロジェクトID</param>
    /// <param name="thumbnailId">取得するサムネイルのID (GUID)</param>
    /// <param name="format">オプション: サムネイルのフォーマット (Jpeg / Base64)</param>
    /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
    /// <returns>サムネイルのURLを表す文字列</returns>
    /// <exception cref="HttpRequestException">HTTPリクエストエラーが発生した場合</exception>
    Task<string> FetchProjectThumbnailUrlAsync(string location, string accountId, string projectId, string thumbnailId, string? format, string? accessToken);

    /// <summary>
    /// 指定されたアカウントのプロジェクト一覧を取得する。
    /// List Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects
    /// </summary>
    /// <param name="location">Azureのリージョン。</param>
    /// <param name="accountId">アカウントのID（GUID形式）。</param>
    /// <param name="createdAfter">指定された日付以降に作成されたプロジェクトをフィルタリング。</param>
    /// <param name="createdBefore">指定された日付以前に作成されたプロジェクトをフィルタリング。</param>
    /// <param name="pageSize">取得するページサイズ。</param>
    /// <param name="skip">スキップするレコード数。</param>
    /// <param name="accessToken">認証用のアクセストークン。</param>
    /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
    Task<ApiProjectSearchResultModel> GetProjectsAsync(string location, string accountId, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string? accessToken = null);

    /// <summary>
    /// 指定されたアカウントのプロジェクト一覧をJSON形式で取得する。
    /// List Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects        /// </summary>
    /// <param name="location">Azureのリージョン。</param>
    /// <param name="accountId">アカウントのID（GUID形式）。</param>
    /// <param name="createdAfter">指定された日付以降に作成されたプロジェクトをフィルタリング。</param>
    /// <param name="createdBefore">指定された日付以前に作成されたプロジェクトをフィルタリング。</param>
    /// <param name="pageSize">取得するページサイズ。</param>
    /// <param name="skip">スキップするレコード数。</param>
    /// <param name="accessToken">認証用のアクセストークン。</param>
    /// <returns>APIレスポンスのJSON文字列。</returns>
    Task<string> FetchProjectsJsonAsync(string location, string accountId, string? createdAfter, string? createdBefore, int? pageSize, int? skip, string? accessToken);

    /// <summary>
    /// JSONレスポンスを `ApiProjectSearchResultModel` に変換する。
    /// List Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects  /// </summary>
    /// <param name="jsonResponse">APIから取得したJSONレスポンス。</param>
    /// <returns>マッピングされた `ApiProjectSearchResultModel` オブジェクト。</returns>
    ApiProjectSearchResultModel MapToApiProjectSearchResultModel(string jsonResponse);

    /// <summary>
    /// プロジェクトのレンダリングを開始し、結果を取得する非同期メソッド
    /// Render Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントID (GUID)</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">オプションのアクセストークン (Bearer トークンとして利用可能)</param>
    /// <param name="sendCompletionEmail">レンダリング完了時にメール通知を送信するかのフラグ</param>
    /// <returns>レンダリング結果のオブジェクトを返す</returns>
    Task<ApiProjectRenderResponseModel> RenderProjectAsync(string location, string accountId, string projectId, string? accessToken = null, bool sendCompletionEmail = false);

    /// <summary>
    /// 指定されたパラメータを基にURLを構築し、APIにリクエストを送信し、JSONレスポンスを取得する非同期メソッド
    /// Render Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントID (GUID)</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">オプションのアクセストークン (Bearer トークンとして利用可能)</param>
    /// <param name="sendCompletionEmail">レンダリング完了時にメール通知を送信するかのフラグ</param>
    /// <returns>APIから取得したJSONレスポンス</returns>
    Task<string> FetchProjectRenderJsonAsync(string location, string accountId, string projectId, string? accessToken, bool sendCompletionEmail);

    /// <summary>
    /// JSONレスポンスを解析し、レンダリング結果のオブジェクトに変換するメソッド
    /// Render Project
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
    /// </summar
    /// <param name="jsonResponse">APIから取得したJSONレスポンス</param>
    /// <returns>パース済みのレンダリング結果オブジェクト</returns>
    ApiProjectRenderResponseModel ParseProjectRenderJson(string jsonResponse);

    /// <summary>
    /// 指定された検索条件でプロジェクトを検索する非同期メソッド
    /// Search Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
    /// </summary>
    /// <param name="location">Azureリージョン</param>
    /// <param name="accountId">アカウントのGUID</param>
    /// <param name="query">(オプション) フリーテキスト検索クエリ</param>
    /// <param name="sourceLanguage">(オプション) ソース言語</param>
    /// <param name="pageSize">(オプション) 取得するプロジェクトの最大件数</param>
    /// <param name="skip">(オプション) スキップするプロジェクトの件数 (ページネーション用)</param>
    /// <param name="accessToken">(オプション) アクセストークン</param>
    /// <returns>検索結果のレスポンスモデル</returns>
    Task<ApiProjectSearchResultModel> SearchProjectsAsync(string location, string accountId, string? query = null, string? sourceLanguage = null, int? pageSize = null, int? skip = null, string? accessToken = null);

    /// <summary>
    /// 指定された検索条件でプロジェクトを検索する非同期メソッド
    /// Search Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
    /// </summary>
    /// <param name="location">Azureリージョン</param>
    /// <param name="accountId">アカウントのGUID</param>
    /// <param name="query">(オプション) フリーテキスト検索クエリ</param>
    /// <param name="sourceLanguage">(オプション) ソース言語</param>
    /// <param name="pageSize">(オプション) 取得するプロジェクトの最大件数</param>
    /// <param name="skip">(オプション) スキップするプロジェクトの件数 (ページネーション用)</param>
    /// <param name="accessToken">(オプション) アクセストークン</param>
    /// <returns>検索結果のレスポンスモデル</returns>
    Task<string> FetchProjectSearchJsonAsync(string location, string accountId, string? query, string? sourceLanguage, int? pageSize, int? skip, string? accessToken);

    /// <summary>
    /// APIリクエストを送信し、JSONレスポンスを取得する共通メソッド
    /// Search Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
    /// </summary>
    /// <param name="method">HTTPメソッド (GET, POSTなど)</param>
    /// <param name="url">リクエストを送信するURL</param>
    /// <returns>APIからのJSONレスポンス</returns>
    Task<string> SendApiRequestAsync(HttpMethod method, string url);

    /// <summary>
    /// APIリクエストを送信し、JSONレスポンスを取得する共通メソッド。
    /// </summary>
    /// <param name="method">HTTPメソッド (GET, POST, PUT など)</param>
    /// <param name="url">APIのリクエストURL</param>
    /// <param name="content">(オプション) HTTPリクエストのボディコンテンツ</param>
    /// <returns>APIからのJSONレスポンス</returns>
    Task<string> SendApiRequestAsync(HttpMethod method, string url, HttpContent? content = null);

    /// <summary>
    /// JSONレスポンスを指定した型にパースするメソッド
    /// Search Projects
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
    /// </summary>
    /// <typeparam name="T">パースするオブジェクトの型</typeparam>
    /// <param name="jsonResponse">APIからのJSONレスポンス</param>
    /// <returns>パースされたオブジェクト</returns>
    ApiProjectSearchResultModel ParseApiProjectSearchResultModelJson(string jsonResponse);

    /// <summary>
    /// 指定されたプロジェクトの情報を更新する非同期メソッド。
    /// </summary>
    /// <param name="location">Azureリージョン</param>
    /// <param name="accountId">アカウントのGUID</param>
    /// <param name="projectId">更新するプロジェクトのID</param>
    /// <param name="updateRequest">更新するプロジェクトのデータ (名前とビデオ範囲)</param>
    /// <param name="accessToken">(オプション) アクセストークン</param>
    /// <returns>更新されたプロジェクトのレスポンスモデル</returns>
    Task<ApiProjectUpdateResponseModel> UpdateProjectAsync(string location, string accountId, string projectId, ApiProjectUpdateRequestModel updateRequest, string? accessToken = null);

    /// <summary>
    /// 指定されたプロジェクトの更新リクエストをAPIに送信し、レスポンスを取得する。
    /// </summary>
    /// <param name="location">Azureリージョンの識別子</param>
    /// <param name="accountId">更新対象のアカウントID</param>
    /// <param name="projectId">更新するプロジェクトのID</param>
    /// <param name="updateRequest">プロジェクトの更新内容</param>
    /// <param name="accessToken">(オプション) アクセストークン</param>
    /// <returns>APIのJSONレスポンス</returns>        
    Task<string> FetchProjectUpdateJsonAsync(string location, string accountId, string projectId, ApiProjectUpdateRequestModel updateRequest, string? accessToken);

    /// <summary>
    /// JSONレスポンスを指定した型にパースするメソッド。
    /// </summary>
    /// <param name="jsonResponse">APIからのJSONレスポンス</param>
    /// <returns>パースされたオブジェクト</returns>
    ApiProjectUpdateResponseModel ParseProjectUpdateResponseJson(string jsonResponse);
}