using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface ITrialAccountsRepository
{
    /// <summary>
    /// 指定された条件に基づいてトライアルアカウント情報を非同期で取得します。
    /// </summary>
    /// <param name="includeUsage">使用状況情報を含めるかどうかを指定します。</param>
    /// <param name="includeStatistics">統計情報を含めるかどうかを指定します。</param>
    /// <returns>トライアルアカウント情報の配列。</returns>
    Task<TrialAccountModel[]> GetAccountAsync(bool? includeUsage = null, bool? includeStatistics = null);

    /// <summary>
    /// 指定されたロケーションと条件に基づいてトライアルアカウント情報を非同期で取得します。
    /// </summary>
    /// <param name="location">APIの呼び出し先リージョン。</param>
    /// <param name="includeUsage">使用状況情報を含めるかどうかを指定します。</param>
    /// <param name="includeStatistics">統計情報を含めるかどうかを指定します。</param>
    /// <param name="accessToken">アクセストークン（省略可能）。</param>
    /// <returns>トライアルアカウント情報の配列。</returns>
    Task<TrialAccountModel[]> GetAccountAsync(string location, string accountId, bool? includeUsage = null, bool? includeStatistics = null, string? accessToken = null);

    Task<TrialAccountWithTokenModel[]> GetAccountsAsync(bool? generateAccessTokens = null);

    /// <summary>
    /// 指定されたロケーションと条件に基づいてトライアルアカウント情報を非同期で取得します。
    /// </summary>
    /// <param name="location">APIの呼び出し先リージョン。</param>
    /// <param name="generateAccessTokens">各アカウントに対してアクセストークンを生成するかどうか。</param>
    /// <param name="allowEdit">アクセストークンに書き込み権限（Contributor）を含めるかどうか。</param>
    /// <param name="accessToken">アクセストークン（省略可能）。</param>
    /// <returns>トライアルアカウント情報の配列。</returns>
    Task<TrialAccountWithTokenModel[]> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? accessToken = null);
}