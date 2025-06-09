using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    /// <summary>
    /// ビデオインデックスのリポジトリクラス
    /// </summary>
    public class VideoIndexRepository : IVideoIndexRepository
    {
        private readonly ILogger<VideoListRepository> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IVideoIndexApiAccess _videoIndexApiAccess;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IVideoItemDataMapper _videoItemDataMapper;
        private readonly IAccountRepository _accountRepository;

        private const string ParamName = "account";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="authenticationTokenizer">認証トークンプロバイダー</param>
        /// <param name="accountAccess">アカウントアクセスプロバイダー</param>
        /// <param name="videoIndexApiAccess">ビデオアイテムAPIアクセスプロバイダー</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        /// <param name="videoItemDataMapper">ビデオアイテムデータマッパー</param>
        /// <param name="accountRepository">アカウントリポジトリ</param>
        public VideoIndexRepository(ILogger<VideoListRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IVideoIndexApiAccess videoIndexApiAccess, IApiResourceConfigurations apiResourceConfigurations, IVideoItemDataMapper videoItemDataMapper, IAccountRepository accountRepository)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _videoIndexApiAccess = videoIndexApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _videoItemDataMapper = videoItemDataMapper;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// ビデオアイテムのJSONを非同期で取得する
        /// </summary>
        /// <param name="videoId">ビデオID</param>
        /// <returns>ビデオアイテムのJSONレスポンス</returns>
        public async Task<string> GetVideoItemJsonAsync(string videoId)
        {
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            _accountRepository.CheckAccount(account);
            string? location = account.location;
            string? accountId = account.properties?.id;
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            return await _videoIndexApiAccess.GetVideoItemJsonAsync(location!, accountId!, videoId, accessToken);
        }

        /// <summary>
        /// ビデオアイテムを非同期で取得し、パースして返す
        /// </summary>
        /// <param name="videoId">ビデオID</param>
        /// <returns>ビデオアイテムデータモデル</returns>
        public async Task<VideoItemDataModel> GetVideoItemAsync(string videoId)
        {
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            _accountRepository.CheckAccount(account);
            string? location = account.location;
            string? accountId = account.properties?.id;
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            var dataModel = await _videoIndexApiAccess.GetVideoItemAsync(location!, accountId!, videoId, accessToken);
            return _videoItemDataMapper.MapFrom(dataModel);
        }
    }
}
