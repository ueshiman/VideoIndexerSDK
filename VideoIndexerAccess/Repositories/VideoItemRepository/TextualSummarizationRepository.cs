using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class TextualSummarizationRepository : ITextualSummarizationRepository
    {
        // ロガーインスタンス
        private readonly ILogger<TextualSummarizationRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // API アクセス
        private readonly ITextualSummarizationApiAccess _textualSummarizationApiAccess;

        // スピーチデータセットマッパー
        private readonly ITextualSummarizationJobContractMapper _summarizationJobContractMapper;
        private readonly ITextualSummarizationJobWithSummaryContentContractMapper _summarizationJobWithSummaryContentContractMapper;
        private readonly ITextualSummarizationContractPageMapper _textualSummarizationContractPageMapper;

        private const string ParamName = "textualSummarization";


        public TextualSummarizationRepository(ILogger<TextualSummarizationRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ITextualSummarizationApiAccess textualSummarizationApiAccess, ITextualSummarizationJobContractMapper summarizationJobContractMapper, ITextualSummarizationJobWithSummaryContentContractMapper summarizationJobWithSummaryContentContractMapper, ITextualSummarizationContractPageMapper textualSummarizationContractPageMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _textualSummarizationApiAccess = textualSummarizationApiAccess;
            _summarizationJobContractMapper = summarizationJobContractMapper;
            _summarizationJobWithSummaryContentContractMapper = summarizationJobWithSummaryContentContractMapper;
            _textualSummarizationContractPageMapper = textualSummarizationContractPageMapper;
        }

        /// <summary>
        /// 指定されたビデオのテキスト要約を取得する非同期メソッド。
        /// Create Video Summary1
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <param name="videoId">対象のビデオ ID (GUID 形式)</param>
        /// <param name="length">要約の長さ (Short, Medium, Long のいずれか)</param>
        /// <param name="style">要約のスタイル (Neutral, Casual, Formal のいずれか)</param>
        /// <param name="includedFrames">含めるフレーム (None, Keyframes のいずれか)</param>
        /// <returns>ビデオ要約のレスポンスモデル `ApiVideoSummaryModel` を返す。失敗時は null。</returns>
        public async Task<TextualSummarizationJobContractModel?> GetVideoSummaryAsync(string videoId, string? length = null, string? style = null, string? includedFrames = null)
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

            // API呼び出し
            return await GetVideoSummaryAsync(location!, accountId!, videoId, length, style, includedFrames, accessToken);
        }

        /// <summary>
        /// 指定されたビデオのテキスト要約を取得する非同期メソッド。
        /// Create Video Summary1
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
        /// </summary>
        /// <param name="location">API のリージョン名 (例: "trial")</param>
        /// <param name="accountId">Azure Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="videoId">対象のビデオ ID (GUID 形式)</param>
        /// <param name="length">要約の長さ (Short, Medium, Long のいずれか)</param>
        /// <param name="style">要約のスタイル (Neutral, Casual, Formal のいずれか)</param>
        /// <param name="includedFrames">含めるフレーム (None, Keyframes のいずれか)</param>
        /// <param name="accessToken">API のアクセストークン (オプション、null の場合は未指定)</param>
        /// <returns>ビデオ要約のレスポンスモデル `ApiVideoSummaryModel` を返す。失敗時は null。</returns>
        public async Task<TextualSummarizationJobContractModel?> GetVideoSummaryAsync(string location, string accountId, string videoId, string? length = null, string? style = null, string? includedFrames = null, string? accessToken = null)
        {
            try
            {
                // API呼び出し
                ApiAOAITextualSummarizationJobContractModel? result = await _textualSummarizationApiAccess.GetVideoSummaryAsync(location, accountId, videoId, length, style, includedFrames, accessToken);
                return result is null ? null : _summarizationJobContractMapper.MapFrom(result);
            }
            // 既存のcatchブロックで未定義の変数 'request' を 'videoId' に修正
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, videoId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, videoId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット作成中に予期せぬエラー: location={Location}, accountId={AccountId}, videoId={VideoId}", location, accountId, videoId);
                return null;
            }
        }

        /// <summary>
        /// ビデオのテキスト要約を削除する非同期メソッド。
        /// </summary>
        /// <param name="videoId">対象ビデオの ID</param>
        /// <param name="summaryId">削除する要約の ID</param>
        /// <returns>成功した場合 true、失敗した場合 false</returns>
        public async Task<bool> DeleteVideoSummaryAsync(string videoId, string summaryId)
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

            // API呼び出し
            return await DeleteVideoSummaryAsync(location!, accountId!, videoId, summaryId, accessToken);
        }

        /// <summary>
        /// ビデオのテキスト要約を削除する非同期メソッド。
        /// </summary>
        /// <param name="location">API のリージョン名 (例: "trial")</param>
        /// <param name="accountId">Azure Video Indexer のアカウント ID</param>
        /// <param name="videoId">対象ビデオの ID</param>
        /// <param name="summaryId">削除する要約の ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>成功した場合 true、失敗した場合 false</returns>
        public async Task<bool> DeleteVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        {
            try
            {
                return await _textualSummarizationApiAccess.DeleteVideoSummaryAsync(location, accountId, videoId, summaryId, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "テキスト要約削除中に予期せぬエラー: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return false;
            }
        }

        /// <summary>
        /// Video Indexer API を使用して動画の要約情報を取得します。
        /// Get Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
        /// </summary>
        /// <param name="videoId">対象の動画 ID</param>
        /// <param name="summaryId">取得するサマリー ID（GUID）</param>
        /// <returns>動画要約情報を格納した ApiVideoSummaryResponseModel オブジェクト。失敗時は null。</returns>
        public async Task<TextualSummarizationJobWithSummaryContentContractModel?> GetVideoSummaryAsync(string videoId, string summaryId)
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

            // API呼び出し
            return await GetVideoSummaryAsync(location!, accountId!, videoId, summaryId, accessToken);
        }

        /// <summary>
        /// Video Indexer API を使用して動画の要約情報を取得します。
        /// Get Video Summary
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
        /// </summary>
        /// <param name="location">API リージョン（例: "trial" や "japaneast"）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象の動画 ID</param>
        /// <param name="summaryId">取得するサマリー ID（GUID）</param>
        /// <param name="accessToken">（オプション）アクセストークン。URL クエリに付加されます</param>
        /// <returns>動画要約情報を格納した ApiVideoSummaryResponseModel オブジェクト。失敗時は null。</returns>
        public async Task<TextualSummarizationJobWithSummaryContentContractModel?> GetVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null)
        {
            try
            {
                ApiAOAITextualSummarizationJobWithSummaryContentContractModel? result = await _textualSummarizationApiAccess.GetVideoSummaryAsync(location, accountId, videoId, summaryId, accessToken);

                return result is null ? null :  _summarizationJobWithSummaryContentContractMapper.MapFrom(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "動画要約取得中に予期せぬエラー: location={Location}, accountId={AccountId}, videoId={VideoId}, summaryId={SummaryId}", location, accountId, videoId, summaryId);
                return null;
            }
        }

        /// <summary>
        /// 動画に紐づくすべてのテキスト要約メタ情報をリスト形式で取得します。
        /// List Video Summaries
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Video-Summaries
        /// </summary>
        /// <param name="videoId">対象の動画の ID</param>
        /// <param name="pageNumber">取得するページ番号（0 から始まる。省略時は 0）</param>
        /// <param name="pageSize">1ページあたりの最大件数（最大20、デフォルト20）</param>
        /// <param name="state">フィルタ対象の状態（例: "Processed", "Failed" など）</param>
        /// <returns>TextualSummarizationContractPage（要約リスト、ページ情報を含む）。失敗時は null。</returns>
        public async Task<TextualSummarizationContractPageModel?> ListVideoSummariesAsync(string videoId, int? pageNumber = null, int? pageSize = null, string[]? state = null)
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

            // API呼び出し
            return await ListVideoSummariesAsync(location!, accountId!, videoId, pageNumber, pageSize, state, accessToken);
        }

        /// <summary>
        /// 動画に紐づくすべてのテキスト要約メタ情報をリスト形式で取得します。
        /// List Video Summaries
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Video-Summaries
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "trial"、"japaneast"）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象の動画の ID</param>
        /// <param name="pageNumber">取得するページ番号（0 から始まる。省略時は 0）</param>
        /// <param name="pageSize">1ページあたりの最大件数（最大20、デフォルト20）</param>
        /// <param name="state">フィルタ対象の状態（例: "Processed", "Failed" など）</param>
        /// <param name="accessToken">アクセストークン（クエリ文字列に追加。省略可）</param>
        /// <returns>TextualSummarizationContractPage（要約リスト、ページ情報を含む）。失敗時は null。</returns>
        public async Task<TextualSummarizationContractPageModel?> ListVideoSummariesAsync(string location, string accountId, string videoId, int? pageNumber = null, int? pageSize = null, string[]? state = null, string? accessToken = null)
        {
            try
            {
                ApiTextualSummarizationContractPageModel? apiResult = await _textualSummarizationApiAccess.ListVideoSummariesAsync(location, accountId, videoId, pageNumber, pageSize, state, accessToken);

                // マッピング（必要に応じて拡張）
                return apiResult is null ? null : _textualSummarizationContractPageMapper.MapFrom(apiResult);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, videoId={VideoId}, pageNumber={PageNumber}, pageSize={PageSize}", location, accountId, videoId, pageNumber, pageSize);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, videoId={VideoId}, pageNumber={PageNumber}, pageSize={PageSize}", location, accountId, videoId, pageNumber, pageSize);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "テキスト要約一覧取得中に予期せぬエラー: location={Location}, accountId={AccountId}, videoId={VideoId}, pageNumber={PageNumber}, pageSize={PageSize}", location, accountId, videoId, pageNumber, pageSize);
                return null;
            }
        }
    }
}
