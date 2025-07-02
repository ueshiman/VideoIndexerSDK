using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ITrialAccountsApiAccess
{
    /// <summary>
    /// Video Indexer API からアカウント情報を取得する非同期メソッド
    /// Get ApiTrialAccountModel
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
    /// </summary>
    /// <param name="location">APIの呼び出し先リージョン</param>
    /// <param name="accountId">アカウントID (GUID)</param>
    /// <param name="includeUsage">使用状況情報を含めるか</param>
    /// <param name="includeStatistics">統計情報を含めるか</param>
    /// <param name="accessToken">オプションのアクセストークン</param>
    /// <returns>ApiTrialAccountModel オブジェクトの配列</returns>
    Task<ApiTrialAccountModel[]> GetAccountsAsync(string location, string accountId, bool? includeUsage = null, bool? includeStatistics = null, string? accessToken = null);

    /// <summary>
    /// API に HTTP GET リクエストを送信し、JSON 形式のアカウント情報を取得する
    /// Get ApiTrialAccountModel
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="includeUsage">使用量情報の有無</param>
    /// <param name="includeStatistics">統計情報の有無</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>レスポンスJSON文字列</returns>
    Task<string> FetchAccountsJsonAsync(string location, string accountId, bool? includeUsage, bool? includeStatistics, string? accessToken);

    /// <summary>
    /// 取得した JSON 文字列を ApiTrialAccountModel オブジェクトの配列に変換する
    /// Get ApiTrialAccountModel
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <returns>Account配列</returns>
    ApiTrialAccountModel[] ParseAccountJson(string json);

    /// <summary>
    /// Video Indexer API からアカウント一覧を取得します。
    /// Get Accounts With Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-With-Token
    /// </summary>
    /// <param name="location">リクエストをルーティングする Azure リージョン。</param>
    /// <param name="generateAccessTokens">各アカウントに対してアクセストークンを生成するかどうか。</param>
    /// <param name="allowEdit">アクセストークンに書き込み権限（Contributor）を含めるかどうか。</param>
    /// <param name="accessToken">（任意）クエリパラメータまたは Authorization ヘッダーで渡すアクセストークン。</param>
    /// <returns>取得したアカウント情報の配列。</returns>
    Task<ApiTrialAccountWithTokenModel[]?> GetAccountsWithTokenAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? accessToken = null);

    /// <summary>
    /// アカウント一覧を取得する API を呼び出し、JSON文字列を取得します。
    /// Get Accounts With Token
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-With-Token
    /// </summary>
    /// <param name="location">Azure リージョン。</param>
    /// <param name="generateAccessTokens">アクセストークンを生成するかどうか。</param>
    /// <param name="allowEdit">編集許可付きトークンを生成するかどうか。</param>
    /// <param name="accessToken">（任意）アクセストークン。</param>
    /// <returns>レスポンスとして返却された JSON 文字列。</returns>
    Task<string> FetchAccountsWithTokenJsonAsync(string location, bool? generateAccessTokens, bool? allowEdit, string accessToken);

    /// <summary>
    /// JSON文字列を ApiTrialAccountWithTokenModel オブジェクトに変換します。
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <returns>ApiTrialAccountWithTokenModel オブジェクト</returns>
    public ApiTrialAccountWithTokenModel[]? ParseAccountWithTokenJson(string json);
}