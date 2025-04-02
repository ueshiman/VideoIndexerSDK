using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class LanguagesApiAccess : ILanguagesApiAccess
    {
        private readonly ILogger<LanguagesApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public LanguagesApiAccess(ILogger<LanguagesApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// API から JSON を取得する
        /// </summary>
        /// <param name="location">API のリクエストを送る Azure のリージョン (例: "trial")</param>
        /// <returns>取得した JSON データ（文字列）</returns>
        public async Task<string> FetchSupportedLanguagesJsonAsync(string location)
        {
            var apiBaseUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/SupportedLanguages"; // location を動的に設定

            try
            {
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(apiBaseUrl) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                response.EnsureSuccessStatusCode(); // HTTP エラー時に例外を発生

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// JSON データをパースして言語リストに変換する
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>サポートされている言語のリスト</returns>
        public List<ApiSupportedLanguageModel> ParseSupportedLanguagesJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<List<ApiSupportedLanguageModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? [];
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// サポートされている言語の一覧を取得する
        /// </summary>
        /// <param name="location">API のリクエストを送る Azure のリージョン (例: "trial")</param>
        /// <returns>サポートされている言語のリスト</returns>
        public async Task<List<ApiSupportedLanguageModel>> GetSupportedLanguagesAsync(string location)
        {
            try
            {
                string json = await FetchSupportedLanguagesJsonAsync(location);
                return ParseSupportedLanguagesJson(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                throw;
            }
        }
    }
}
