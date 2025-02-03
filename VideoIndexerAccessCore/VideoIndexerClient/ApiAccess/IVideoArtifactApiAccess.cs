namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IVideoArtifactApiAccess
{
    /// <summary>
    /// 指定されたビデオのアーティファクトURLを非同期で取得します。
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoId">ビデオ ID</param>
    /// <param name="accessToken">アクセス トークン</param>
    /// <param name="artifactType">アーティファクトの種類</param>
    /// <returns>ビデオのアーティファクトURL</returns>
    Task<string> GetVideoArtifactUrlAsync(string location, string accountId, string videoId, string? accessToken = null, string? artifactType = null);
}