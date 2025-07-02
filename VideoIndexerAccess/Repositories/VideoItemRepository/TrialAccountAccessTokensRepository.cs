using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
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



        public async Task<string?> GetAccountAccessTokenAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントアクセストークンを取得
            return await _accountAccessTokensApiAccess.GetAccountAccessTokenAsync(location, accountId, allowEdit, clientRequestId);
        }
    }
}
