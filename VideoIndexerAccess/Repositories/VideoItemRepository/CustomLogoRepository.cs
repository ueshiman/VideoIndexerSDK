using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class CustomLogoRepository : ICustomLogoRepository
    {
        // ロガーインスタンス
        private readonly ILogger<CustomLogoRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly ILogoResponseMapper _logoResponseMapper;
        private readonly ILogoRequestMapper _logoRequestMapper;

        private readonly ILogoGroupRequestMapper _logoGroupRequestMapper;
        private readonly ILogoGroupContractMapper _logoGroupContractMapper;

        private readonly ILogoContractMapper _logoContractMapper;
        private readonly ILogoGroupLinkedLogosMapper _logoGroupLinkedLogosMapper;
        private readonly ILogoUpdateRequestMapper _logoUpdateRequestMapper;
        private readonly ILogoGroupUpdateRequestMapper _logoGroupUpdateRequestMapper;

        // 例外スロー時のパラメータ名
        private const string ParamName = "createLogo";

        public CustomLogoRepository(ILogger<CustomLogoRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ILogoResponseMapper logoResponseMapper, ILogoRequestMapper logoRequestMapper, ILogoGroupRequestMapper logoGroupRequestMapper, ILogoGroupContractMapper logoGroupContractMapper, ILogoContractMapper logoContractMapper, ILogoGroupLinkedLogosMapper logoGroupLinkedLogosMapper, ILogoUpdateRequestMapper logoUpdateRequestMapper, ILogoGroupUpdateRequestMapper logoGroupUpdateRequestMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _logoResponseMapper = logoResponseMapper;
            _logoRequestMapper = logoRequestMapper;
            _logoGroupRequestMapper = logoGroupRequestMapper;
            _logoGroupContractMapper = logoGroupContractMapper;
            _logoContractMapper = logoContractMapper;
            _logoGroupLinkedLogosMapper = logoGroupLinkedLogosMapper;
            _logoUpdateRequestMapper = logoUpdateRequestMapper;
            _logoGroupUpdateRequestMapper = logoGroupUpdateRequestMapper;
        }

        /// <summary>
        /// カスタムロゴを作成する非同期メソッド。
        /// アカウント情報を取得し、APIを呼び出してロゴを作成します。
        /// </summary>
        /// <param name="request">ロゴ作成リクエストモデル</param>
        /// <returns>作成されたロゴのレスポンスモデル</returns>
        public async Task<LogoContractModel> CreateCustomLogoAsync(LogoRequestModel request)
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
        public async Task<LogoContractModel> CreateCustomLogoAsync(string location, string accountId, LogoRequestModel request, string? accessToken = null)
        {
            try
            {
                // リクエストモデルをAPI用モデルにマッピング
                var apiRequest = _logoRequestMapper.MapToApiLogoRequestModel(request);

                // APIへロゴ作成リクエスト送信
                var apiResponse = await _customLogosApiAccess.CreateCustomLogoAsync(location, accountId, apiRequest, accessToken);

                // レスポンスをドメインモデルにマッピング
                var result = _logoResponseMapper.MapFrom(apiResponse);

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

        /// <summary>
        /// カスタムロゴグループを作成する非同期メソッド。
        /// アカウント情報を取得し、APIを呼び出してロゴグループを作成します。
        /// </summary>
        /// <param name="request">ロゴグループ作成リクエストモデル</param>
        /// <returns>作成されたロゴグループのレスポンスモデル</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        public async Task<LogoGroupContractModel> CreateLogoGroupAsync(LogoGroupRequestModel request)
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

        /// <summary>
        /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでロゴグループを作成します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ロゴグループ作成リクエストモデル</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>作成されたロゴグループのレスポンスモデル</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<LogoGroupContractModel> CreateLogoGroupAsync(string location, string accountId, LogoGroupRequestModel request, string? accessToken = null)
        {
            try
            {
                // リクエストモデルをAPI用モデルにマッピング
                ApiLogoGroupRequestModel apiRequest = _logoGroupRequestMapper.MapToApiLogoGroupRequestModel(request);
                // API へロゴグループ作成リクエスト送信
                ApiLogoGroupContractModel apiResponse = await _customLogosApiAccess.CreateLogoGroupAsync(location, accountId, apiRequest, accessToken);

                // レスポンスをドメインモデルにマッピング
                var result = _logoGroupContractMapper.MapFrom(apiResponse);

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.Name);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.Name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating logo group: location={Location}, accountId={AccountId}, groupName={Name}", location, accountId, request.Name);
                throw;
            }
        }

        /// <summary>
        /// 指定したロゴIDのロゴを削除します。
        /// アカウント情報を取得し、APIを呼び出してロゴを削除します。
        /// </summary>
        /// <param name="logoId">削除するロゴのID</param>
        /// <returns>非同期タスク</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task DeleteLogoAsync(string logoId)
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
            await DeleteLogoAsync(location!, accountId!, logoId, accessToken);
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・ロゴID・アクセストークンでロゴを削除します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">削除するロゴのID</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>非同期タスク</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task DeleteLogoAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                // API へロゴ削除リクエスト送信
                await _customLogosApiAccess.DeleteLogoAsync(location, accountId, logoId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
        }

        /// <summary>
        /// API にロゴグループを削除するリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">削除するロゴグループのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        public async Task DeleteLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            try
            {
                // API へロゴ削除リクエスト送信
                await _customLogosApiAccess.DeleteLogoGroupAsync(location, accountId, logoGroupId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoGroupId={logoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoGroupId={logoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoGroupId={logoGroupId}", location, accountId, logoGroupId);
                throw;
            }
        }

        /// <summary>
        /// 指定されたロゴIDに関連付けられたロゴを取得します。
        /// </summary>
        /// <remarks>
        /// このメソッドはアカウント情報を取得・検証し、アカウントのリージョンとID、アクセストークンを使用してロゴを取得します。
        /// <paramref name="logoId"/> が有効であること、必要なアカウント設定が正しく構成されていることを確認してください。
        /// </remarks>
        /// <param name="logoId">取得するロゴの一意な識別子。nullや空文字は不可。</param>
        /// <returns>ロゴの詳細を表す <see cref="LogoContractModel"/>。</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合、または必要なアカウント情報がnullの場合にスローされます。</exception>
        /// 指定されたロゴIDに関連付けられたロゴを取得します。
        /// </summary>
        public async Task<LogoContractModel> GetLogoAsync(string logoId)
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
            return await GetLogoAsync(location!, accountId!, logoId, accessToken);
        }


        /// <summary>
        /// ロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴレスポンスモデル</returns>
        public async Task<LogoContractModel> GetLogoAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                // API へロゴ取得リクエスト送信
                ApiLogoContractModel logo = await _customLogosApiAccess.GetLogoAsync(location, accountId, logoId, accessToken);
                return _logoContractMapper.MapFrom(logo);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
        }

        /// <summary>
        /// 指定されたロゴグループIDに関連付けられたロゴグループを取得します。
        /// </summary>
        /// <param name="logoGroupId">取得するロゴグループの一意な識別子。nullや空文字は不可。</param>
        /// <returns>ロゴグループの詳細を表す <see cref="LogoGroupContractModel"/>。</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合、または必要なアカウント情報がnullの場合にスローされます。</exception>
        public async Task<LogoGroupContractModel> GetLogoGroupAsync(string logoGroupId)
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
            return await GetLogoGroupAsync(location!, accountId!, logoGroupId, accessToken);
        }


        /// <summary>
        /// ロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴレスポンスモデル</returns>
        public async Task<LogoGroupContractModel> GetLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            try
            {
                // API へロゴグループ取得リクエスト送信
                ApiLogoGroupContractModel logoGroup = await _customLogosApiAccess.GetLogoGroupAsync(location, accountId, logoGroupId, accessToken);
                return _logoGroupContractMapper.MapFrom(logoGroup);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
        }

        /// <summary>
        /// <summary>
        /// 指定したロゴグループの関連ロゴを非同期で取得します。
        /// </summary>
        /// <remarks>
        /// このメソッドはアカウント情報を取得・検証し、アカウントのリージョンとID、アクセストークンを使用して指定したロゴグループの関連ロゴを取得します。
        /// </remarks>
        /// <param name="logoGroupId">関連ロゴを取得するロゴグループの一意な識別子。</param>
        /// <returns>指定したロゴグループに関連付けられたロゴ情報を含む <see cref="LogoGroupLinkedLogosModel"/>。</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合にスローされます。</exception>
        public async Task<LogoGroupLinkedLogosModel> GetLogoGroupLinkedLogosAsync(string logoGroupId)
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
            return await GetLogoGroupLinkedLogosAsync(location!, accountId!, logoGroupId, accessToken);
        }

        /// <summary>
        /// ロゴグループリンクロゴを取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">取得するロゴグループのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴグループに関連するロゴグループリンクロゴ</returns>
        public async Task<LogoGroupLinkedLogosModel> GetLogoGroupLinkedLogosAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            try
            {
                // API ロゴグループリンクロゴ取得リクエスト送信
                ApiLogoGroupLinkedLogosModel logoGroup = await _customLogosApiAccess.GetLogoGroupLinkedLogosAsync(location, accountId, logoGroupId, accessToken);
                return _logoGroupLinkedLogosMapper.MapFrom(logoGroup);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
        }


        /// <summary>
        /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
        /// <returns>解析済みのロゴグループ情報のリスト</returns>
        /// </summary>
        public async Task<LogoGroupContractModel[]> GetLogoGroupsAsync()
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
            return await GetLogoGroupsAsync(location!, accountId!, accessToken);
        }

        /// <summary>
        /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴグループ情報のリスト</returns>
        /// </summary>
        public async Task<LogoGroupContractModel[]> GetLogoGroupsAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                // API へロゴグループ取得リクエスト送信
                ApiLogoGroupContractModel[] logoGroups = await _customLogosApiAccess.GetLogoGroupsAsync(location, accountId, accessToken);
                return logoGroups.Select(logoGroup => _logoGroupContractMapper.MapFrom(logoGroup)).ToArray();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}", location, accountId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}", location, accountId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}", location, accountId);
                throw;
            }
        }

        /// <summary>
        /// 指定したロゴIDに関連するロゴグループ情報を取得します。
        /// アカウント情報を取得し、APIを呼び出してロゴIDに関連するロゴグループ情報を取得します。
        /// </summary>
        /// <param name="logoId">関連グループを取得するロゴのID</param>
        /// <returns>ロゴIDに関連するロゴグループ情報の配列</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        public async Task<LogoGroupLinkedLogosModel[]> GetLogoLinkedGroupsAsync(string logoId)
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
            return await GetLogoLinkedGroupsAsync(location!, accountId!, logoId, accessToken);
        }

        /// <summary>
        /// 指定したロゴIDに関連するロゴグループ情報を取得します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">関連グループを取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ロゴIDに関連するロゴグループ情報の配列</returns>
        public async Task<LogoGroupLinkedLogosModel[]> GetLogoLinkedGroupsAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                // API へロゴグループリンクロゴ取得リクエスト送信
                ApiLogoGroupLinkedLogosModel[] logoLinkedGroups = await _customLogosApiAccess.GetLogoLinkedGroupsAsync(location, accountId, logoId, accessToken);
                return logoLinkedGroups.Select(logoGroup => _logoGroupLinkedLogosMapper.MapFrom(logoGroup)).ToArray();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
        }

        /// <summary>
        /// ロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴレスポンスモデル</returns>
        public async Task<LogoContractModel> GetLogosAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                // API へロゴ取得リクエスト送信
                ApiLogoContractModel logo = await _customLogosApiAccess.GetLogoAsync(location, accountId, logoId, accessToken);
                return _logoContractMapper.MapFrom(logo);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
        }

        /// <summary>
        /// ロゴ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="logoId">更新するロゴのID</param>
        /// <param name="updateRequest">更新するロゴ情報</param>
        /// <returns>更新後のロゴ情報</returns>
        public async Task<LogoContractModel> UpdateLogoAsync(string logoId, LogoUpdateRequestModel updateRequest)
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
            return await UpdateLogoAsync(location!, accountId!, logoId, updateRequest, accessToken);
        }


        /// <summary>
        /// ロゴ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">更新するロゴのID</param>
        /// <param name="updateRequest">更新するロゴ情報</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>更新後のロゴ情報</returns>
        public async Task<LogoContractModel> UpdateLogoAsync(string location, string accountId, string logoId, LogoUpdateRequestModel updateRequest, string? accessToken = null)
        {
            try
            {
                // API へロゴグループリンクロゴ取得リクエスト送信
                ApiLogoContractModel updateResponse = await _customLogosApiAccess.UpdateLogoAsync(location, accountId, logoId, _logoUpdateRequestMapper.MapToApiLogoUpdateRequestModel(updateRequest), accessToken);
                return _logoContractMapper.MapFrom(updateResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoId={LogoId}", location, accountId, logoId);
                throw;
            }
        }

        /// <summary>
        /// API にロゴグループ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="logoGroupId">更新するロゴグループのID</param>
        /// <param name="updateRequest">更新するロゴグループ情報</param>
        /// <returns>更新後のロゴグループ情報</returns>
        public async Task<LogoGroupContractModel> UpdateLogoGroupAsync(string logoGroupId, LogoGroupUpdateRequestModel updateRequest)
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
            return await UpdateLogoGroupAsync(logoGroupId, updateRequest);
        }

        /// <summary>
        /// API にロゴグループ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">更新するロゴグループのID</param>
        /// <param name="updateRequest">更新するロゴグループ情報</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>更新後のロゴグループ情報</returns>
        public async Task<LogoGroupContractModel> UpdateLogoGroupAsync(string location, string accountId, string logoGroupId, LogoGroupUpdateRequestModel updateRequest, string? accessToken = null)
        {
            try
            {
                // API へロゴグループリンクロゴ取得リクエスト送信
                ApiLogoGroupContractModel updateResponse = await _customLogosApiAccess.UpdateLogoGroupAsync(location, accountId, logoGroupId, _logoGroupUpdateRequestMapper.MapToApiApiLogoGroupUpdateRequestModel(updateRequest), accessToken);
                return _logoGroupContractMapper.MapFrom(updateResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting logo: location={Location}, accountId={AccountId}, logoGroupId={LogoGroupId}", location, accountId, logoGroupId);
                throw;
            }
        }
    }
}
