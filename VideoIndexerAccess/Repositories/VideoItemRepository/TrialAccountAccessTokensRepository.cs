using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class TrialAccountAccessTokensRepository : ITrialAccountAccessTokensRepository
    {
        // ロガーインスタンス
        private readonly ILogger<TrialAccountAccessTokensRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API設定
        private readonly ITrialAccountAccessTokensApiAccess _accountAccessTokensApiAccess;

        private const string ParamName = "accountAccessToken";

        // マッパーインターフェース
        private readonly IAccountSlimMapper _accountSlimMapper;


        public TrialAccountAccessTokensRepository(ILogger<TrialAccountAccessTokensRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, ITrialAccountAccessTokensApiAccess accountAccessTokensApiAccess, IAccountSlimMapper accountSlimMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountAccessTokensApiAccess = accountAccessTokensApiAccess;
            _accountSlimMapper = accountSlimMapper;
        }

        /// <summary>
        /// アカウントアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenAsync(bool? allowEdit = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            return await GetAccountAccessTokenAsync(location!, accountId!, allowEdit);
        }

        /// <summary>
        /// 指定されたロケーションとアカウントIDに基づいてアクセストークンを取得します。
        /// </summary>
        /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
        /// <param name="accountId">アカウントID（GUID形式）。</param>
        /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null)
        {
            // アカウントアクセストークンを取得
            return await _accountAccessTokensApiAccess.GetAccountAccessTokenAsync(location, accountId, allowEdit, clientRequestId);
        }

        /// <summary>
        /// アカウントアクセストークンを指定されたパーミッションで取得する非同期メソッド。
        /// </summary>
        /// <param name="permission">取得するパーミッション（例: "Reader", "Contributor"）。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenWithPermissionAsync(string? permission = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アカウントアクセストークンを取得
            return await GetAccountAccessTokenWithPermissionAsync(location!, accountId!, permission);
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、およびパーミッションに基づいてアクセストークンを取得します。
        /// </summary>
        /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
        /// <param name="accountId">アカウントID（GUID形式）。</param>
        /// <param name="permission">取得するパーミッション（例: "Reader", "Contributor"）。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenWithPermissionAsync(string location, string accountId, string? permission = null, string? clientRequestId = null)
        {
            // アカウントアクセストークンを取得
            return await _accountAccessTokensApiAccess.GetAccountAccessTokenWithPermissionAsync(location, accountId, permission, clientRequestId);
        }

        /// <summary>
        /// アカウント情報のリストを取得する非同期メソッド。
        /// </summary>
        /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
        /// <returns>アカウント情報のリスト。エラーが発生した場合は null。</returns>
        public async Task<List<AccountSlimModel>?> GetAccountsAsync(bool generateAccessTokens = true)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;

            return await GetAccountsAsync(location!, generateAccessTokens);
        }

        /// <summary>
        /// 指定されたロケーションに基づいてアカウント情報のリストを取得します。
        /// </summary>
        /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
        /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を与えるかどうか。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>アカウント情報のリスト。エラーが発生した場合は null。</returns>
        public async Task<List<AccountSlimModel>?> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? clientRequestId = null)
        {
            var results = await _accountAccessTokensApiAccess.GetAccountsAsync(location, generateAccessTokens, allowEdit, clientRequestId);
            return results?.Select(_accountSlimMapper.MapFrom).ToList();
        }

        /// <summary>
        /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="projectId">プロジェクト ID。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetProjectAccessTokenAsync(string projectId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // プロジェクトアクセストークンを取得
            return await GetProjectAccessTokenAsync(location!, accountId!, projectId);
        }

        /// <summary>
        /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
        /// <param name="accountId">アカウント ID（GUID形式）。</param>
        /// <param name="projectId">プロジェクト ID。</param>
        /// <param name="allowEdit">編集を許可するかどうか（true で書き込み可）。省略可。</param>
        /// <param name="clientRequestId">リクエストトラッキング用の GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetProjectAccessTokenAsync(string location, string accountId, string projectId, bool? allowEdit = null, string? clientRequestId = null)
        {
            // アカウントアクセストークンを取得
            return await _accountAccessTokensApiAccess.GetProjectAccessTokenAsync(location, accountId, projectId, allowEdit, clientRequestId);
        }

        /// <summary>
        /// ユーザーに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
        public async Task<string?> GetUserAccessTokenAsync()
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;

            // ユーザーアクセストークンを取得
            return await GetUserAccessTokenAsync(location!, allowEdit: null, clientRequestId: null);
        }

        /// <summary>
        /// ユーザーに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="allowEdit">編集を許可するかどうか（true または false）。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用の GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
        public async Task<string?> GetUserAccessTokenAsync(string location, bool? allowEdit = null, string? clientRequestId = null)
        {
            return await _accountAccessTokensApiAccess.GetUserAccessTokenAsync(location, allowEdit, clientRequestId);
        }
        
        /// <summary>
        /// ビデオに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="videoId">ビデオ ID。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetVideoAccessTokenAsync(string videoId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // ビデオアクセストークンを取得
            return await GetVideoAccessTokenAsync(location!, accountId!, videoId, allowEdit: null, clientRequestId: null);
        }

        /// <summary>
        /// ビデオに対するアクセストークンを取得する非同期メソッド。
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="accountId">アカウント ID。</param>
        /// <param name="videoId">ビデオ ID。</param>
        /// <param name="allowEdit">編集を許可するかどうか（true または false）。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用の GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetVideoAccessTokenAsync(string location, string accountId, string videoId, bool? allowEdit = null, string? clientRequestId = null)
        {
            return await _accountAccessTokensApiAccess.GetVideoAccessTokenAsync(location, accountId, videoId, allowEdit, clientRequestId);
        }

    }
}
