namespace VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

public interface IDurableHttpClient
{
    string HttpClientName { get; set; }
    HttpClient DefaultHttpClient { get; }
    HttpClient HttpClient { get; }

    /// <summary>
    /// 新しいHttpClientの生成
    /// </summary>
    /// <returns></returns>
    HttpClient NewHttpClient();

    /// <summary>
    /// キャッシュのHttpClientの再生成
    /// </summary>
    /// <returns></returns>
    HttpClient RenewHttpClient();
}