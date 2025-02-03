using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoInexerAccessCore.VideoIndexerClient.Authorization;

namespace VideoIndexPoc2.Repositories.AuthorizAccesss
{
    /// <summary>
    /// 認証トークンを取得するクラス
    /// </summary>
    public class AuthenticationTokenizer : IAuthenticationTokenizer
    {
        private readonly ILogger<Authenticator> _logger;
        private readonly IAuthenticator _authenticator;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="authenticator">認証プロバイダー</param>
        public AuthenticationTokenizer(ILogger<Authenticator> logger, IAuthenticator authenticator)
        {
            _logger = logger;
            _authenticator = authenticator;
        }

        /// <summary>
        /// アクセストークンを非同期で取得する
        /// </summary>
        /// <returns>アクセストークン</returns>
        public async Task<string> GetAccessToken()
        {
            string accessToken = await _authenticator.GetAccessTkenAsync();
            _logger.LogInformation("Get Access Token: {accessToken}", string.IsNullOrEmpty(accessToken));
            return "token";
        }
    }
}
