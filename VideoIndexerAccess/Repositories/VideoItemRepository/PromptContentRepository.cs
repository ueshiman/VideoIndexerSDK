using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class PromptContentRepository
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
        private readonly IPromptContentApiAccess _promptContentApiAccess;

        // プロンプトコンテンツのレスポンスをマッピングするインターフェース
        private readonly IPromptCreateResponseMapper _promptCreateResponseMapper;

        // パラメータ名
        private const string ParamName = "promptContent";


        public PromptContentRepository(ILogger<ProjectsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IPromptContentApiAccess promptContentApiAccess, IPromptCreateResponseMapper promptCreateResponseMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _promptContentApiAccess = promptContentApiAccess;
            _promptCreateResponseMapper = promptCreateResponseMapper;
        }


        /// <summary>
        /// プロンプトコンテンツリポジトリクラス。
        /// Video Indexer API を使用してプロンプトコンテンツを取得するためのメソッドを提供します。
        /// </summary>
        /// <remarks>
        /// このクラスは、アカウント情報の取得、アクセストークンの管理、
        /// および API 呼び出しのロジックをカプセル化します。
        /// </remarks>
        public async Task<PromptCreateResponseModel?> GetPromptContentAsync(PromptContentRequestModel requestModel)
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

            // プロンプトコンテンツを取得
            return await GetPromptContentAsync(location!, accountId!, requestModel, accessToken);
        }


        /// <summary>
        /// 指定されたロケーション、アカウントID、リクエストモデル、およびアクセストークンを使用して
        /// プロンプトコンテンツを取得します。
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">アカウントの一意の識別子</param>
        /// <param name="requestModel">プロンプトコンテンツ取得リクエストモデル</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）</param>
        /// <returns>取得したプロンプトコンテンツのレスポンスモデル、失敗時はnull</returns>
        /// <exception cref="ArgumentException">引数が無効な場合にスローされます</exception>
        /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされます</exception>
        /// <exception cref="Exception">その他のエラーが発生した場合にスローされます</exception>
        public async Task<PromptCreateResponseModel?> GetPromptContentAsync(string location, string accountId, PromptContentRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // アクセストークンが指定されていない場合は取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }
                // アカウント情報を取得
                var account = await _accountAccess.GetAccountAsync(accountId);
                if (account == null)
                {
                    _logger.LogError("Account not found: {AccountId}", accountId);
                    return null;
                }
                // プロンプトコンテンツを取得
                ApiPromptCreateResponseModel? promptContent = await _promptContentApiAccess.GetPromptContentAsync(location, accountId, requestModel.VideoId, requestModel.ModelName, requestModel.PromptStyle, accessToken);

                return promptContent is null ? null : _promptCreateResponseMapper.MapFrom(promptContent);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error occurred. Location: {Location}, AccountId: {AccountId}, video {VideoId} in account {AccountId}", location, accountId, requestModel.VideoId, accountId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed. Location: {Location}, AccountId: {AccountId}, video {VideoId} in account {AccountId}", location, accountId, requestModel.VideoId, accountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get prompt content: {Location}, AccountId: {AccountId}, video {VideoId} in account {AccountId}", location, accountId, requestModel.VideoId, accountId);
                return null;
            }
        }


    }
}
