using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class SpeechCustomizationApiAccess
    {
        private readonly ILogger<SpeechCustomizationApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public SpeechCustomizationApiAccess(ILogger<SpeechCustomizationApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// API からスピーチデータセット作成の JSON データを取得します。
        /// Create Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>API から取得した JSON 文字列</returns>
        public async Task<string> FetchCreateSpeechDatasetJsonAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null)
        {
            try
            {
                _logger.LogInformation("Starting to fetch create speech dataset JSON. Location: {Location}, AccountId: {AccountId}", location, accountId);
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = content
                };
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(httpRequest);
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();
                _logger.LogInformation("Successfully fetched create speech dataset JSON. Location: {Location}, AccountId: {AccountId}", location, accountId);
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP request error while fetching create speech dataset JSON: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("JSON serialization error while fetching create speech dataset JSON: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error while fetching create speech dataset JSON: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON をパースして ApiSpeechDatasetUpdateModel オブジェクトに変換します。
        /// Create Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiSpeechDatasetUpdateModel オブジェクト、エラー時は null</returns>
        public ApiModel.ApiSpeechDatasetResponseModel? ParseSpeechDatasetResponseJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiModel.ApiSpeechDatasetResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API を呼び出してスピーチデータセットを作成します。
        /// Create Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>作成したスピーチデータセット情報、エラー時は null</returns>
        public async Task<ApiModel.ApiSpeechDatasetResponseModel?> CreateSpeechDatasetAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null)
        {
            var json = await FetchCreateSpeechDatasetJsonAsync(location, accountId, request, accessToken);
            return ParseSpeechDatasetResponseJson(json);
        }


        // Create Speech Model

        /// <summary>
        /// API からスピーチモデル作成の JSON データを取得します。
        /// Create Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチモデルのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>API から取得した JSON 文字列</returns>
        public async Task<string> FetchCreateSpeechModelJsonAsync(string location, string accountId, ApiSpeechModelRequestModel request, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/models";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var jsonContent = JsonSerializer.Serialize(request);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var httpRequest = new HttpRequestMessage(HttpMethod.Post, requestUrl)
                {
                    Content = content
                };
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(httpRequest);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON serialization error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出してスピーチモデルを作成します。
        /// Create Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="request">スピーチモデルのリクエストオブジェクト</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>作成したスピーチモデル情報、エラー時は null</returns>
        public async Task<ApiSpeechModelResponseModel?> CreateSpeechModelAsync(string location, string accountId, ApiSpeechModelRequestModel request, string? accessToken = null)
        {
            var json = await FetchCreateSpeechModelJsonAsync(location, accountId, request, accessToken);
            return ParseSpeechModelJson(json);
        }

        /// <summary>
        /// JSON をパースして ApiSpeechModelResponseModel オブジェクトに変換します。
        /// Create Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>パースした ApiSpeechModelResponseModel オブジェクト、エラー時は null</returns>
        public ApiSpeechModelResponseModel? ParseSpeechModelJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiSpeechModelResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                return null;
            }
        }

        // Delete Speech Dataset

        /// <summary>
        /// API からスピーチデータセット削除リクエストを送信します。
        /// Delete Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">削除するデータセットの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets/{datasetId}";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var httpRequest = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(httpRequest);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to delete speech dataset: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete speech dataset request failed: {ex.Message}");
                return false;
            }
        }

        // Delete Speech Model

        /// <summary>
        /// API からスピーチモデル削除リクエストを送信します。
        /// Delete Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="modelId">削除するスピーチモデルの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/models/{modelId}";
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                var httpRequest = new HttpRequestMessage(HttpMethod.Delete, requestUrl);
                if (!string.IsNullOrEmpty(accessToken))
                {
                    httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(httpRequest);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    _logger.LogError($"Failed to delete speech model: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Delete speech model request failed: {ex.Message}");
                return false;
            }
        }

        // Get Speech Dataset

        /// <summary>
        /// API からスピーチデータセットを取得します。
        /// Get Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセット情報を含む ApiSpeechDatasetUpdateModel オブジェクト</returns>
        public async Task<ApiModel.ApiSpeechDatasetModel?> GetSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null)
        {
            try
            {
                var responseContent = await FetchSpeechDatasetJsonAsync(location, accountId, datasetId, accessToken);
                return responseContent != null ? ParseSpeechDatasetJson(responseContent) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get speech dataset request failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API から JSON を取得するメソッド。
        /// Get Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
        public async Task<string?> FetchSpeechDatasetJsonAsync(string location, string accountId, string datasetId, string? accessToken)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets/{datasetId}";
            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUrl += $"?accessToken={accessToken}";
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"Failed to get speech dataset: {response.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// JSON を ApiSpeechDatasetUpdateModel オブジェクトにパースするメソッド。
        /// Get Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
        /// </summary>
        /// <param name="jsonContent">JSON 形式のレスポンス</param>
        /// <returns>パースした ApiSpeechDatasetUpdateModel オブジェクト。パースに失敗した場合は null。</returns>
        public ApiModel.ApiSpeechDatasetModel? ParseSpeechDatasetJson(string jsonContent)
        {
            return JsonSerializer.Deserialize<ApiModel.ApiSpeechDatasetModel>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Get Speech Dataset Files

        /// <summary>
        /// API からスピーチデータセットのファイル一覧を取得します。
        /// Get Speech Dataset Files
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセットのファイルリストを含むリスト。取得できなかった場合は null。</returns>
        public async Task<List<ApiSpeechDatasetFileModel>?> GetSpeechDatasetFilesAsync(string location, string accountId, string datasetId, int? sasValidityInSeconds = null, string? accessToken = null)
        {
            try
            {
                var responseContent = await FetchSpeechDatasetFilesJsonAsync(location, accountId, datasetId, sasValidityInSeconds, accessToken);
                return responseContent != null ? ParseSpeechDatasetFilesJson(responseContent) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get speech dataset files request failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API から JSON を取得するメソッド。
        /// Get Speech Dataset Files
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
        public async Task<string?> FetchSpeechDatasetFilesJsonAsync(string location, string accountId, string datasetId, int? sasValidityInSeconds, string? accessToken)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets/{datasetId}/files";
            var queryParams = new List<string>();

            if (sasValidityInSeconds.HasValue)
            {
                queryParams.Add($"sasValidityInSeconds={sasValidityInSeconds.Value}");
            }
            if (!string.IsNullOrEmpty(accessToken))
            {
                queryParams.Add($"accessToken={accessToken}");
            }

            if (queryParams.Count > 0)
            {
                requestUrl += "?" + string.Join("&", queryParams);
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"Failed to get speech dataset files: {response.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// JSON を ApiSpeechDatasetFileModel のリストにパースするメソッド。
        /// Get Speech Dataset Files
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
        /// </summary>
        /// <param name="jsonContent">JSON 形式のレスポンス</param>
        /// <returns>パースした ApiSpeechDatasetFileModel のリスト。パースに失敗した場合は null。</returns>
        public List<ApiSpeechDatasetFileModel>? ParseSpeechDatasetFilesJson(string jsonContent)
        {
            try
            {
                return JsonSerializer.Deserialize<List<ApiSpeechDatasetFileModel>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Failed to parse speech dataset files JSON: {ex.Message}");
                return null;
            }
        }

        // Get Speech Datasets

        /// <summary>
        /// API からスピーチデータセット一覧を取得します。
        /// Get Speech Datasets
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="locale">取得するデータセットのロケール（省略可、null の場合はすべて取得）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセットのリスト。取得できなかった場合は null。</returns>
        public async Task<List<ApiModel.ApiSpeechDatasetModel>?> GetSpeechDatasetsAsync(string location, string accountId, string? locale = null, string? accessToken = null)
        {
            try
            {
                var responseContent = await FetchSpeechDatasetsJsonAsync(location, accountId, locale, accessToken);
                return responseContent != null ? ParseSpeechDatasetsJson(responseContent) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get speech datasets request failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API から JSON を取得するメソッド。
        /// Get Speech Datasets
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="locale">取得するデータセットのロケール（省略可）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
        private async Task<string?> FetchSpeechDatasetsJsonAsync(string location, string accountId, string? locale, string? accessToken)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/datasets";
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(locale))
            {
                queryParams.Add($"locale={locale}");
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                queryParams.Add($"accessToken={accessToken}");
            }

            if (queryParams.Count > 0)
            {
                requestUrl += "?" + string.Join("&", queryParams);
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"Failed to get speech datasets: {response.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// JSON を List＜ApiSpeechDatasetUpdateModel＞? にパースするメソッド。
        /// Get Speech Datasets
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
        /// </summary>
        /// <param name="jsonContent">JSON 形式のレスポンス</param>
        /// <returns>パースしたスピーチデータセットのリスト。パースに失敗した場合は null。</returns>
        private List<ApiModel.ApiSpeechDatasetModel>? ParseSpeechDatasetsJson(string jsonContent)
        {
            try
            {
                return JsonSerializer.Deserialize<List<ApiModel.ApiSpeechDatasetModel>>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Failed to parse speech datasets JSON: {ex.Message}");
                return null;
            }
        }

        // Get Speech Model

        /// <summary>
        /// API からスピーチモデルを取得します。
        /// Get Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="modelId">取得するスピーチモデルの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチモデル情報。取得できなかった場合は null。</returns>
        public async Task<ApiCustomSpeechModel?> GetSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                var responseContent = await FetchSpeechModelJsonAsync(location, accountId, modelId, accessToken);
                return responseContent != null ? ParseCustomSpeechModelJson(responseContent) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Get speech model request failed: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// API から JSON を取得するメソッド。
        /// Get Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="modelId">取得するスピーチモデルの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
        private async Task<string?> FetchSpeechModelJsonAsync(string location, string accountId, string modelId, string? accessToken)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Speech/models/{modelId}";
            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUrl += $"?accessToken={accessToken}";
            }

            var httpRequest = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            if (!string.IsNullOrEmpty(accessToken))
            {
                httpRequest.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(httpRequest);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                _logger.LogError($"Failed to get speech model: {response.StatusCode}");
                return null;
            }
        }

        /// <summary>
        /// JSON を ApiCustomSpeechModel にパースするメソッド。
        /// Get Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="jsonContent">JSON 形式のレスポンス</param>
        /// <returns>パースしたスピーチモデル情報。パースに失敗した場合は null。</returns>
        private ApiCustomSpeechModel? ParseCustomSpeechModelJson(string jsonContent)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiCustomSpeechModel>(jsonContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Failed to parse speech model JSON: {ex.Message}");
                return null;
            }
        }

        // Get Speech Models

        /// <summary>
        /// APIからスピーチモデルを取得します。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>取得した <see cref="ApiCustomSpeechModel"/> を含む非同期タスク。</returns>
        public async Task<ApiCustomSpeechModel?> GetSpeechModelsAsync(string location, string accountId, string? locale = null, string? accessToken = null)
        {
            var jsonResponse = await FetchJsonAsync(location, accountId, locale, accessToken);
            return ParseSpeechModel(jsonResponse);
        }

        /// <summary>
        /// スピーチモデルAPIからJSONレスポンスを取得します。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>APIから取得した生のJSONレスポンス（文字列）。</returns>
        private async Task<string> FetchJsonAsync(string location, string accountId, string? locale, string? accessToken)
        {
            var requestUri = BuildRequestUri(location, accountId, locale, accessToken);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(requestUri);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// スピーチモデルを取得するためのリクエストURIを構築します。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>構築されたリクエストURI（文字列）。</returns>
        private string BuildRequestUri(string location, string accountId, string? locale, string? accessToken)
        {
            var requestUri = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Customization/Speech/models";
            var queryParams = new List<string>();

            if (!string.IsNullOrEmpty(locale))
            {
                queryParams.Add($"locale={locale}");
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                queryParams.Add($"accessToken={accessToken}");
            }

            if (queryParams.Count > 0)
            {
                requestUri += "?" + string.Join("&", queryParams);
            }

            return requestUri;
        }

        /// <summary>
        /// JSONレスポンスを <see cref="ApiCustomSpeechModel"/> オブジェクトにパースします。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="jsonResponse">APIから取得したJSONレスポンス。</param>
        /// <returns>パースされた <see cref="ApiCustomSpeechModel"/> オブジェクト。</returns>
        private ApiCustomSpeechModel? ParseSpeechModel(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiCustomSpeechModel>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        // Update Speech Dataset
        // https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset

        /// <summary>
        /// スピーチデータセットを更新します。
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="datasetId">更新するスピーチデータセットのID。</param>
        /// <param name="displayName">更新するデータセットの表示名。</param>
        /// <param name="description">更新するデータセットの説明。</param>
        /// <param name="customProperties">更新するデータセットのカスタムプロパティ。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>更新された <see cref="ApiSpeechDatasetUpdateModel"/> を含む非同期タスク。</returns>
        public async Task<ApiSpeechDatasetUpdateModel?> UpdateSpeechDatasetAsync(string location, string accountId, string datasetId, string? displayName, string? description, Dictionary<string, string>? customProperties, string? accessToken = null)
        {
            var updateRequestModelData = new ApiSpeechDatasetUpdateRequestModel
            {
                displayName = displayName,
                description = description,
                customProperties = customProperties
            };

            return await UpdateSpeechDatasetAsync(location, accountId, datasetId, updateRequestModelData, accessToken);
        }

        /// <summary>
        /// スピーチデータセットを更新します。
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="datasetId">更新するスピーチデータセットのID。</param>
        /// <param name="updateRequestModelData">更新するデータセットの情報。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>更新された <see cref="ApiSpeechDatasetUpdateModel"/> を含む非同期タスク。</returns>
        public async Task<ApiSpeechDatasetUpdateModel?> UpdateSpeechDatasetAsync(string location, string accountId, string datasetId, ApiSpeechDatasetUpdateRequestModel updateRequestModelData, string? accessToken = null)
        {
            _logger.LogInformation("Starting speech dataset update. Dataset ID: {DatasetId}", datasetId);
            var jsonResponse = await FetchUpdateJsonAsync(location, accountId, datasetId, updateRequestModelData, accessToken);
            _logger.LogInformation("Speech dataset update completed. Dataset ID: {DatasetId}", datasetId);
            return ParseSpeechDataset(jsonResponse);
        }

        /// <summary>
        /// スピーチデータセットの更新APIを呼び出し、JSONレスポンスを取得します。
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="datasetId">更新対象のスピーチデータセットのID（GUID）。</param>
        /// <param name="updateRequestModelData">更新内容を含むオブジェクト。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>APIのレスポンスとして返されるJSON文字列。</returns>
        private async Task<string> FetchUpdateJsonAsync(string location, string accountId, string datasetId, ApiSpeechDatasetUpdateRequestModel updateRequestModelData, string? accessToken)
        {
            var requestUri = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Customization/Speech/datasets/{datasetId}";
            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUri += "?accessToken=" + accessToken;
            }

            _logger.LogDebug("Sending speech dataset update request: {RequestUri}", requestUri);

            var content = new StringContent(JsonSerializer.Serialize(updateRequestModelData), Encoding.UTF8, "application/json");
            try
            {
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.PutAsync(requestUri, content);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Speech dataset update response: {ResponseBody}", responseBody);
                return responseBody;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update speech dataset. Dataset ID: {DatasetId}", datasetId);
                throw;
            }
        }

        /// <summary>
        /// APIから取得したスピーチデータセットのJSONレスポンスを解析し、SpeechDatasetオブジェクトに変換します。
        /// </summary>
        /// <param name="jsonResponse">APIから返されたJSON文字列。</param>
        /// <returns>解析されたSpeechDatasetオブジェクト。</returns>
        private ApiSpeechDatasetUpdateModel? ParseSpeechDataset(string jsonResponse)
        {
            _logger.LogDebug("Parsing speech dataset response...");
            return JsonSerializer.Deserialize<ApiSpeechDatasetUpdateModel>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}