using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    /// <summary>
    /// ビデオデータのリポジトリクラス
    /// </summary>
    public class VideoDataRepository : IVideoDataRepository
    {
        private readonly ILogger<VideoDataRepository> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly IVideoDownloadApiAccess _videoDownload;
        private readonly IAccountRepository _accountRepository;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="authenticationTokenizer">認証トークンプロバイダー</param>
        /// <param name="accountAccess">アカウントアクセスプロバイダー</param>
        /// <param name="apiResourceConfigurations">APIリソース設定</param>
        /// <param name="videoDownload">ビデオダウンロードAPIアクセスプロバイダー</param>
        /// <param name="accountRepository">アカウントリポジトリ</param>
        public VideoDataRepository(
            ILogger<VideoDataRepository> logger,
            IAuthenticationTokenizer authenticationTokenizer,
            IAccounApitAccess accountAccess,
            IApiResourceConfigurations apiResourceConfigurations,
            IVideoDownloadApiAccess videoDownload,
            IAccountRepository accountRepository)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _videoDownload = videoDownload;
            _accountRepository = accountRepository;
        }

        /// <summary>
        /// ビデオのダウンロードURLを取得する
        /// </summary>
        /// <param name="videoId">ビデオID</param>
        /// <returns>ビデオのダウンロードURL</returns>
        public async Task<string> GetVideoDownloadUrl(string videoId)
        {
            _logger.LogInformation("GetVideoDownloadUrl started for videoId: {VideoId}", videoId);

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();
            _logger.LogInformation("Access token retrieved successfully.");

            string? accountId = _apiResourceConfigurations.ViAccountName;

            // アカウント情報を取得
            ApiAccountModel? account = await _accountAccess.GetAccountAsync(accountId);
            _accountRepository.CheckAccount(account);

            _logger.LogInformation("Account retrieved successfully for accountId: {AccountId}", accountId);

            // ビデオのダウンロードURLを取得
            string downloadUrlResponse = await _videoDownload.GetVideoDownloadUrl(account!.Location!, account!.Properties!.Id!, videoId, accessToken);
            _logger.LogInformation("Download URL retrieved successfully for videoId: {VideoId}", videoId);

            return downloadUrlResponse;
        }
    }
}