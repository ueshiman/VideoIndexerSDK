using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class CreateLogoRepository
    {
        // ロガーインスタンス
        private readonly ILogger<CreateLogoRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository; private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly ILogoResponseMapper _logoResponseMapper;
        private readonly ILogoRequestMapper _logoRequestMapper;
        // 例外スロー時のパラメータ名
        private const string ParamName = "createLogo";

        public CreateLogoRepository(ILogger<CreateLogoRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ILogoResponseMapper logoResponseMapper, ILogoRequestMapper logoRequestMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _logoResponseMapper = logoResponseMapper;
            _logoRequestMapper = logoRequestMapper;
        }

        public async Task<LogoResponseModel> CreateCustomLogoAsync(LogoRequestModel request)
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

            return await CreateCustomLogoAsync(location!, accountId!, request, accessToken);
        }

        public async Task<LogoResponseModel> CreateCustomLogoAsync(string location, string accountId, LogoRequestModel request, string? accessToken = null)
        {
            try
            {
                // リクエストモデルをAPI用モデルにマッピング
                var apiRequest = _logoRequestMapper.MapToApiLogoRequestModel(request);

                // APIへロゴ作成リクエスト送信
                var apiResponse = await _customLogosApiAccess.CreateCustomLogoAsync(location, accountId, apiRequest, accessToken);

                // レスポンスをドメインモデルにマッピング
                var result = _logoResponseMapper.Map(apiResponse);

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, model={Name}", location, accountId, request.Name);
                throw; // 元の例外をそのまま再スロー
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, model={Name}", location, accountId, request.Name);
                throw; // 元の例外をそのまま再スロー
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating custom logol: location={Location}, accountId={AccountId}, model={Name}", location, accountId, request.Name);
                throw; // 元の例外をそのまま再スロー
            }
        }
    }
}
