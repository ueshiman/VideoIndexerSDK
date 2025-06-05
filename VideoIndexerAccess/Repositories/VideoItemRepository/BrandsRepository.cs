using Microsoft.Extensions.Logging;
using System.Net;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class BrandsRepository
    {
        private readonly ILogger<BrandsRepository> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer; 
        private readonly IAccounApitAccess _accountAccess;
        private readonly IAccountRepository _accountRepository;

        private readonly IBrandsApiAccess _brandslApiAccess;

        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private const string ParamName = "account";


        public BrandsRepository(ILogger<BrandsRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IBrandsApiAccess brandslApiAccess, IApiResourceConfigurations apiResourceConfigurations, IAccounApitAccess accountAccess, IAccountRepository accountRepository)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _brandslApiAccess = brandslApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// 指定されたロケーションとアカウントIDを使用して、新しいブランドモデルを作成します。
        /// </summary>
        /// <param name="location">APIのリージョンを指定します。</param>
        /// <param name="accountId">アカウントIDを指定します。</param>
        /// <param name="accessToken">アクセストークン（省略可能）。指定しない場合はデフォルトのトークンが使用されます。</param>
        /// <returns>ブランドモデルの作成に成功した場合はtrue、失敗した場合はfalseを返します。</returns>
        public async Task<bool> CreateApiBrandModelAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var response = await _brandslApiAccess.CreateApiBrandModelAsync(location, accountId, accessToken, new ApiBrandModel());
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var brandModel = _brandslApiAccess.ParseApiBrandModel(jsonResponse);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            _logger.LogDebug("Brand created successfully: {BrandModel}", brandModel);
                            return true;
                        default:
                            _logger.LogWarning("Brand created with status code: {StatusCode}", response.StatusCode);
                            return true;
                    }
                }
                else
                {
                    _logger.LogError("Failed to create brand: {StatusCode}", response?.StatusCode);
                }
                return false;
            }
            catch (Exception exception)
            {
                // ログの出力
                _logger.LogError(exception, "Error occurred while creating brand");
                return false;
            }
        }

        /// <summary>
        /// アカウント情報を取得し、存在しない場合は例外をスローします。
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// BrandsRepository クラスは、Video Indexer API を使用してブランドモデルを管理するためのリポジトリです。
        /// </summary>
        /// <remarks>
        /// 主な機能:
        /// - ブランドモデルの作成
        /// - アカウント情報の取得と検証
        /// - APIとの通信を通じたブランドデータの操作
        /// 
        /// 使用する依存コンポーネント:
        /// - ILogger<BrandsRepository>: ログ出力用
        /// - IAuthenticationTokenizer: アクセストークンの取得
        /// - IAccounApitAccess: アカウント情報の取得
        /// - IAccountRepository: アカウント情報の検証
        /// - IBrandsApiAccess: ブランドモデルに関するAPI操作
        /// - IApiResourceConfigurations: APIリソース設定
        /// 
        /// .NET バージョン: .NET 8
        /// C# バージョン: 12.0
        /// </remarks>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> CreateApiBrandModelAsync()
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            return await CreateApiBrandModelAsync(location!, accountId!, accessToken);
        }


    }
}
