using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IRedactVideoRepository
{
    /// <summary>
    /// ビデオの編集 (Redact) を非同期で実行します。
    /// アカウント情報を取得し、アクセストークンを生成し、指定されたビデオを編集します。
    /// </summary>
    /// <param name="videoId">編集対象のビデオ ID</param>
    /// <param name="request">ビデオ編集リクエストモデル</param>
    /// <returns>編集リクエストの成功状態を示すブール値</returns>
    Task<bool> RedactVideoAsync(string videoId, RedactVideoRequestModel request);

    /// <summary>
    /// ビデオの編集 (Redact) を非同期で実行します。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="request">ビデオの編集リクエストモデル</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>編集リクエストの成功状態を示すブール値</returns>
    Task<bool> RedactVideoAsync(string location, string accountId, string videoId, RedactVideoRequestModel request, string? accessToken = null);
}