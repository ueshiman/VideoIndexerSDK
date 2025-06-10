using Azure.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class IndexingRepository
    {
        // ロガーインスタンス
        private readonly ILogger<CreateLogoRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly IIndexingApiAccess _indexingApiAccess;
        private const string ParamName = "indexing";

        public IndexingRepository(ILogger<CreateLogoRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, IIndexingApiAccess indexingApiAccess)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _indexingApiAccess = indexingApiAccess;
        }

        /// <summary>
        /// 指定したリクエストモデルに基づき、動画から顔情報を削除します。
        /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
        /// </summary>
        /// <param name="request">動画IDおよび顔IDを含む削除リクエストモデル</param>
        /// <returns>削除が成功した場合は true、それ以外は false</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        public async Task<bool> DeleteVideoFaceAsync(DeleteVideoFaceRequestModel request)
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

            return await DeleteVideoFaceAsync(location!, accountId!, request, accessToken);
        }

        /// <summary>
        /// 指定したロケーション・アカウントID・リクエスト・アクセストークンで動画から顔情報を削除します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">動画IDおよび顔IDを含む削除リクエストモデル</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>削除が成功した場合は true、それ以外は false</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<bool> DeleteVideoFaceAsync(string location, string accountId, DeleteVideoFaceRequestModel request, string? accessToken = null)
        {

            try
            {
                return await _indexingApiAccess.DeleteVideoFaceAsync(location, accountId, request.VideoId, request.FaceId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, VideoId={VideoId}, FaceId ={FaceId}", location, accountId, request.VideoId, request.FaceId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, VideoId={VideoId}, FaceId ={FaceId}", location, accountId, request.VideoId, request.FaceId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating logo group: location={Location}, accountId={AccountId}, VideoId={VideoId}, FaceId ={FaceId}", location, accountId, request.VideoId, request.FaceId);
                throw;
            }
        }
    }
}
