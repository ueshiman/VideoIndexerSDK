using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IJobsRepository
{
    Task<JobStatusResponseModel?> GetJobStatusAsync(string jobId);

    /// <summary>
    /// 指定されたロケーション・アカウントID・ジョブID・アクセストークンでジョブのステータスを取得します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="jobId">ジョブID</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>ジョブのステータス情報モデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<JobStatusResponseModel?> GetJobStatusAsync(string location, string accountId, string jobId, string? accessToken = null);
}