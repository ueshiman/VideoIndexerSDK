using System.Threading.Tasks;

namespace VideoIndexPoc2.Repositories.AuthorizAccesss;

/// <summary>
/// 認証トークンを取得するためのインターフェース
/// </summary>
public interface IAuthenticationTokenizer
{
    /// <summary>
    /// アクセストークンを非同期で取得する
    /// </summary>
    /// <returns>アクセストークン</returns>
    Task<string> GetAccessToken();
}
