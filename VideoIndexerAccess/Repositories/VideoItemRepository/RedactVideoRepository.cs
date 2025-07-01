using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class RedactVideoRepository : IRedactVideoRepository
    {
        // ロガーインスタンス
        private readonly ILogger<ProjectsRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API設定
        private readonly IRedactionApiAccess _redactionApiAccess;

        // プロンプトコンテンツのレスポンスをマッピングするインターフェース
        private readonly IRedactVideoRequestMapper _redactVideoRequestMapper;

        // パラメータ名
        private const string ParamName = "redactVideo";


        public RedactVideoRepository(ILogger<ProjectsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IRedactionApiAccess redactionApiAccess, IRedactVideoRequestMapper redactVideoRequestMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _redactionApiAccess = redactionApiAccess;
            _redactVideoRequestMapper = redactVideoRequestMapper;
        }

        /// <summary>
        /// ビデオの編集 (Redact) を非同期で実行します。
        /// アカウント情報を取得し、アクセストークンを生成し、指定されたビデオを編集します。
        /// </summary>
        /// <param name="videoId">編集対象のビデオ ID</param>
        /// <param name="request">ビデオ編集リクエストモデル</param>
        /// <returns>編集リクエストの成功状態を示すブール値</returns>
        public async Task<bool> RedactVideoAsync(string videoId, RedactVideoRequestModel request)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // ビデオの編集 (Redact) を実行
            return await RedactVideoAsync(location!, accountId!, videoId, request, accessToken);
        }


        /// <summary>
        /// ビデオの編集 (Redact) を非同期で実行します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="videoId">ビデオ ID</param>
        /// <param name="request">ビデオの編集リクエストモデル</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>編集リクエストの成功状態を示すブール値</returns>
        public async Task<bool> RedactVideoAsync(string location, string accountId, string videoId, RedactVideoRequestModel request, string? accessToken = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アクセストークンを取得
            accessToken ??= await _authenticationTokenizer.GetAccessToken();
            // ビデオの編集 (Redact) を実行
            return await _redactionApiAccess.RedactVideoAsync(location, accountId, videoId, _redactVideoRequestMapper.MapToApiRedactVideoRequestModel(request), accessToken);
        }
    }
}
