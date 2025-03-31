using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IJobsApiAccess
{
    /// <summary>
    /// 指定されたジョブ ID に対するステータスを取得する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="jobId">ジョブ ID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>ジョブのステータス情報を含むレスポンス</returns>
    Task<ApiJobStatusResponseModel?> GetJobStatusAsync(string location, string accountId, string jobId, string? accessToken = null);

    /// <summary>
    /// 指定された URL からジョブステータスの JSON を取得する
    /// </summary>
    /// <param name="url">API のエンドポイント URL</param>
    /// <returns>ジョブステータスの JSON 文字列</returns>
    Task<string> FetchJobStatusJsonAsync(string url);

    /// <summary>
    /// API から取得した JSON を解析し、JobStatusResponse オブジェクトに変換する
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>解析後の JobStatusResponse オブジェクト</returns>
    ApiJobStatusResponseModel? ParseJobStatusJson(string json);
}