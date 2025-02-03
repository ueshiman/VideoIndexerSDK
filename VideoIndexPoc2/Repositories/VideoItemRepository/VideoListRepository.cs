using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoIndexPoc2.Repositories.AuthorizAccesss;
using Microsoft.Identity.Client;
using VideoInexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoInexerAccessCore.VideoIndexerClient.Model;

namespace VideoIndexPoc2.Repositories.VideoItemRepository
{
    /// <summary>
    /// ビデオリストのリポジトリクラス
    /// </summary>
    public class VideoListRepository : IVideoListRepository
    {
        private readonly ILogger<VideoListRepository> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IVideoListApiAccess _videoListApiAccess;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガー</param>
        /// <param name="authenticationTokenizer">認証トークンプロバイダー</param>
        /// <param name="accountAccess">アカウントアクセスプロバイダー</param>
        /// <param name="videoListApiAccess">ビデオリストAPIアクセスプロバイダー</param>
        public VideoListRepository(ILogger<VideoListRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IVideoListApiAccess videoListApiAccess)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _videoListApiAccess = videoListApiAccess;
        }

        /// <summary>
        /// ビデオリストのJSONを非同期で取得する
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <returns>ビデオリストのJSONレスポンス</returns>
        public async Task<string> GetVideoListJsonAsync(string? location, string? accountId)
        {
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            return await _videoListApiAccess.GetVideoListJsonAsync(location, accountId, accessToken);
        }

        /// <summary>
        /// ビデオリストを非同期で取得し、パースして返す
        /// </summary>
        /// <param name="createdAfter">指定された日付以降に作成されたビデオをフィルタリング</param>
        /// <param name="createdBefore">指定された日付以前に作成されたビデオをフィルタリング</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップするレコード数</param>
        /// <param name="partitions">パーティションでビデオをフィルタリング</param>
        /// <returns>ビデオリスト</returns>
        public async Task<IEnumerable<VideoListItem>> ListVideosAsync(DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null)
        {
            try
            {
                var account = await _accountAccess.GetAccountAsync(); // キャッシュしちゃダメ
                if (account == null)
                {
                    _logger.LogError("Account is not found.");
                    throw new ApplicationException("Account is not found.");
                }
                string location = account.Location;
                string accountId = account.Properties.Id;
                var accessToken = await _authenticationTokenizer.GetAccessToken();
                return await _videoListApiAccess.ListVideosAsync(location, accountId, accessToken: accessToken, createdAfter: createdAfter?.ToString("o"), createdBefore: createdBefore?.ToString("o"), pageSize: pageSize, skip: skip, partitions: partitions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while listing videos.");
                throw;
            }
        }

        /// <summary>
        /// 指定された条件でビデオを検索する
        /// </summary>
        /// <param name="sourceLanguage">ソース言語</param>
        /// <param name="hasSourceVideoFile">ソースビデオファイルの有無</param>
        /// <param name="sourceVideoId">ソースビデオID</param>
        /// <param name="state">ビデオの状態</param>
        /// <param name="privacy">プライバシーレベル</param>
        /// <param name="id">ビデオID</param>
        /// <param name="partition">パーティション</param>
        /// <param name="externalId">外部ID</param>
        /// <param name="owner">所有者</param>
        /// <param name="face">顔</param>
        /// <param name="animatedCharacter">アニメキャラクター</param>
        /// <param name="query">クエリ</param>
        /// <param name="textScope">テキストスコープ</param>
        /// <param name="language">言語</param>
        /// <param name="createdAfter">作成日（以降）</param>
        /// <param name="createdBefore">作成日（以前）</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップ数</param>
        /// <returns>検索結果のビデオリストのJSONレスポンス</returns>
        public async Task<string> SearchVideosAsync(string? sourceLanguage = null, bool? hasSourceVideoFile = null, string? sourceVideoId = null, string[]? state = null, string[]? privacy = null, string[]? id = null, string[]? partition = null, string[]? externalId = null, string[]? owner = null, string[]? face = null, string[]? animatedCharacter = null, string[]? query = null, string[]? textScope = null, string[]? language = null, DateTimeOffset? createdAfter = null, DateTimeOffset? createdBefore = null, int? pageSize = null, int? skip = null)
        {
            try
            {
                var account = await _accountAccess.GetAccountAsync(); // キャッシュしちゃダメ
                if (account == null)
                {
                    _logger.LogError("Account is not found.");
                    throw new ApplicationException("Account is not found.");
                }
                string location = account.Location;
                string accountId = account.Properties.Id;
                var accessToken = await _authenticationTokenizer.GetAccessToken();
                return await _videoListApiAccess.SearchVideosAsync(location, accountId, sourceLanguage, hasSourceVideoFile, sourceVideoId, state, privacy, id, partition, externalId, owner, face, animatedCharacter, query, textScope, language, createdAfter?.ToString("o"), createdBefore?.ToString("o"), pageSize, skip, accessToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while searching videos.");
                throw;
            }
        }
    }
}
