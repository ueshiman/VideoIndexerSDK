using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class VideosRepository
    {
        // ロガーインスタンス
        private readonly ILogger<VideosRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API設定
        private readonly IVideosApiAccess _videosApiAccess;
        private readonly IVideoDownloadApiAccess _videoDownloadApiAccess;

        private const string ParamName = "videos";

        // マッパーインターフェース
        private readonly IDeleteVideoResultMapper _deleteVideoResultMapper;

        public VideosRepository(ILogger<VideosRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IVideosApiAccess videosApiAccess, IVideoDownloadApiAccess videoDownloadApiAccess, IDeleteVideoResultMapper deleteVideoResultMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _videosApiAccess = videosApiAccess;
            _videoDownloadApiAccess = videoDownloadApiAccess;
            _deleteVideoResultMapper = deleteVideoResultMapper;
        }

        /// <summary>
        /// 指定されたビデオを削除します。
        /// </summary>
        /// <param name="videoId">削除対象のビデオID</param>
        /// <returns>削除結果を示す <see cref="DeleteVideoResultModel"/> オブジェクト</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合にスローされます。</exception>
        public async Task<DeleteVideoResultModel?> DeleteVideoAsync(string videoId)
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

            // ビデオを削除する
            return await DeleteVideoAsync(location!, accountId!, videoId, accessToken);
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、ビデオIDを使用してビデオを削除します。
        /// </summary>
        /// <param name="location">Azureリージョン名</param>
        /// <param name="accountId">Video IndexerアカウントID</param>
        /// <param name="videoId">削除対象のビデオID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>削除結果を示す <see cref="DeleteVideoResultModel"/> オブジェクト</returns>
        public async Task<DeleteVideoResultModel?> DeleteVideoAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            ApiDeleteVideoResultModel? resultModel = await _videosApiAccess.DeleteVideoAsync(location, accountId, videoId, accessToken);
            return resultModel is null ? null : _deleteVideoResultMapper.MapFrom(resultModel);
        }
    }
}
