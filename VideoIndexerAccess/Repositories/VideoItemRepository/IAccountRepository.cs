using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

/// <summary>
/// アカウントリポジトリインターフェース
/// </summary>
public interface IAccountRepository
{
    /// <summary>
    /// アカウント情報をチェックする
    /// </summary>
    /// <param name="account">アカウント情報</param>
    void CheckAccount(ApiAccountModel? account);
}
