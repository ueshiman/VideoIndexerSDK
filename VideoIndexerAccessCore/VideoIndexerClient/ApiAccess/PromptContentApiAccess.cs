using System.Net;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class PromptContentApiAccess : IPromptContentApiAccess
    {
        private readonly ILogger<PromptContentApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public PromptContentApiAccess(ILogger<PromptContentApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        // Create Prompt Content
        // 戻り値はboolかもしれない todo

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
        public async Task<bool> CreatePromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PromptContent";
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(modelName)) queryParams.Add($"modelName={modelName}");
                if (!string.IsNullOrEmpty(promptStyle)) queryParams.Add($"promptStyle={promptStyle}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");
                if (queryParams.Count > 0) requestUrl += "?" + string.Join("&", queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Response Content: {responseContent}", responseContent);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Accepted: break;    // Accepted (202) - リクエストは受け付けられたが、処理は完了していない
                    default:
                        _logger.LogWarning("Unexpected status code: {StatusCode}", response.StatusCode);
                        break;
                }
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("API request failed: {ex.Message}", ex.Message);
                throw;
            }
        }

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
        public async Task<string> FetchPromptContentJsonAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PromptContent";
                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(modelName)) queryParams.Add($"modelName={modelName}");
                if (!string.IsNullOrEmpty(promptStyle)) queryParams.Add($"promptStyle={promptStyle}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");
                if (queryParams.Count > 0) requestUrl += "?" + string.Join("&", queryParams);

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
                //return false; // todo
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("API request failed: {ex.Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON データをパースし、オブジェクトに変換します。
        /// Create Prompt Content
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Prompt-Content
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiPromptCreateResponseModel オブジェクト、エラー時は null</returns>
        public ApiPromptCreateResponseModel? ParsePromptContentJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiPromptCreateResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing error: {ex.Message}", ex.Message);
                return null;
            }
        }

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
        public async Task<ApiPromptCreateResponseModel?> GetPromptContentAsync(string location, string accountId, string videoId, string? modelName = null, string? promptStyle = null, string? accessToken = null)
        {
            var json = await FetchPromptContentJsonAsync(location, accountId, videoId, modelName, promptStyle, accessToken);
            return ParsePromptContentJson(json);
        }


        // Get Prompt Content

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
        public async Task<string> FetchPromptContentJsonAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PromptContent";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("API request failed: {ex.Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// JSON をパースして ApiPromptContentContractModel オブジェクトに変換します。
        /// Get Prompt Content
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Prompt-Content
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiPromptContentContractModel オブジェクト、エラー時は null</returns>
        public ApiPromptContentContractModel? ParseGetPromptContentJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiPromptContentContractModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing error: {ex.Message}", ex.Message);
                return null;
            }
        }

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
        public async Task<ApiPromptContentContractModel?> GetPromptContentAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            var json = await FetchPromptContentJsonAsync(location, accountId, videoId, accessToken);
            return ParseGetPromptContentJson(json);
        }
    }
}

