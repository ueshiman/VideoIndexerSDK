using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class TrialAccountAccessTokensRepository
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

        public async Task<List<ApiAccountSlimModel>?> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? clientRequestId = null)
        {

        }

    }
}
