﻿using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class ProjectsApiAccess
    {
        private readonly ILogger<ProjectsApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public ProjectsApiAccess(ILogger<ProjectsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// Video Indexer API でプロジェクトのレンダー操作をキャンセルします。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">プロジェクトの一意の識別子。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        /// <returns>キャンセルが成功した場合は ApiProjectRenderOperationModel オブジェクト、それ以外は null を返します。</returns>
        public async Task<ApiProjectRenderOperationModel?> CancelProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await SendPostRequestForCancelRenderAsync(location, accountId, projectId, accessToken);
                return ParseRenderOperationJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while cancelling project render operation.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing failed while cancelling project render operation.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while cancelling project render operation.");
            }

            return null;
        }

        /// <summary>
        /// Video Indexer API に POST リクエストを送信し、レンダー操作をキャンセルします。
        /// </summary>
        /// <returns>JSON レスポンスを文字列として返します。</returns>
        public async Task<string> SendPostRequestForCancelRenderAsync(string location, string accountId, string projectId, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/renderoperation/cancel";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.PostAsync(url, null);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null."); response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を ApiProjectRenderOperationModel オブジェクトにパースします。
        /// </summary>
        /// <param name="json">パースする JSON 文字列。</param>
        /// <returns>パースに成功した場合は ApiProjectRenderOperationModel オブジェクト、それ以外は null を返します。</returns>
        public ApiProjectRenderOperationModel? ParseRenderOperationJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiProjectRenderOperationModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Project Render Operation JSON.");
                return null;
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
        public async Task<string> SendPostRequestForCreateProjectAsync(string location, string accountId, string projectName, List<ApiVideoTimeRangeModel> videoRanges, string? accessToken)
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
        public ApiProjectModel? ParseProjectJson(string json)
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
        public async Task SendDeleteRequestForProjectAsync(string location, string accountId, string projectId, string? accessToken)
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

        /// <summary>
        /// Video Indexer API からレンダリングされたプロジェクトのダウンロード URL を取得します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">ダウンロード URL を取得するプロジェクトの ID。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。API へのアクセス権限を付与する。</param>
        /// <returns>取得したダウンロード URL を含む文字列、それ以外は null を返します。</returns>
        public async Task<string?> GetProjectRenderedFileDownloadUrlAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await SendGetRequestForRenderedFileUrlAsync(location, accountId, projectId, accessToken);
                return ParseDownloadUrlJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while retrieving project rendered file download URL.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing failed while retrieving project rendered file download URL.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving project rendered file download URL.");
            }

            return null;
        }

        /// <summary>
        /// Video Indexer API に GET リクエストを送信し、レンダリングされたプロジェクトのダウンロード URL を取得します。
        /// </summary>
        public async Task<string> SendGetRequestForRenderedFileUrlAsync(string location, string accountId, string projectId, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/renderedfile/downloadurl";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(url);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を解析し、ダウンロード URL を取得します。
        /// </summary>
        /// <param name="json">パースする JSON 文字列。</param>
        /// <returns>ダウンロード URL を含む文字列、それ以外は null を返します。</returns>
        public string? ParseDownloadUrlJson(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("downloadUrl").GetString();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse Download URL JSON.");
                return null;
            }
        }


        /// <summary>
        /// Video Indexer API を使用してプロジェクトのキャプションを取得します。
        /// 指定されたパラメータに基づき、特定の形式や言語でキャプションを取得できます。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">キャプションを取得するプロジェクトの ID。</param>
        /// <param name="indexId">オプションの動画 ID。</param>
        /// <param name="format">キャプションのフォーマット (Vtt / Ttml / Srt / Txt / Csv)。</param>
        /// <param name="language">キャプションの言語。</param>
        /// <param name="includeAudioEffects">オーディオエフェクトを含めるかどうか。</param>
        /// <param name="includeSpeakers">スピーカー情報を含めるかどうか。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        /// <returns>
        /// キャプションデータの文字列。指定されたフォーマットとオプションに従って取得されます。
        /// エラーが発生した場合は null を返します。
        /// </returns>
        public async Task<string?> GetProjectCaptionsAsync(string location, string accountId, string projectId, string? indexId = null, string? format = null, string? language = null, bool? includeAudioEffects = null, bool? includeSpeakers = null, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await SendGetRequestForProjectCaptionsAsync(location, accountId, projectId, indexId, format, language, includeAudioEffects, includeSpeakers, accessToken);
                return jsonResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while retrieving project captions.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving project captions.");
            }

            return null;
        }

        /// <summary>
        /// Video Indexer API に GET リクエストを送信し、プロジェクトのキャプションを取得します。
        /// 指定されたパラメータを基に API へリクエストを行い、キャプションデータを取得します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="projectId">キャプションを取得するプロジェクトの ID。</param>
        /// <param name="indexId">オプションの動画 ID。</param>
        /// <param name="format">キャプションのフォーマット (Vtt / Ttml / Srt / Txt / Csv)。</param>
        /// <param name="language">キャプションの言語。</param>
        /// <param name="includeAudioEffects">オーディオエフェクトを含めるかどうか。</param>
        /// <param name="includeSpeakers">スピーカー情報を含めるかどうか。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        /// <returns>
        /// API からのレスポンスを文字列として返します。
        /// 成功した場合はキャプションデータ、失敗した場合は例外がスローされます。
        /// </returns>
        public async Task<string> SendGetRequestForProjectCaptionsAsync(string location, string accountId, string projectId, string? indexId, string? format, string? language, bool? includeAudioEffects, bool? includeSpeakers, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/Captions";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(indexId)) queryParams.Add($"indexId={Uri.EscapeDataString(indexId)}");
            if (!string.IsNullOrEmpty(format)) queryParams.Add($"format={Uri.EscapeDataString(format)}");
            if (!string.IsNullOrEmpty(language)) queryParams.Add($"language={Uri.EscapeDataString(language)}");
            if (includeAudioEffects.HasValue) queryParams.Add($"includeAudioEffects={includeAudioEffects.Value.ToString().ToLower()}");
            if (includeSpeakers.HasValue) queryParams.Add($"includeSpeakers={includeSpeakers.Value.ToString().ToLower()}");
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(url);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 指定されたプロジェクトのインデックスを取得する。
        /// </summary>
        /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
        /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
        /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
        /// <param name="language">(オプション) 動画インサイトを翻訳する言語コード。</param>
        /// <param name="reTranslate">(オプション) 既存の翻訳を上書きするかどうかのフラグ。</param>
        /// <param name="includedInsights">(オプション) 含めるインサイトの種類（カンマ区切り）。</param>
        /// <param name="excludedInsights">(オプション) 除外するインサイトの種類（カンマ区切り）。</param>
        /// <param name="includeSummarizedInsights">(オプション) SummarizedInsights を含めるかどうかのフラグ。</param>
        /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
        /// <returns>取得したプロジェクトのインデックス情報を表す <see cref="ApiProjectIndexModel"/> オブジェクト。</returns>
        public async Task<ApiProjectIndexModel> GetProjectIndexAsync(string location, string accountId, string projectId, string? language = null, bool? reTranslate = null, string? includedInsights = null, string? excludedInsights = null, bool? includeSummarizedInsights = null, string? accessToken = null)
        {
            try
            {
                // API から JSON レスポンスを取得
                string jsonResponse = await FetchProjectIndexJsonAsync(location, accountId, projectId, language, reTranslate, includedInsights, excludedInsights, includeSummarizedInsights, accessToken);

                // JSON をオブジェクトに変換して返す
                return ParseProjectIndexJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project index");
                throw;
            }
        }

        /// <summary>
        /// API へリクエストを送信し、JSON レスポンスを取得する。
        /// </summary>
        /// <returns>API のレスポンス（JSON 形式）を文字列として返す。</returns>
        public async Task<string> FetchProjectIndexJsonAsync(string location, string accountId, string projectId, string? language, bool? reTranslate, string? includedInsights, string? excludedInsights, bool? includeSummarizedInsights, string? accessToken)
        {
            try
            {
                // API の URL を構成
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/Index";
                var queryParams = new List<string>();

                // オプションのクエリパラメータを追加
                if (!string.IsNullOrEmpty(language)) queryParams.Add($"language={language}");
                if (reTranslate.HasValue) queryParams.Add($"reTranslate={reTranslate.Value.ToString().ToLower()}");
                if (!string.IsNullOrEmpty(includedInsights)) queryParams.Add($"includedInsights={includedInsights}");
                if (!string.IsNullOrEmpty(excludedInsights)) queryParams.Add($"excludedInsights={excludedInsights}");
                if (includeSummarizedInsights.HasValue) queryParams.Add($"includeSummarizedInsights={includeSummarizedInsights.Value.ToString().ToLower()}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                // HTTP リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error");
                throw;
            }
        }

        /// <summary>
        /// JSON レスポンスをパースし、オブジェクトに変換する。
        /// </summary>
        /// <param name="json">API から取得した JSON データ。</param>
        /// <returns>パースされた <see cref="ApiProjectIndexModel"/> オブジェクト。</returns>
        public ApiProjectIndexModel ParseProjectIndexJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiProjectIndexModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                       ?? throw new JsonException("Failed to parse JSON");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON deserialization error");
                throw;
            }
        }


    }
}
