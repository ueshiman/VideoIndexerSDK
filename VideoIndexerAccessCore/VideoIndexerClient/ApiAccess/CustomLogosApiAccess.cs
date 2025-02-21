using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// ロゴ関連のAPIを操作するサービスクラス
    /// </summary>
    public class CustomLogosApiAccess : ICustomLogosApiAccess
    {
        private readonly ILogger<CustomLogosApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;
        private readonly ISecureLogMessageBuilder _secureLogMessageBuilder;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガーインスタンス</param>
        /// <param name="durableHttpClient">HTTPクライアントインスタンス</param>
        /// <param name="apiResourceConfigurations">APIリソース設定インスタンス</param>
        public CustomLogosApiAccess(ILogger<CustomLogosApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations, ISecureLogMessageBuilder secureLogMessageBuilder)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
            _secureLogMessageBuilder = secureLogMessageBuilder;
        }

        /// <summary>
        /// API にカスタムロゴを作成するリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ロゴ作成リクエストデータ</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>作成されたロゴのレスポンス情報</returns>
        public async Task<ApiLogoResponseModel> CreateCustomLogoAsync(string location, string accountId, ApiLogoRequestModel request, string? accessToken = null)
        {
            // APIリクエストを送信し、レスポンスを取得
            var jsonResponse = await SendApiRequestAsync(location, accountId, request, accessToken);
            // レスポンスを解析し、モデルに変換して返す
            return ParseApiResponse(jsonResponse);
        }

        /// <summary>
        /// API にリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ロゴ作成リクエストデータ</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>APIからのレスポンス文字列</returns>
        public async Task<string> SendApiRequestAsync(string location, string accountId, ApiLogoRequestModel request, string? accessToken)
        {
            HttpResponseMessage? response;
            try
            {
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos";
                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PostAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError("API Error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
            throw new HttpRequestException($"API Error: {response.StatusCode}");
        }

        /// <summary>
        /// API レスポンスの JSON を解析する
        /// </summary>
        /// <param name="jsonResponse">APIからのレスポンス文字列</param>
        /// <returns>解析されたレスポンスモデル</returns>
        public ApiLogoResponseModel ParseApiResponse(string jsonResponse)
        {
            try
            {
                // JSONレスポンスをデシリアライズし、モデルに変換
                return JsonSerializer.Deserialize<ApiLogoResponseModel>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API にロゴグループを作成するリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="request">ロゴグループ作成リクエストデータ</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>作成されたロゴグループのレスポンス情報</returns>
        public async Task<ApiLogoGroupResponseModel> CreateLogoGroupAsync(string location, string accountId, ApiLogoGroupRequestModel request, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await SendPostRequestAsync(location, accountId, request, accessToken);
                return ParseLogoGroupJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error creating logo group: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// POST リクエストを送信する
        /// </summary>
        public async Task<string> SendPostRequestAsync(string location, string accountId, ApiLogoGroupRequestModel request, string? accessToken)
        {
            HttpResponseMessage? response;
            try
            {
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups";
                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PostAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError("API Error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
            throw new HttpRequestException($"API Error: {response.StatusCode}");
        }

        /// <summary>
        /// JSON を解析しロゴグループレスポンスモデルを生成する
        /// </summary>
        public ApiLogoGroupResponseModel ParseLogoGroupJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiLogoGroupResponseModel>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
        }

        /// <summary>
        /// API にロゴを削除するリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">削除するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        public async Task DeleteLogoAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos/{logoId}";
                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending DELETE request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                using var response = await httpClient.DeleteAsync(requestUri);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API にロゴグループを削除するリクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">削除するロゴグループのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        public async Task DeleteLogoGroupAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups/{logoGroupId}";
                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending DELETE request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                using var response = await httpClient.DeleteAsync(requestUri);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API からロゴ情報の JSON を取得する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>取得したロゴ情報の JSON 文字列</returns>
        public async Task<string> GetLogoJsonAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos/{logoId}";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending GET request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            
            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON を解析しロゴレスポンスモデルを生成する
        /// </summary>
        /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
        /// <returns>解析したロゴレスポンスモデル</returns>
        public ApiLogoContractResponseModel ParseLogoJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiLogoContractResponseModel>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
        }

        /// <summary>
        /// ロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴレスポンスモデル</returns>
        public async Task<ApiLogoContractResponseModel> GetLogoAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await GetLogoJsonAsync(location, accountId, logoId, accessToken);
                return ParseLogoJson(jsonResponse);
            }
            catch (Exception e)
            {
                _logger.LogError("Error Get Logo: {Message}", e.Message);
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// API からロゴグループに関連するすべてのロゴ情報の JSON を取得する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">取得するロゴグループのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>取得したロゴグループに関連するロゴ情報の JSON 文字列</returns>
        public async Task<string> GetLogoGroupLinkedLogosJsonAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups/{logoGroupId}/Logos";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending GET request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON を解析しロゴグループに関連するロゴ情報のリストを生成する
        /// </summary>
        /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
        /// <returns>解析したロゴリスト</returns>
        public ApiLogoContractResponseModel[] ParseLogoGroupLinkedLogosJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiLogoContractResponseModel[]>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// ロゴグループに関連するロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">取得するロゴグループのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴグループに関連するロゴリスト</returns>
        public async Task<ApiLogoContractResponseModel[]> GetLogoGroupLinkedLogosAsync(string location, string accountId, string logoGroupId, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await GetLogoGroupLinkedLogosJsonAsync(location, accountId, logoGroupId, accessToken);
                return ParseLogoGroupLinkedLogosJson(jsonResponse);
            }

            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API からすべてのロゴグループ情報の JSON を取得する
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ロゴグループ情報の JSON 文字列</returns>
        /// </summary>
        public async Task<string> GetLogoGroupsJsonAsync(string location, string accountId, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending GET request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }
            
            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON を解析しロゴグループ情報のリストを生成する
        /// <param name="jsonResponse">API から取得したロゴグループ情報の JSON</param>
        /// <returns>解析したロゴグループのリスト</returns>
        /// </summary>
        public ApiLogoGroupContractResponseModel[] ParseLogoGroupsJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiLogoGroupContractResponseModel[]>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// すべてのロゴグループ情報を取得し、オブジェクトとして返す
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴグループ情報のリスト</returns>
        /// </summary>
        public async Task<ApiLogoGroupContractResponseModel[]> GetLogoGroupsAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await GetLogoGroupsJsonAsync(location, accountId, accessToken);
                return ParseLogoGroupsJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API から特定のロゴが関連するロゴグループ情報の JSON を取得する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">取得するロゴのID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ロゴグループ情報の JSON 文字列</returns>
        public async Task<string> GetLogoLinkedGroupsJsonAsync(string location, string accountId, string logoId, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos/{logoId}/Groups";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending GET request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON を解析しロゴが関連するロゴグループ情報のリストを生成する
        /// </summary>
        /// <param name="jsonResponse">API から取得したロゴグループ情報の JSON</param>
        /// <returns>解析したロゴグループのリスト</returns>
        public ApiLogoGroupContractResponseModel[] ParseLogoLinkedGroupsJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiLogoGroupContractResponseModel[]>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API からすべてのロゴ情報の JSON を取得する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ロゴ情報の JSON 文字列</returns>
        public async Task<string> GetLogosJsonAsync(string location, string accountId, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                _logger.LogInformation("Sending GET request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

                response = await httpClient.GetAsync(requestUri);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }
            
            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON を解析しロゴ情報のリストを生成する
        /// </summary>
        /// <param name="jsonResponse">API から取得したロゴ情報の JSON</param>
        /// <returns>解析したロゴのリスト</returns>
        public ApiLogoContractResponseModel[] ParseLogosJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiLogoContractResponseModel[]>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// すべてのロゴ情報を取得し、オブジェクトとして返す
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>解析済みのロゴ情報のリスト</returns>
        public async Task<ApiLogoContractResponseModel[]> GetLogosAsync(string location, string accountId, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await GetLogosJsonAsync(location, accountId, accessToken);
                return ParseLogosJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }
        }
        /// <summary>
        /// API にロゴ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoId">更新するロゴのID</param>
        /// <param name="updateRequest">更新するロゴ情報</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>更新後のロゴ情報</returns>
        public async Task<ApiLogoContractResponseModel> UpdateLogoAsync(string location, string accountId, string logoId, ApiLogoUpdateRequestModel updateRequest, string? accessToken = null)
        {
            HttpResponseMessage? response;

            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos/{logoId}";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                var jsonContent = JsonSerializer.Serialize(updateRequest);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending PUT request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PutAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiLogoContractResponseModel>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
        }

        /// <summary>
        /// API にロゴグループ情報の更新リクエストを送信する
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="logoGroupId">更新するロゴグループのID</param>
        /// <param name="updateRequest">更新するロゴグループ情報</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>更新後のロゴグループ情報</returns>
        public async Task<ApiLogoGroupContractResponseModel> UpdateLogoGroupAsync(string location, string accountId, string logoGroupId, ApiLogoGroupUpdateRequestModel updateRequest, string? accessToken = null)
        {
            HttpResponseMessage? response;
            try
            {
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups/{logoGroupId}";

                requestUri = _secureLogMessageBuilder.BuildRequestUri(requestUri, accessToken, out var logUrl);

                var jsonContent = JsonSerializer.Serialize(updateRequest);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending PUT request to {Url}", logUrl);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PutAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError("Request timeout: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON parsing failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {Message}", ex.Message);
                throw;
            }

            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiLogoGroupContractResponseModel>(jsonResponse) ?? throw new JsonException("Failed to deserialize response.");
        }
    }
}

