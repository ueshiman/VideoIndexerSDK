using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ITextualSummarizationApiAccess
{
    /// <summary>
    /// 指定されたビデオのテキスト要約を取得する非同期メソッド。
    /// Create Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
    /// </summary>
    /// <param name="location">API のリージョン名 (例: "trial")</param>
    /// <param name="accountId">Azure Video Indexer のアカウント ID (GUID 形式)</param>
    /// <param name="videoId">対象のビデオ ID (GUID 形式)</param>
    /// <param name="accessToken">API のアクセストークン (オプション、null の場合は未指定)</param>
    /// <param name="length">要約の長さ (Short, Medium, Long のいずれか)</param>
    /// <param name="style">要約のスタイル (Neutral, Casual, Formal のいずれか)</param>
    /// <param name="includedFrames">含めるフレーム (None, Keyframes のいずれか)</param>
    /// <returns>ビデオ要約のレスポンスモデル `ApiVideoSummaryModel` を返す。失敗時は null。</returns>
    Task<ApiVideoSummaryModel?> GetVideoSummaryAsync(
        string location, string accountId, string videoId, string? accessToken = null,
        string? length = null, string? style = null, string? includedFrames = null);

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
    Task<ApiVideoSummaryResponseModel?> GetVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null);

    /// <summary>
    /// API のエンドポイント URL を構築する。
    /// Create Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
    /// </summary>
    /// <returns>構築された API URL (クエリパラメータ含む)</returns>
    string BuildApiUrl(out string maskedUrl, string location, string accountId, string videoId, string? accessToken, string? length, string? style, string? includedFrames);

    /// <summary>
    /// 指定した URL へ GET リクエストを送り、JSON レスポンスを取得する。
    /// Create Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
    /// </summary>
    /// <param name="url">リクエストを送信する API の URL</param>
    /// <param name="maskedUrl"></param>
    /// <returns>API から取得した JSON レスポンス文字列</returns>
    Task<string> FetchApiResponseAsync(string url, string maskedUrl);

    /// <summary>
    /// JSON レスポンスを `ApiVideoSummaryModel` オブジェクトに変換する。
    /// Create Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Video-Summary
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースされた `ApiVideoSummaryModel` オブジェクト。失敗時は null。</returns>
    ApiVideoSummaryModel? ParseVideoSummaryJson(string json);

    /// <summary>
    /// ビデオのテキスト要約を削除する非同期メソッド。
    /// </summary>
    /// <param name="location">API のリージョン名 (例: "trial")</param>
    /// <param name="accountId">Azure Video Indexer のアカウント ID</param>
    /// <param name="videoId">対象ビデオの ID</param>
    /// <param name="summaryId">削除する要約の ID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>成功した場合 true、失敗した場合 false</returns>
    Task<bool> DeleteVideoSummaryAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null);

    /// <summary>
    /// API にアクセスして JSON 文字列を取得します。
    /// Get Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント GUID</param>
    /// <param name="videoId">動画 ID</param>
    /// <param name="summaryId">サマリー ID</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>取得した JSON 文字列</returns>
    Task<string> FetchVideoSummaryJsonAsync(string location, string accountId, string videoId, string summaryId, string? accessToken = null);

    /// <summary>
    /// JSON 文字列をパースして ApiVideoSummaryModel オブジェクトに変換します。
    /// Get Video Summary
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Summary
    /// </summary>
    /// <param name="json">動画要約の JSON データ</param>
    /// <returns>ApiVideoSummaryModel オブジェクト</returns>
    ApiVideoSummaryResponseModel? ParseVideoSummaryResponseJson(string json);

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
    Task<ApiTextualSummarizationContractPageModel?> ListVideoSummariesAsync(string location, string accountId, string videoId, int? pageNumber = null, int? pageSize = null, string[]? state = null, string? accessToken = null);

    /// <summary>
    /// テキスト要約の一覧 JSON を取得するために Video Indexer API を呼び出します。
    /// List Video Summaries
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Video-Summaries
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント GUID</param>
    /// <param name="videoId">動画 ID</param>
    /// <param name="pageNumber">ページ番号</param>
    /// <param name="pageSize">ページあたりの件数</param>
    /// <param name="state">状態フィルター</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <returns>API から返される JSON 文字列</returns>
    Task<string> FetchVideoSummariesJsonAsync(string location, string accountId, string videoId, int? pageNumber, int? pageSize, string[]? state, string? accessToken);

    /// <summary>
    /// テキスト要約一覧の JSON を ApiTextualSummarizationContractPageModel にパースします。
    /// List Video Summaries
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Video-Summaries
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>ApiTextualSummarizationContractPageModel オブジェクト</returns>
    ApiTextualSummarizationContractPageModel? ParseVideoSummariesJson(string json);
}