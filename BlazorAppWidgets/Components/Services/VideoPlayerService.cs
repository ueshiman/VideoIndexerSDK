using Microsoft.Identity.Client;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.VideoItemRepository;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace BlazorAppWidgets.Components.Services
{
    public class VideoPlayerService
    {
        private const string ParamName = "videoPlayerService";

        public  VideoPlayerService(ILogger<VideoPlayerService> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IVideoListRepository videoListRepository, ITrialAccountAccessTokensRepository trialAccountAccessTokensRepository)
        {
            _apiResourceConfigurations = apiResourceConfigurations;
            _videoListRepository = videoListRepository;
            _trialAccountAccessTokensRepository = trialAccountAccessTokensRepository;
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;

            // アカウント情報を取得し、存在しない場合は例外をスロー
            ApiAccountModel? account = _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName).Result ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            Region = account.location ?? string.Empty;
            AccountId = account.properties?.id ?? string.Empty;

            // アクセストークンを取得
            //AccessToken =  _authenticationTokenizer.GetAccessToken().Result;
        }

        public  string Region { get; private set; }
        public  string AccountId { get; private set; }
        public string  AccessToken => _authenticationTokenizer.GetAccessToken().GetAwaiter().GetResult();

        public async Task<string?> GetAccessToken(string videoId)
        {
            // アクセストークンを取得
            return await _authenticationTokenizer.GetAccessToken();

            //return await _accountAccess.G(videoId, allowEdit: null, clientRequestId: null);
            //return await _trialAccountAccessTokensRepository.GetVideoAccessTokenAsync(videoId, allowEdit: null, clientRequestId: null);
        }
        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // ロガーインスタンス
        private readonly ILogger<VideoPlayerService> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        private readonly IVideoListRepository _videoListRepository;

        private readonly ITrialAccountAccessTokensRepository _trialAccountAccessTokensRepository;

        public async Task<string[]> GetVideoListAsync()
        {
            var list = await _videoListRepository.ListVideosAsync();
            var array = list.Where(item => item.Id is not null).Select(item => item.Id ?? string.Empty);
            return array.ToArray();
        }
    }
}
