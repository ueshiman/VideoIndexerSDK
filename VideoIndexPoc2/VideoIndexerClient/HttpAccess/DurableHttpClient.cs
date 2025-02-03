using Microsoft.Extensions.Logging;
using System.Net.Http;
using VideoIndexPoc2.VideoIndexerClient.Configuration;
using VideoIndexPoc2.VideoIndexerClient.Model;
using HttpClient = System.Net.Http.HttpClient;

namespace VideoIndexPoc2.VideoIndexerClient.HttpAccess
{
    public class DurableHttpClient : IDurableHttpClient
    {
        public const string DefaultHttpClientName = "AccountAccess-client";

        private readonly ILogger<DurableHttpClient> _logger;
        private readonly IHttpClientFactory? _httpClientFactory;
        private IApiResourceConfigurations _apiResourceConfigurations;

        public string HttpClientName { get; set; } = DefaultHttpClientName;

        private readonly HttpClient _defaultHttpClient;
        private HttpClient _httpClient;

        public HttpClient DefaultHttpClient => _defaultHttpClient;
        public HttpClient HttpClient => _httpClient;

        public DurableHttpClient(ILogger<DurableHttpClient> logger, IApiResourceConfigurations apiResourceConfigurations, IHttpClientFactory? httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _apiResourceConfigurations = apiResourceConfigurations;
            HttpClientName = _apiResourceConfigurations.DefaultHttpClientName ?? DefaultHttpClientName;

            _httpClient = _defaultHttpClient = NewHttpClient();
        }

        /// <summary>
        /// 新しいHttpClientの生成
        /// </summary>
        /// <returns></returns>
        public HttpClient NewHttpClient() => CreateClient(HttpClientName) ?? new HttpClient();

        private HttpClient? CreateClient(string httpClientName)
        {
            var newHttpClient = _httpClientFactory?.CreateClient(httpClientName);

            if (newHttpClient is null) _logger.LogWarning($"HttpClientFactory から HttpClient が取得できませんでした。HttpClientName: {httpClientName}");

            return newHttpClient;
        }

        /// <summary>
        /// キャッシュのHttpClientの再生成
        /// </summary>
        /// <returns></returns>
        public HttpClient RenewHttpClient()
        {
            _httpClient = NewHttpClient();
            return _httpClient;
        }
    }
}
