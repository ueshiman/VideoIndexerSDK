using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface ITrialAccountAccessTokensRepository
{
    /// <summary>
    /// アカウントアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenAsync(bool? allowEdit = null);

    /// <summary>
    /// 指定されたロケーションとアカウントIDに基づいてアクセストークンを取得します。
    /// </summary>
    /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
    /// <param name="accountId">アカウントID（GUID形式）。</param>
    /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// アカウントアクセストークンを指定されたパーミッションで取得する非同期メソッド。
    /// </summary>
    /// <param name="permission">取得するパーミッション（例: "Reader", "Contributor"）。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenWithPermissionAsync(string? permission = null);

    /// <summary>
    /// 指定されたロケーション、アカウントID、およびパーミッションに基づいてアクセストークンを取得します。
    /// </summary>
    /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
    /// <param name="accountId">アカウントID（GUID形式）。</param>
    /// <param name="permission">取得するパーミッション（例: "Reader", "Contributor"）。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
    Task<string?> GetAccountAccessTokenWithPermissionAsync(string location, string accountId, string? permission = null, string? clientRequestId = null);

    /// <summary>
    /// アカウント情報のリストを取得する非同期メソッド。
    /// </summary>
    /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
    /// <returns>アカウント情報のリスト。エラーが発生した場合は null。</returns>
    Task<List<AccountSlimModel>?> GetAccountsAsync(bool generateAccessTokens = true);

    /// <summary>
    /// 指定されたロケーションに基づいてアカウント情報のリストを取得します。
    /// </summary>
    /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
    /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
    /// <param name="allowEdit">アクセストークンに編集権限を与えるかどうか。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
    /// <returns>アカウント情報のリスト。エラーが発生した場合は null。</returns>
    Task<List<AccountSlimModel>?> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="projectId">プロジェクト ID。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetProjectAccessTokenAsync(string projectId);

    /// <summary>
    /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
    /// <param name="accountId">アカウント ID（GUID形式）。</param>
    /// <param name="projectId">プロジェクト ID。</param>
    /// <param name="allowEdit">編集を許可するかどうか（true で書き込み可）。省略可。</param>
    /// <param name="clientRequestId">リクエストトラッキング用の GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetProjectAccessTokenAsync(string location, string accountId, string projectId, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// ユーザーに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
    Task<string?> GetUserAccessTokenAsync();

    /// <summary>
    /// ユーザーに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="allowEdit">編集を許可するかどうか（true または false）。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用の GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
    Task<string?> GetUserAccessTokenAsync(string location, bool? allowEdit = null, string? clientRequestId = null);

    /// <summary>
    /// ビデオに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="videoId">ビデオ ID。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetVideoAccessTokenAsync(string videoId);

    /// <summary>
    /// ビデオに対するアクセストークンを取得する非同期メソッド。
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="accountId">アカウント ID。</param>
    /// <param name="videoId">ビデオ ID。</param>
    /// <param name="allowEdit">編集を許可するかどうか（true または false）。省略可。</param>
    /// <param name="clientRequestId">リクエスト識別用の GUID（省略可）。</param>
    /// <returns>アクセストークンの文字列。失敗時は null。</returns>
    Task<string?> GetVideoAccessTokenAsync(string location, string accountId, string videoId, bool? allowEdit = null, string? clientRequestId = null);
}