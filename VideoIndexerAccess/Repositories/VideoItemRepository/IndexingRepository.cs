using Azure.Core;
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

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly IIndexingApiAccess _indexingApiAccess;
        private readonly IVideoIndexApiAccess _videoIndexApiAccess;
        public readonly IVideoItemDataMapper _videoItemDataMapper;

        private const string ParamName = "indexing";

        public IndexingRepository(ILogger<CreateLogoRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, IIndexingApiAccess indexingApiAccess, IVideoIndexApiAccess videoIndexApiAccess, IVideoItemDataMapper videoItemDataMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _indexingApiAccess = indexingApiAccess;
            _videoIndexApiAccess = videoIndexApiAccess;
            _videoItemDataMapper = videoItemDataMapper;
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

        /// <summary>
        /// 指定されたリクエストモデルに基づいて、ビデオインデックスを取得します。
        /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
        /// </summary>
        /// <param name="request">ビデオIDおよびオプションのパラメータを含むリクエストモデル</param>
        /// <returns>ビデオインデックスデータモデル</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<VideoItemDataModel> GetVideoIndexAsync(VideoIndexRequestModel request)
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
            
            return await GetVideoIndexAsync(location!, accountId!, request, accessToken);

        }


        /// <summary>
        /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでビデオインデックスを取得します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ビデオIDおよびオプションのパラメータを含むリクエストモデル</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>ビデオインデックスデータモデル</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<VideoItemDataModel> GetVideoIndexAsync(string location, string accountId, VideoIndexRequestModel request, string? accessToken = null)
        {
            try
            {
                // アクセストークンが指定されていない場合は取得
                if (string.IsNullOrEmpty(accessToken))
                {
                    accessToken = await _authenticationTokenizer.GetAccessToken();
                }

                // APIを呼び出してビデオインデックスを取得
                VideoItemApiModel videoApiIndex = await _videoIndexApiAccess.GetVideoIndexAsync(location, accountId, request.VideoId, accessToken, request.Language, request.ReTranslate, request.IncludeStreamingUrls, request.IncludedInsights, request.ExcludedInsights);

                return _videoItemDataMapper.MapFrom(videoApiIndex);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching video index: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
        }

        /// <summary>
        /// 指定された動画の再インデックスを実行します。
        /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
        /// </summary>
        /// <param name="request">再インデックスリクエストモデル</param>
        /// <returns>再インデックスが成功した場合は true、それ以外は false</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<bool> ReIndexVideoAsync(ReIndexVideoRequestModel request)
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

            return await ReIndexVideoAsync(location!, accountId!, request, accessToken);
        }


        /// <summary>
        /// 指定された動画の再インデックスを実行します。
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="request">再インデックスリクエストモデル</param>
        /// <param name="accessToken">API アクセストークン（省略可）</param>
        /// <returns>再インデックスが成功した場合は true、それ以外は false</returns>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<bool> ReIndexVideoAsync(string location, string accountId, ReIndexVideoRequestModel request, string? accessToken = null)
        {
            try
            {
                // 処理の開始をログに記録
                _logger.LogInformation("ReIndexVideoAsync started: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);

                // 再インデックス処理を実行
                var result = await _indexingApiAccess.ReIndexVideoAsync(location, accountId, request.VideoId, accessToken, request.ExcludedAI, request.IsSearchable, request.IndexingPreset, request.StreamingPreset, request.CallbackUrl, request.SourceLanguage, request.SendSuccessEmail, request.LinguisticModelId, request.PersonModelId, request.Priority, request.BrandsCategories, request.CustomLanguages, request.LogoGroupId, request.PunctuationMode);

                // 処理結果に応じてログを記録
                if (result)
                {
                    _logger.LogInformation("ReIndexVideoAsync succeeded: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                }
                else
                {
                    _logger.LogWarning("ReIndexVideoAsync failed: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                }

                // 処理結果を返却
                return result;
            }
            catch (ArgumentException ex)
            {
                // 引数エラーをログに記録し、例外を再スロー
                _logger.LogError(ex, "Argument error in ReIndexVideoAsync: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                // APIリクエスト失敗をログに記録し、例外を再スロー
                _logger.LogError(ex, "API request failed in ReIndexVideoAsync: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
            catch (Exception ex)
            {
                // その他の予期しないエラーをログに記録し、例外を再スロー
                _logger.LogError(ex, "Unexpected error occurred in ReIndexVideoAsync: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, request.VideoId);
                throw;
            }
        }
        

    }
}
