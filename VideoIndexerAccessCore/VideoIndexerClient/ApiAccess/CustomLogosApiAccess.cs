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
    public class CustomLogosApiAccess
    {
        private readonly ILogger<CustomLogosApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logger">ロガーインスタンス</param>
        /// <param name="durableHttpClient">HTTPクライアントインスタンス</param>
        /// <param name="apiResourceConfigurations">APIリソース設定インスタンス</param>
        public CustomLogosApiAccess(ILogger<CustomLogosApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
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
                // リクエストデータをJSON形式にシリアライズ
                var jsonContent = JsonSerializer.Serialize(request);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // リクエストURIを構築
                var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Logos";

                // アクセストークンが指定されている場合、URIに追加
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUri += $"?accessToken={accessToken}";
                }

                _logger.LogInformation("Sending request to {Url}", requestUri);

                // HTTPクライアントを取得し、POSTリクエストを送信
                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PostAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }

            // responseがnullなら例外をスロー
            if (response is null) throw new HttpRequestException("The response was null.");

            // 成功した場合、レスポンス内容を文字列として返す
            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            // エラーが発生した場合、エラーレスポンスをログに記録し、例外をスロー
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

                string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/CustomLogos/Groups";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUri += $"?accessToken={accessToken}";
                }

                _logger.LogInformation("Sending request to {Url}", requestUri);

                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.PostAsync(requestUri, httpContent);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request failed: {Message}", ex.Message);
                throw;
            }


            // responseがnullなら例外をスロー
            if (response is null) throw new HttpRequestException("The response was null.");

            // 成功した場合、レスポンス内容を文字列として返す
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

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUri += $"?accessToken={accessToken}";
                }

                _logger.LogInformation("Sending DELETE request to {Url}", requestUri);

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

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUri += $"?accessToken={accessToken}";
                }

                _logger.LogInformation("Sending DELETE request to {Url}", requestUri);

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
    }
}
