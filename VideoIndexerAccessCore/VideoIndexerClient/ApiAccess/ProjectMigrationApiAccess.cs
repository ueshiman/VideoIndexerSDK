﻿using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    /// <summary>
    /// プロジェクトのマイグレーション情報を取得するためのクラス
    /// </summary>
    public partial class ProjectMigrationApiAccess : IProjectMigrationApiAccess
    {
        private readonly ILogger<ProjectMigrationApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        /// <summary>
        /// ProjectMigrationApiAccess クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="logger">ロガーインスタンス</param>
        /// <param name="durableHttpClient">HTTP クライアント</param>
        /// <param name="apiResourceConfigurations">API リソースの設定</param>
        public ProjectMigrationApiAccess(ILogger<ProjectMigrationApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// プロジェクトのマイグレーション情報を取得します。
        /// Get Project Migration
        /// Try it Get project migration
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <returns>プロジェクトのマイグレーション情報</returns>
        public async Task<ApiProjectMigrationModel> GetProjectMigrationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            _logger.LogInformation("Starting GetProjectMigrationAsync for accountId: {AccountId}, projectId: {ProjectId}", accountId, projectId);
            try
            {
                var json = await GetProjectMigrationJsonAsync(location, accountId, projectId, accessToken);
                var result = ParseProjectMigrationJson(json);
                _logger.LogInformation("Successfully retrieved project migration for accountId: {AccountId}, projectId: {ProjectId}", accountId, projectId);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Request error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API からプロジェクトのマイグレーションデータを JSON 文字列として取得します。
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <returns>プロジェクトのマイグレーションデータの JSON 文字列</returns>
        public async Task<string> GetProjectMigrationJsonAsync(string location, string accountId, string projectId, string? accessToken)
        {
            _logger.LogInformation("Starting GetProjectMigrationJsonAsync for accountId: {AccountId}, projectId: {ProjectId}", accountId, projectId);
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/ProjectAMSAssetMigrations/{projectId}";

            if (!string.IsNullOrEmpty(accessToken))
            {
                requestUrl += $"?accessToken={accessToken}";
            }

            var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            }

            try
            {
                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully retrieved JSON response for accountId: {AccountId}, projectId: {ProjectId}", accountId, projectId);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API call failed. Status code: {StatusCode}, Error message: {ErrorMessage}", response.StatusCode, errorResponse);
                    throw new Exception($"API call failed. Status code: {response.StatusCode}, Error message: {errorResponse}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Network error occurred. Failed to connect to API.");
                throw new HttpRequestException("Network error occurred. Failed to connect to API.", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving project migration information.");
                throw new Exception("An unexpected error occurred while retrieving project migration information.", ex);
            }
        }

        /// <summary>
        /// 取得した JSON 文字列を ProjectMigrationResponse オブジェクトに変換します。
        /// </summary>
        /// <param name="json">JSON 文字列</param>
        /// <returns>プロジェクトのマイグレーション情報</returns>
        public ApiProjectMigrationModel ParseProjectMigrationJson(string json)
        {
            _logger.LogInformation("Starting ParseProjectMigrationJson");
            try
            {
                var result = JsonSerializer.Deserialize<ApiProjectMigrationModel>(json);
                if (result == null)
                {
                    _logger.LogError("Failed to deserialize JSON response.");
                    throw new InvalidOperationException("Failed to deserialize JSON response.");
                }
                _logger.LogInformation("Successfully parsed project migration JSON.");
                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error: {Message}", ex.Message);
                throw new InvalidOperationException("Failed to deserialize JSON response.", ex);
            }
        }

        /// <summary>
        /// プロジェクトのマイグレーション情報を取得します。
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップする項目数</param>
        /// <param name="states">状態フィルター</param>
        /// <returns>プロジェクトのマイグレーション情報のリスト</returns>
        public async Task<ApiProjectsMigrations> GetProjectMigrationsAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null)
        {
            _logger.LogInformation("Starting GetProjectMigrationsAsync for accountId: {AccountId}", accountId);
            try
            {
                var jsonResponse = await GetProjectMigrationsJsonAsync(location, accountId, accessToken, pageSize, skip, states);
                var result = ParseJsonResponse(jsonResponse);
                _logger.LogInformation("Successfully retrieved project migrations for accountId: {AccountId}", accountId);
                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP Request error: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// API からプロジェクトのマイグレーションデータを JSON 文字列として取得します。
        /// </summary>
        /// <param name="location">ロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン</param>
        /// <param name="pageSize">ページサイズ</param>
        /// <param name="skip">スキップする項目数</param>
        /// <param name="states">状態フィルター</param>
        /// <returns>プロジェクトのマイグレーションデータの JSON 文字列</returns>
        public async Task<string> GetProjectMigrationsJsonAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null)
        {
            _logger.LogInformation("Starting GetProjectMigrationsJsonAsync for accountId: {AccountId}", accountId);
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/ProjectAMSAssetMigrations?accessToken={accessToken}";
            if (pageSize.HasValue)
                url += "&pageSize=" + pageSize.Value;
            if (skip.HasValue)
                url += "&skip=" + skip.Value;
            if (states != null && states.Any())
                url += "&states=" + string.Join(",", states);

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            try
            {
                var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully retrieved JSON response for accountId: {AccountId}", accountId);
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogError("API call failed. Status code: {StatusCode}, Error message: {ErrorMessage}", response.StatusCode, errorResponse);
                    throw new Exception($"API call failed. Status code: {response.StatusCode}, Error message: {errorResponse}");
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Network error occurred. Failed to connect to API.");
                throw new HttpRequestException("Network error occurred. Failed to connect to API.", httpEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while retrieving project migrations information.");
                throw new Exception("An unexpected error occurred while retrieving project migrations information.", ex);
            }
        }

        /// <summary>
        /// 取得した JSON 文字列を ApiProjectsMigrations オブジェクトに変換します。
        /// </summary>
        /// <param name="jsonResponse">JSON 文字列</param>
        /// <returns>プロジェクトのマイグレーション情報のリスト</returns>
        public ApiProjectsMigrations ParseJsonResponse(string jsonResponse)
        {
            _logger.LogInformation("Starting ParseJsonResponse");
            try
            {
                var result = JsonSerializer.Deserialize<ApiProjectsMigrations>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (result == null)
                {
                    _logger.LogError("Failed to deserialize JSON response.");
                    throw new InvalidOperationException("Failed to deserialize JSON response.");
                }
                _logger.LogInformation("Successfully parsed project migrations JSON.");
                return result;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error: {Message}", ex.Message);
                throw new InvalidOperationException("Failed to deserialize JSON response.", ex);
            }
        }

        /// <summary>
        /// Video Indexer API で新しいプロジェクトを作成します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectName">作成するプロジェクトの名前。</param>
        /// <param name="videoRanges">プロジェクトに含めるビデオ範囲リスト。各ビデオの ID と時間範囲を含む。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
        /// <returns>作成されたプロジェクトの情報を含む <see cref="ApiProjectModel"/> オブジェクト、それ以外は null を返します。</returns>
        public async Task<ApiProjectModel?> CreateProjectAsync(string location, string accountId, string projectName, List<ApiVideoTimeRangeModel> videoRanges, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await SendPostRequestForCreateProjectAsync(location, accountId, projectName, videoRanges, accessToken);
                return ParseProjectJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while creating project.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing failed while creating project.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating project.");
            }

            return null;
        }

        /// <summary>
        /// Video Indexer API に POST リクエストを送信し、新しいプロジェクトを作成します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectName">作成するプロジェクトの名前。</param>
        /// <param name="videoRanges">プロジェクトに含めるビデオ範囲リスト。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        /// <returns>API からの JSON レスポンスを文字列として返します。</returns>
        private async Task<string> SendPostRequestForCreateProjectAsync(string location, string accountId, string projectName, List<ApiVideoTimeRangeModel> videoRanges, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            var projectData = new
            {
                name = projectName,
                videosRanges = videoRanges,
                isSearchable = (bool?)null
            };

            var content = new StringContent(JsonSerializer.Serialize(projectData), System.Text.Encoding.UTF8, "application/json");
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.PostAsync(url, content);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を <see cref="ApiProjectModel"/> オブジェクトにパースします。
        /// </summary>
        /// <param name="json">パースする JSON 文字列。API から返されたレスポンス。</param>
        /// <returns>パースに成功した場合は <see cref="ApiProjectModel"/> オブジェクト、それ以外は null を返します。</returns>
        private ApiProjectModel? ParseProjectJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiProjectModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Project JSON.");
                return null;
            }
        }

        /// <summary>
        /// Video Indexer API でプロジェクトを削除します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">削除するプロジェクトの ID。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
        /// <returns>削除が成功した場合は true、それ以外は false を返します。</returns>
        public async Task<bool> DeleteProjectAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                await SendDeleteRequestForProjectAsync(location, accountId, projectId, accessToken);
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while deleting project.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting project.");
            }

            return false;
        }

        /// <summary>
        /// Video Indexer API に DELETE リクエストを送信し、プロジェクトを削除します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">削除するプロジェクトの ID。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        private async Task SendDeleteRequestForProjectAsync(string location, string accountId, string projectId, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.DeleteAsync(url);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
        }
    }
}

