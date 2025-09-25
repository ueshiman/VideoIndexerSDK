using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class LanguagesRepository
    {
        // ロガーインスタンス
        private readonly ILogger<LanguagesRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private const string ParamName = "account";

        private readonly ILanguagesApiAccess _languagesApiAccess;
        private readonly ISupportedLanguageMapper _supportedLanguageMapper;


        public LanguagesRepository(ILogger<LanguagesRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ILanguagesApiAccess languagesApiAccess, ISupportedLanguageMapper supportedLanguageMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _languagesApiAccess = languagesApiAccess;
            _supportedLanguageMapper = supportedLanguageMapper;
        }

        /// <summary>
        /// サポートされている言語の一覧を取得する
        /// </summary>
        /// <returns>サポートされている言語のリスト</returns>
        public async Task<List<SupportedLanguageModel>> GetSupportedLanguagesAsync()
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            return await GetSupportedLanguagesAsync();
        }

        /// <summary>
        /// サポートされている言語の一覧を取得する
        /// </summary>
        /// <param name="location">API のリクエストを送る Azure のリージョン (例: "trial")</param>
        /// <returns>サポートされている言語のリスト</returns>
        public async Task<List<SupportedLanguageModel>> GetSupportedLanguagesAsync(string location)
        {
            List<ApiSupportedLanguageModel> languageModels = await _languagesApiAccess.GetSupportedLanguagesAsync(location);
            return languageModels.Select(_supportedLanguageMapper.MapFrom).ToList();
        }
    }
}
