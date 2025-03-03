using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ILanguagesApiAccess
{
    /// <summary>
    /// API から JSON を取得する
    /// </summary>
    /// <param name="location">API のリクエストを送る Azure のリージョン (例: "trial")</param>
    /// <returns>取得した JSON データ（文字列）</returns>
    Task<string> FetchSupportedLanguagesJsonAsync(string location);

    /// <summary>
    /// JSON データをパースして言語リストに変換する
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>サポートされている言語のリスト</returns>
    List<ApiSupportedLanguageModel> ParseSupportedLanguagesJson(string json);

    /// <summary>
    /// サポートされている言語の一覧を取得する
    /// </summary>
    /// <param name="location">API のリクエストを送る Azure のリージョン (例: "trial")</param>
    /// <returns>サポートされている言語のリスト</returns>
    Task<List<ApiSupportedLanguageModel>> GetSupportedLanguagesAsync(string location);
}