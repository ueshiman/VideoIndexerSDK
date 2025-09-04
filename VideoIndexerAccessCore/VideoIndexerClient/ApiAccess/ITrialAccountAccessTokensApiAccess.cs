using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ITrialAccountAccessTokensApiAccess
{
    /// <summary>
    /// アカウントアクセストークンを取得する非同期メソッド。
    /// Get ApiTrialAccountModel Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
    /// </summary>
    /// <param name="location">Azureリージョンを示す文字列（例: "japaneast"）。</param>
    /// <param name="accountId">アカウントID（GUID形式）。</param>
    /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// 指定されたパラメータに基づいて Video Indexer API からアクセストークンを取得する非同期メソッド。
    /// Get ApiTrialAccountModel Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="accountId">アカウント ID。</param>
    /// <param name="allowEdit">編集許可の有無（true または false）。省略可。</param>
    /// <param name="clientRequestId">クライアントからのリクエスト ID。省略可。</param>
    /// <returns>JSON形式のアクセストークンレスポンス文字列。</returns>
    Task<string> FetchAccessTokenJsonAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// アクセストークンの JSON 文字列をパースしてトークン文字列を返す。
    /// Get ApiTrialAccountModel Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
    /// </summary>
    /// <param name="json">APIから取得した JSON 文字列。</param>
    /// <returns>アクセストークンの文字列。null または不正な形式の場合は例外をスロー。</returns>
    string ParseAccessTokenJson(string json);

    /// <summary>
    /// アカウントアクセストークン（パーミッション指定付き）を取得する非同期メソッド。
    /// Get ApiTrialAccountModel Access Token With Permission (PREVIEW)
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token-With-Permission
    /// </summary>
    /// <param name="location">Azureリージョン。</param>
    /// <param name="accountId">アカウントID（GUID形式）。</param>
    /// <param name="permission">取得するパーミッション（Reader / Contributor / Owner など）。省略可。</param>
    /// <param name="clientRequestId">クライアントリクエストID。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenWithPermissionAsync(string location, string accountId, string? permission = null, string? clientRequestId = null);

    /// <summary>
    /// 指定されたパーミッションに基づいて Video Indexer API からアクセストークンを取得する非同期メソッド。
    /// Get ApiTrialAccountModel Access Token With Permission (PREVIEW)
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token-With-Permission
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="accountId">アカウント ID。</param>
    /// <param name="permission">リクエストするパーミッション。省略可。</param>
    /// <param name="clientRequestId">クライアントからのリクエスト ID。省略可。</param>
    /// <returns>JSON形式のアクセストークンレスポンス文字列。</returns>
    Task<string> FetchAccessTokenWithPermissionJsonAsync(string location, string accountId, string? permission = null, string? clientRequestId = null);

    /// <summary>
    /// アカウントの一覧を取得する非同期メソッド（オプションでアクセストークン付き）。
    /// Get Accounts Authorization
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
    /// </summary>
    /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
    /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
    /// <param name="allowEdit">アクセストークンに編集権限を与えるか。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>ApiAccountSlimModel オブジェクトのリスト。失敗時は null。</returns>
    Task<List<ApiAccountSlimModel>?> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// アカウント一覧の JSON をパースしてオブジェクトに変換するメソッド。
    /// Get Accounts Authorization
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
    /// </summary>
    /// <param name="json">APIから取得したアカウント一覧のJSON文字列。</param>
    /// <returns>ApiAccountSlimModel オブジェクトのリスト。</returns>
    List<ApiAccountSlimModel>? ParseAccountsJson(string json);

    /// <summary>
    /// アカウントの一覧を取得する非同期メソッド（オプションでアクセストークン付き）。
    /// Get Accounts Authorization
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
    /// </summary>
    /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
    /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
    /// <param name="allowEdit">アクセストークンに編集権限を与えるか。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>ApiAccountSlimModel オブジェクトのリスト。失敗時は null。</returns>
    Task<string> FetchAccountsJsonAsync(string location, bool? generateAccessTokens, bool? allowEdit, string? clientRequestId);

    /// <summary>
    /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
    /// Get Project Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
    /// </summary>
    /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
    /// <param name="accountId">アカウント ID（GUID形式）。</param>
    /// <param name="projectId">プロジェクト ID。</param>
    /// <param name="allowEdit">編集を許可するかどうか（true で書き込み可）。省略可。</param>
    /// <param name="clientRequestId">リクエストトラッキング用の GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetProjectAccessTokenAsync(string location, string accountId, string projectId, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// プロジェクトアクセストークン取得用の JSON を Video Indexer API から取得する非同期メソッド。
    /// Get Project Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
    /// </summary>
    /// <param name="location">API 呼び出し対象の Azure リージョン（例: "japaneast"）。</param>
    /// <param name="accountId">対象のアカウント ID（GUID形式）。</param>
    /// <param name="projectId">対象のプロジェクト ID。</param>
    /// <param name="allowEdit">アクセストークンに編集権限を含めるか（true: 編集可、false: 読み取り専用）。省略可。</param>
    /// <param name="clientRequestId">リクエストの識別に使用される GUID（任意）。</param>
    /// <returns>API 応答の JSON 文字列。</returns>
    Task<string> FetchProjectAccessTokenJsonAsync(string location, string accountId, string projectId, bool? allowEdit, string? clientRequestId);

    /// <summary>
    /// JSON 文字列から単一の文字列値をデシリアライズする汎用メソッド。
    /// Get Project Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
    /// </summary>
    /// <param name="json">文字列を含む JSON データ。</param>
    /// <returns>デシリアライズされた文字列。null や不正な形式の場合は例外をスロー。</returns>
    string ParseProjectAccessTokenJson(string json);

    /// <summary>
    /// ユーザーに対するアクセストークンを取得する非同期メソッド。
    /// Get User Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
    /// </summary>
    /// <param name="location">API 呼び出し対象の Azure リージョン（例: "japaneast"）。</param>
    /// <param name="allowEdit">アクセストークンに編集権限を付与するか（true: 編集可, false: 読み取り専用）。省略可。</param>
    /// <param name="clientRequestId">リクエストを識別する GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
    Task<string?> GetUserAccessTokenAsync(string location, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// ユーザーアクセストークンを取得する API を呼び出して JSON 文字列を取得する。
    /// Get User Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="allowEdit">編集を許可するか（true または false）。省略可。</param>
    /// <param name="clientRequestId">任意のリクエスト ID。</param>
    /// <returns>API 応答の JSON 文字列。</returns>
    Task<string> FetchUserAccessTokenJsonAsync(string location, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// ユーザーアクセストークンの JSON を解析してトークン文字列を抽出する。
    /// Get User Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
    /// </summary>
    /// <param name="json">JSON 文字列。</param>
    /// <returns>アクセストークンの文字列。</returns>
    string ParseUserAccessTokenJson(string json);

    /// <summary>
    /// JSON 文字列から単一の文字列値をデシリアライズする汎用メソッド。
    /// Get User Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
    /// </summary>
    /// <param name="json">文字列を含む JSON データ。</param>
    /// <returns>デシリアライズされた文字列。null や不正な形式の場合は例外をスロー。</returns>
    string ParseStringJson(string json);

    /// <summary>
    /// ビデオに対するアクセストークンを取得する非同期メソッド。
    /// Get Video Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
    /// </summary>
    /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
    /// <param name="accountId">対象のアカウント ID（GUID形式）。</param>
    /// <param name="videoId">対象のビデオ ID。</param>
    /// <param name="accessToken"></param>
    /// <param name="allowEdit">アクセストークンに編集権限を含めるか（true: 編集可、false: 読み取り専用）。省略可。</param>
    /// <param name="clientRequestId">リクエストを識別するための GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetVideoAccessTokenAsync(string location, string accountId, string videoId, string? accessToken, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// Video Indexer API からビデオアクセストークンの JSON データを取得する非同期メソッド。
    /// Get Video Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
    /// </summary>
    /// <param name="location">API 呼び出し対象の Azure リージョン。</param>
    /// <param name="accountId">アカウント ID（GUID形式）。</param>
    /// <param name="videoId">ビデオ ID。</param>
    /// <param name="accessToken"></param>
    /// <param name="allowEdit">編集権限を付与するか（true または false）。省略可。</param>
    /// <param name="clientRequestId">リクエストトラッキング用の GUID（任意）。</param>
    /// <returns>API 応答の JSON 文字列。</returns>
    Task<string> FetchVideoAccessTokenJsonAsync(string location, string accountId, string videoId, string? accessToken = null, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// ビデオアクセストークンの JSON を解析してトークン文字列を抽出する。
    /// Get Video Access Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
    /// </summary>
    /// <param name="json">JSON 文字列。</param>
    /// <returns>アクセストークンの文字列。</returns>
    string ParseVideoAccessTokenJson(string json);
}