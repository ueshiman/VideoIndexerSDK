using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

/// <summary>
/// アカウント情報へのアクセスを提供するインターフェース
/// </summary>
public interface IAccounApitAccess
{
    /// <summary>
    /// HttpClientの名前
    /// </summary>
    string HttpClientName { get; set; }

    /// <summary>
    /// Video indexer APIでアカウント情報の取得
    /// 事前取得をして設定ファイルで持つのではなくAPIで取得する
    /// </summary>
    /// <param name="accountName">アカウント名</param>
    /// <returns>アカウント情報</returns>
    ApiAccountModel? GetAccount(string? accountName = null);

    /// <summary>
    /// Video indexer APIでアカウント情報の取得を非同期で行う
    /// 事前取得をして設定ファイルで持つのではなくAPIで取得する
    /// </summary>
    /// <param name="accountName">アカウント名</param>
    /// <returns>アカウント情報</returns>
    Task<ApiAccountModel?> GetAccountAsync(string? accountName = null);
}
