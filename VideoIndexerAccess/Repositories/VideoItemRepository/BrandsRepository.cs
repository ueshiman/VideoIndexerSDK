using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<bool> CreateApiBrandModelAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var response = await _brandslApiAccess.CreateApiBrandModelAsync(location, accountId, accessToken, new ApiBrandModel());
                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var brandModel = _brandslApiAccess.ParseApiBrandModel(jsonResponse);
                    return true;
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
