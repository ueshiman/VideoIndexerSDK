using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ICustomLogosApiAccess
{
    /// <summary>
    /// API にカスタムロゴを作成するリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ロゴ作成リクエストデータ</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>作成されたロゴのレスポンス情報</returns>
    Task<ApiLogoContractModel> CreateCustomLogoAsync(string location, string accountId, ApiLogoRequestModel request, string? accessToken = null);

    /// <summary>
    /// API にリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ロゴ作成リクエストデータ</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>APIからのレスポンス文字列</returns>
    Task<string> SendApiRequestAsync(string location, string accountId, ApiLogoRequestModel request, string? accessToken);

    /// <summary>
    /// API レスポンスの JSON を解析する
    /// </summary>
    /// <param name="jsonResponse">APIからのレスポンス文字列</param>
    /// <returns>解析されたレスポンスモデル</returns>
    ApiLogoContractModel ParseApiResponse(string jsonResponse);

    /// <summary>
    /// API にロゴグループを作成するリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ロゴグループ作成リクエストデータ</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>作成されたロゴグループのレスポンス情報</returns>
    Task<ApiLogoGroupResponseModel> CreateLogoGroupAsync(string location, string accountId, ApiLogoGroupRequestModel request, string? accessToken = null);

    /// <summary>
    /// POST リクエストを送信する
    /// </summary>
    Task<string> SendPostRequestAsync(string location, string accountId, ApiLogoGroupRequestModel request, string? accessToken);

    /// <summary>
    /// JSON を解析しロゴグループレスポンスモデルを生成する
    /// </summary>
    ApiLogoGroupResponseModel ParseLogoGroupJson(string jsonResponse);

    /// <summary>
    /// API にロゴを削除するリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">削除するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    Task DeleteLogoAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// API にロゴグループを削除するリクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">削除するロゴグループのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    Task DeleteLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// API からロゴ情報の JSON を取得する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したロゴ情報の JSON 文字列</returns>
    Task<string> GetLogoJsonAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// JSON を解析しロゴレスポンスモデルを生成する
    /// </summary>
    /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
    /// <returns>解析したロゴレスポンスモデル</returns>
    ApiLogoContractResponseModel ParseLogoJson(string jsonResponse);

    /// <summary>
    /// ロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴレスポンスモデル</returns>
    Task<ApiLogoContractResponseModel> GetLogoAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// API からロゴグループに関連するすべてのロゴ情報の JSON を取得する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">取得するロゴグループのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したロゴグループに関連するロゴ情報の JSON 文字列</returns>
    Task<string> GetLogoGroupLinkedLogosJsonAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// JSON を解析しロゴグループに関連するロゴ情報のリストを生成する
    /// </summary>
    /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
    /// <returns>解析したロゴリスト</returns>
    ApiLogoContractResponseModel[] ParseLogoGroupLinkedLogosJson(string jsonResponse);

    /// <summary>
    /// ロゴグループに関連するロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">取得するロゴグループのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴグループに関連するロゴリスト</returns>
    Task<ApiLogoContractResponseModel[]> GetLogoGroupLinkedLogosAsync(string location, string accountId, string logoGroupId, string? accessToken = null);

    /// <summary>
    /// API からすべてのロゴグループ情報の JSON を取得する
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ロゴグループ情報の JSON 文字列</returns>
    /// </summary>
    Task<string> GetLogoGroupsJsonAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// JSON を解析しロゴグループ情報のリストを生成する
    /// <param name="jsonResponse">API から取得したロゴグループ情報の JSON</param>
    /// <returns>解析したロゴグループのリスト</returns>
    /// </summary>
    ApiLogoGroupContractModel[] ParseLogoGroupsJson(string jsonResponse);

    /// <summary>
    /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴグループ情報のリスト</returns>
    /// </summary>
    Task<ApiLogoGroupContractModel[]> GetLogoGroupsAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// API から特定のロゴが関連するロゴグループ情報の JSON を取得する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">取得するロゴのID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ロゴグループ情報の JSON 文字列</returns>
    Task<string> GetLogoLinkedGroupsJsonAsync(string location, string accountId, string logoId, string? accessToken = null);

    /// <summary>
    /// JSON を解析しロゴが関連するロゴグループ情報のリストを生成する
    /// </summary>
    /// <param name="jsonResponse">API から取得したロゴグループ情報の JSON</param>
    /// <returns>解析したロゴグループのリスト</returns>
    ApiLogoGroupContractModel[] ParseLogoLinkedGroupsJson(string jsonResponse);

    /// <summary>
    /// API からすべてのロゴ情報の JSON を取得する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ロゴ情報の JSON 文字列</returns>
    Task<string> GetLogosJsonAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// JSON を解析しロゴ情報のリストを生成する
    /// </summary>
    /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
    /// <returns>解析したロゴのリスト</returns>
    ApiLogoContractResponseModel[] ParseLogosJson(string jsonResponse);

    /// <summary>
    /// すべてのロゴ情報を取得し、オブジェクトとして返す
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>解析済みのロゴ情報のリスト</returns>
    Task<ApiLogoContractResponseModel[]> GetLogosAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// API にロゴ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoId">更新するロゴのID</param>
    /// <param name="updateRequest">更新するロゴ情報</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>更新後のロゴ情報</returns>
    Task<ApiLogoContractResponseModel> UpdateLogoAsync(string location, string accountId, string logoId, ApiLogoUpdateRequestModel updateRequest, string? accessToken = null);

    /// <summary>
    /// API にロゴグループ情報の更新リクエストを送信する
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="logoGroupId">更新するロゴグループのID</param>
    /// <param name="updateRequest">更新するロゴグループ情報</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>更新後のロゴグループ情報</returns>
    Task<ApiLogoGroupContractModel> UpdateLogoGroupAsync(string location, string accountId, string logoGroupId, ApiLogoGroupUpdateRequestModel updateRequest, string? accessToken = null);
}