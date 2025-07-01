using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IPromptContentApiAccess
{
    /// <summary>
    /// API へプロンプトコンテンツを作成をリクエストするメソッドです。
    /// Create Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Prompt-Content
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="modelName">使用する LLM モデル名（オプション）</param>
    /// <param name="promptStyle">プロンプトのスタイル（オプション）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>API 呼び出しの成功可否を示す bool 値</returns>
    Task<bool> CreatePromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null);

    /// <summary>
    /// API からプロンプトコンテンツの JSON データを取得します。
    /// Create Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Prompt-Content
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="modelName">使用する LLM モデル名（オプション）</param>
    /// <param name="promptStyle">プロンプトのスタイル（オプション）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>API から取得した JSON 文字列</returns>
    Task<string> FetchPromptContentJsonAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null);

    /// <summary>
    /// API からプロンプトコンテンツの JSON データを取得します。
    /// Get Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Prompt-Content
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>API から取得した JSON 文字列</returns>
    Task<string> FetchPromptContentJsonAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// 取得した JSON データをパースし、オブジェクトに変換します。
    /// Create Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Prompt-Content
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースした ApiPromptCreateResponseModel オブジェクト、エラー時は null</returns>
    ApiPromptContentContractModel? ParsePromptContentJson(string json);

    /// <summary>
    /// API を呼び出してプロンプトコンテンツのデータを取得します。
    /// Create Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Prompt-Content
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="modelName">使用する LLM モデル名（オプション）</param>
    /// <param name="promptStyle">プロンプトのスタイル（オプション）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>プロンプトコンテンツのデータ、エラー時は null</returns>
    Task<ApiPromptContentContractModel?> GetPromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null);

    /// <summary>
    /// API を呼び出してプロンプトコンテンツのデータを取得します。
    /// Get Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Prompt-Content
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>プロンプトコンテンツのデータ、エラー時は null</returns>
    Task<ApiPromptContentContractModel?> GetPromptContentAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// JSON をパースして ApiPromptContentContractModel オブジェクトに変換します。
    /// Get Prompt Content
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Prompt-Content
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースした ApiPromptContentContractModel オブジェクト、エラー時は null</returns>
    ApiPromptContentContractModel? ParseGetPromptContentJson(string json);
}