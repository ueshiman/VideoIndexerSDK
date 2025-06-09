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
        private readonly ILogoGroupResponseMapper _logoGroupResponseMapper;
        // 例外スロー時のパラメータ名
        private const string ParamName = "createLogo";

        public CreateLogoRepository(ILogger<CreateLogoRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ILogoResponseMapper logoResponseMapper, ILogoRequestMapper logoRequestMapper, ILogoGroupResponseMapper logoGroupResponseMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _logoResponseMapper = logoResponseMapper;
            _logoRequestMapper = logoRequestMapper;
            _logoGroupResponseMapper = logoGroupResponseMapper;
        }

        /// <summary>
        /// カスタムロゴを作成する非同期メソッド。
        /// アカウント情報を取得し、APIを呼び出してロゴを作成します。
        /// </summary>
        /// <param name="request">ロゴ作成リクエストモデル</param>
        /// <returns>作成されたロゴのレスポンスモデル</returns>
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

        /// <summary>
        /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでカスタムロゴを作成します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ロゴ作成リクエストモデル</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>作成されたロゴのレスポンスモデル</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
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
                _logger.LogError(ex, "Unexpected error occurred while creating custom logo: location={Location}, accountId={AccountId}, model={Name}", location, accountId, request.Name);
                throw; // 元の例外をそのまま再スロー
            }
        }

        public async Task<LogoGroupResponseModel> CreateLogoGroupAsync(ApiLogoGroupRequestModel request)
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

            return await CreateLogoGroupAsync(location!, accountId!, request, accessToken);
        }

        public async Task<LogoGroupResponseModel> CreateLogoGroupAsync(string location, string accountId, ApiLogoGroupRequestModel request, string? accessToken = null)
        {
            try
            {
                // API へロゴグループ作成リクエスト送信
                var apiResponse = await _customLogosApiAccess.CreateLogoGroupAsync(location, accountId, request, accessToken);

                // レスポンスをドメインモデルにマッピング
                var result = _logoGroupResponseMapper.MapFrom(apiResponse);

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.name);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating logo group: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.name);
                throw;
            }
        }
    }
}
