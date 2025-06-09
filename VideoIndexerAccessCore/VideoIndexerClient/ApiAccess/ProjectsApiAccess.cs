using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class ProjectsApiAccess : IProjectsApiAccess
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
        /// Cancel Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
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
        /// Cancel Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
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
            var response = await httpClient.PostAsync(url, null) ?? throw new HttpRequestException("The response was null."); response.EnsureSuccessStatusCode();
            // responseがnullなら例外を
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を ApiProjectRenderOperationModel オブジェクトにパースします。
        /// Cancel Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Cancel-Project-Render-Operation
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
        /// Create Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
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
        /// Create Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
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
            var response = await httpClient.PostAsync(url, content) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を <see cref="ApiProjectModel"/> オブジェクトにパースします。
        /// Create Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Project
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
        /// Delete Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Project
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
        /// Delete Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Project
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
            var response = await httpClient.DeleteAsync(url) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Video Indexer API からレンダリングされたプロジェクトのダウンロード URL を取得します。
        /// Download Project Rendered File Url
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
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
        /// Download Project Rendered File Url
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
        /// </summary>
        public async Task<string> SendGetRequestForRenderedFileUrlAsync(string location, string accountId, string projectId, string? accessToken)
        {
            string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/renderedfile/downloadurl";
            var queryParams = new List<string>();
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={Uri.EscapeDataString(accessToken)}");

            if (queryParams.Count > 0)
                url += "?" + string.Join("&", queryParams);

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列を解析し、ダウンロード URL を取得します。
        /// Download Project Rendered File Url
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Download-Project-Rendered-File-Url
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
        /// Get Project Captions
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Captions
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
        /// Get Project Captions
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Captions
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
            var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// 指定されたプロジェクトのインデックスを取得する。
        /// Get Project Index
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
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
        /// Get Project Index
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
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
                var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
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
        /// Get Project Index
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Index
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

        /// <summary>
        /// 指定されたプロジェクトのインサイトウィジェットのURLを取得する。
        /// Get Project Insights Widget
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Insights-Widget
        /// </summary>
        /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
        /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
        /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
        /// <param name="widgetType">(オプション) 取得するウィジェットの種類（People, Sentiments, Keywords, Search）。</param>
        /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
        /// <returns>
        /// インサイトウィジェットのURLを表す文字列。
        /// response.Headers.Locationか、response.Contentか不明、要精査 todo
        /// </returns>
        public async Task<string> GetProjectInsightsWidgetAsync(string location, string accountId, string projectId, string? widgetType = null, string? accessToken = null)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/InsightsWidget";
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(widgetType)) queryParams.Add($"widgetType={widgetType}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response.Content : {result}", result);
                var headersLocation = response.Headers.Location?.ToString();
                _logger.LogInformation("response.Headers.Location : {headersLocation}", headersLocation);
                return headersLocation ?? throw new Exception("No redirect URL found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error");
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトのプレイヤーウィジェットのURLを取得する。
        /// Get Project Player Widget
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Player-Widget
        /// </summary>
        /// <param name="location">APIのリクエストを送るAzureリージョン。</param>
        /// <param name="accountId">プロジェクトが属するアカウントのGUID。</param>
        /// <param name="projectId">取得するプロジェクトの一意の識別子。</param>
        /// <param name="accessToken">(オプション) APIアクセス用のトークン。</param>
        /// <returns>
        /// プレイヤーウィジェットのURLを表す文字列。
        /// response.Headers.Locationか、response.Contentか不明、要精査 todo
        /// </returns>
        public async Task<string> GetProjectPlayerWidgetAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/PlayerWidget";
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("response.Content : {result}", result);
                var headersLocation = response.Headers.Location?.ToString();
                _logger.LogInformation("response.Headers.Location : {headersLocation}", headersLocation);
                return headersLocation ?? throw new Exception("No redirect URL found");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error");
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトのレンダー操作のステータスを取得する。
        /// Get Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">対象のアカウントID (GUID)</param>
        /// <param name="projectId">対象のプロジェクトID</param>
        /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
        /// <returns>レンダー操作の状態を含むモデル</returns>
        public async Task<ApiProjectRenderOperationModel> GetProjectRenderOperationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await FetchProjectRenderOperationJsonAsync(location, accountId, projectId, accessToken);
                return ParseProjectRenderOperationJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project render operation");
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトのレンダー操作のJSONデータを取得する。
        /// Get Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">対象のアカウントID (GUID)</param>
        /// <param name="projectId">対象のプロジェクトID</param>
        /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
        /// <returns>APIから取得したJSON文字列 (レンダー操作のステータスを含む)</returns>
        /// <exception cref="HttpRequestException">HTTPリクエストエラーが発生した場合</exception>
        public async Task<string> FetchProjectRenderOperationJsonAsync(string location, string accountId, string projectId, string? accessToken)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/renderoperation";
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode(); response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error");
                throw;
            }
        }

        /// <summary>
        /// JSONデータを ApiProjectRenderOperationModel にパースする。
        /// Get Project Render Operation
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Render-Operation
        /// </summary>
        /// <param name="json">JSON 文字列</param>
        /// <returns>パースされた ApiProjectRenderOperationModel オブジェクト</returns>
        public ApiProjectRenderOperationModel ParseProjectRenderOperationJson(string json)
        {
            return JsonSerializer.Deserialize<ApiProjectRenderOperationModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new JsonException("Failed to parse JSON");
        }

        /// <summary>
        /// 指定されたプロジェクトのサムネイルのURLを取得する。
        /// Get Project Thumbnail
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Thumbnail
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">対象のアカウントID (GUID)</param>
        /// <param name="projectId">対象のプロジェクトID</param>
        /// <param name="thumbnailId">取得するサムネイルのID (GUID)</param>
        /// <param name="format">オプション: サムネイルのフォーマット (Jpeg / Base64)</param>
        /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
        /// <returns>サムネイルのURL</returns>
        public async Task<string> GetProjectThumbnailUrlAsync(string location, string accountId, string projectId, string thumbnailId, string? format = null, string? accessToken = null)
        {
            try
            {
                return await FetchProjectThumbnailUrlAsync(location, accountId, projectId, thumbnailId, format, accessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project thumbnail URL");
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトのサムネイルのURLを取得する。
        /// Get Project Thumbnail
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Thumbnail
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">対象のアカウントID (GUID)</param>
        /// <param name="projectId">対象のプロジェクトID</param>
        /// <param name="thumbnailId">取得するサムネイルのID (GUID)</param>
        /// <param name="format">オプション: サムネイルのフォーマット (Jpeg / Base64)</param>
        /// <param name="accessToken">オプション: アクセストークン (省略可)</param>
        /// <returns>サムネイルのURLを表す文字列</returns>
        /// <exception cref="HttpRequestException">HTTPリクエストエラーが発生した場合</exception>
        public async Task<string> FetchProjectThumbnailUrlAsync(string location, string accountId, string projectId, string thumbnailId, string? format, string? accessToken)
        {
            try
            {
                // APIエンドポイントのURLを構築
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/Thumbnails/{thumbnailId}";
                var queryParams = new List<string>();

                // サムネイルのフォーマットが指定されている場合は、クエリパラメータに追加
                if (!string.IsNullOrEmpty(format)) queryParams.Add($"format={format}");

                // アクセストークンが指定されている場合は、クエリパラメータに追加
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                // クエリパラメータをURLに追加
                if (queryParams.Count > 0)
                {
                    url += "?" + string.Join("&", queryParams);
                }

                // APIへHTTP GETリクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode(); // HTTPレスポンスの成功ステータスを確認
                return await response.Content.ReadAsStringAsync(); // サムネイルのURLを取得
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while fetching thumbnail URL");
                throw;
            }
        }

        /// <summary>
        /// 指定されたアカウントのプロジェクト一覧を取得する。
        /// List Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects
        /// </summary>
        /// <param name="location">Azureのリージョン。</param>
        /// <param name="accountId">アカウントのID（GUID形式）。</param>
        /// <param name="createdAfter">指定された日付以降に作成されたプロジェクトをフィルタリング。</param>
        /// <param name="createdBefore">指定された日付以前に作成されたプロジェクトをフィルタリング。</param>
        /// <param name="pageSize">取得するページサイズ。</param>
        /// <param name="skip">スキップするレコード数。</param>
        /// <param name="accessToken">認証用のアクセストークン。</param>
        /// <returns>プロジェクト情報を含むApiProjectSearchResultModel。</returns>
        public async Task<ApiProjectSearchResultModel> GetProjectsAsync(string location, string accountId, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await FetchProjectsJsonAsync(location, accountId, createdAfter, createdBefore, pageSize, skip, accessToken);
                return MapToApiProjectSearchResultModel(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching project list");
                throw;
            }
        }

        /// <summary>
        /// 指定されたアカウントのプロジェクト一覧をJSON形式で取得する。
        /// List Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects        /// </summary>
        /// <param name="location">Azureのリージョン。</param>
        /// <param name="accountId">アカウントのID（GUID形式）。</param>
        /// <param name="createdAfter">指定された日付以降に作成されたプロジェクトをフィルタリング。</param>
        /// <param name="createdBefore">指定された日付以前に作成されたプロジェクトをフィルタリング。</param>
        /// <param name="pageSize">取得するページサイズ。</param>
        /// <param name="skip">スキップするレコード数。</param>
        /// <param name="accessToken">認証用のアクセストークン。</param>
        /// <returns>APIレスポンスのJSON文字列。</returns>
        public async Task<string> FetchProjectsJsonAsync(string location, string accountId, string? createdAfter, string? createdBefore, int? pageSize, int? skip, string? accessToken)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects";
                var queryParams = new List<string>();

                if (!string.IsNullOrEmpty(createdAfter)) queryParams.Add($"createdAfter={createdAfter}");
                if (!string.IsNullOrEmpty(createdBefore)) queryParams.Add($"createdBefore={createdBefore}");
                if (pageSize.HasValue) queryParams.Add($"pageSize={pageSize}");
                if (skip.HasValue) queryParams.Add($"skip={skip}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");
                if (queryParams.Count > 0) url += "?" + string.Join("&", queryParams);

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while fetching project list");
                throw;
            }
        }
        /// <summary>
        /// JSONレスポンスを `ApiProjectSearchResultModel` に変換する。
        /// List Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=List-Projects  /// </summary>
        /// <param name="jsonResponse">APIから取得したJSONレスポンス。</param>
        /// <returns>マッピングされた `ApiProjectSearchResultModel` オブジェクト。</returns>
        public ApiProjectSearchResultModel MapToApiProjectSearchResultModel(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiProjectSearchResultModel>(jsonResponse, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }) ?? new ApiProjectSearchResultModel();
        }

        // Render Project

        /// <summary>
        /// プロジェクトのレンダリングを開始し、結果を取得する非同期メソッド
        /// Render Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">アカウントID (GUID)</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">オプションのアクセストークン (Bearer トークンとして利用可能)</param>
        /// <param name="sendCompletionEmail">レンダリング完了時にメール通知を送信するかのフラグ</param>
        /// <returns>レンダリング結果のオブジェクトを返す</returns>
        public async Task<ApiProjectRenderResponseModel> RenderProjectAsync(string location, string accountId, string projectId, string? accessToken = null, bool sendCompletionEmail = false)
        {
            try
            {
                string jsonResponse = await FetchProjectRenderJsonAsync(location, accountId, projectId, accessToken, sendCompletionEmail);
                return ParseProjectRenderJson(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while rendering project.");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while rendering project.");
                throw;
            }
        }

        /// <summary>
        /// 指定されたパラメータを基にURLを構築し、APIにリクエストを送信し、JSONレスポンスを取得する非同期メソッド
        /// Render Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">アカウントID (GUID)</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">オプションのアクセストークン (Bearer トークンとして利用可能)</param>
        /// <param name="sendCompletionEmail">レンダリング完了時にメール通知を送信するかのフラグ</param>
        /// <returns>APIから取得したJSONレスポンス</returns>
        public async Task<string> FetchProjectRenderJsonAsync(string location, string accountId, string projectId, string? accessToken, bool sendCompletionEmail)
        {
            var uriBuilder = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Projects/{projectId}/render");

            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            query["sendCompletionEmail"] = sendCompletionEmail.ToString().ToLower();
            if (!string.IsNullOrEmpty(accessToken))
            {
                query["accessToken"] = accessToken;
            }
            uriBuilder.Query = query.ToString();

            using var request = new HttpRequestMessage(HttpMethod.Post, uriBuilder.ToString());
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSONレスポンスを解析し、レンダリング結果のオブジェクトに変換するメソッド
        /// Render Project
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Render-Project
        /// </summary>
        /// <param name="jsonResponse">APIから取得したJSONレスポンス</param>
        /// <returns>パース済みのレンダリング結果オブジェクト</returns>
        public ApiProjectRenderResponseModel ParseProjectRenderJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiProjectRenderResponseModel>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new JsonException("Failed to parse JSON response.");
        }


        // Search Projects

        /// <summary>
        /// 指定された検索条件でプロジェクトを検索する非同期メソッド
        /// Search Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
        /// </summary>
        /// <param name="location">Azureリージョン</param>
        /// <param name="accountId">アカウントのGUID</param>
        /// <param name="query">(オプション) フリーテキスト検索クエリ</param>
        /// <param name="sourceLanguage">(オプション) ソース言語</param>
        /// <param name="pageSize">(オプション) 取得するプロジェクトの最大件数</param>
        /// <param name="skip">(オプション) スキップするプロジェクトの件数 (ページネーション用)</param>
        /// <param name="accessToken">(オプション) アクセストークン</param>
        /// <returns>検索結果のレスポンスモデル</returns>
        public async Task<ApiProjectSearchResultModel> SearchProjectsAsync(string location, string accountId, string? query = null, string? sourceLanguage = null, int? pageSize = null, int? skip = null, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await FetchProjectSearchJsonAsync(location, accountId, query, sourceLanguage, pageSize, skip, accessToken);
                return ParseApiProjectSearchResultModelJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while searching projects.");
                throw;
            }
        }

        /// <summary>
        /// 指定された検索条件でプロジェクトを検索する非同期メソッド
        /// Search Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
        /// </summary>
        /// <param name="location">Azureリージョン</param>
        /// <param name="accountId">アカウントのGUID</param>
        /// <param name="query">(オプション) フリーテキスト検索クエリ</param>
        /// <param name="sourceLanguage">(オプション) ソース言語</param>
        /// <param name="pageSize">(オプション) 取得するプロジェクトの最大件数</param>
        /// <param name="skip">(オプション) スキップするプロジェクトの件数 (ページネーション用)</param>
        /// <param name="accessToken">(オプション) アクセストークン</param>
        /// <returns>検索結果のレスポンスモデル</returns>
        public async Task<string> FetchProjectSearchJsonAsync(string location, string accountId, string? query, string? sourceLanguage, int? pageSize, int? skip, string? accessToken)
        {
            var uriBuilder = new UriBuilder(string.Format(_apiResourceConfigurations.ApiEndpoint, location, accountId));
            var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(query)) queryParams["query"] = query;
            if (!string.IsNullOrEmpty(sourceLanguage)) queryParams["sourceLanguage"] = sourceLanguage;
            if (pageSize.HasValue) queryParams["pageSize"] = pageSize.Value.ToString();
            if (skip.HasValue) queryParams["skip"] = skip.Value.ToString();
            if (!string.IsNullOrEmpty(accessToken)) queryParams["accessToken"] = accessToken;
            uriBuilder.Query = queryParams.ToString();

            return await SendApiRequestAsync(HttpMethod.Get, uriBuilder.ToString());
        }

        /// <summary>
        /// APIリクエストを送信し、JSONレスポンスを取得する共通メソッド
        /// Search Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
        /// </summary>
        /// <param name="method">HTTPメソッド (GET, POSTなど)</param>
        /// <param name="url">リクエストを送信するURL</param>
        /// <returns>APIからのJSONレスポンス</returns>
        public async Task<string> SendApiRequestAsync(HttpMethod method, string url)
        {
            using var request = new HttpRequestMessage(method, url);
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSONレスポンスを指定した型にパースするメソッド
        /// Search Projects
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Projects
        /// </summary>
        /// <typeparam name="T">パースするオブジェクトの型</typeparam>
        /// <param name="jsonResponse">APIからのJSONレスポンス</param>
        /// <returns>パースされたオブジェクト</returns>
        public ApiProjectSearchResultModel ParseApiProjectSearchResultModelJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiProjectSearchResultModel>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new JsonException("Failed to parse JSON response.");
        }

        // Update Project

        /// <summary>
        /// 指定されたプロジェクトの情報を更新する非同期メソッド。
        /// </summary>
        /// <param name="location">Azureリージョン</param>
        /// <param name="accountId">アカウントのGUID</param>
        /// <param name="projectId">更新するプロジェクトのID</param>
        /// <param name="updateRequest">更新するプロジェクトのデータ (名前とビデオ範囲)</param>
        /// <param name="accessToken">(オプション) アクセストークン</param>
        /// <returns>更新されたプロジェクトのレスポンスモデル</returns>
        public async Task<ApiProjectUpdateResponseModel> UpdateProjectAsync(string location, string accountId, string projectId, ApiProjectUpdateRequestModel updateRequest, string? accessToken = null)
        {
            try
            {
                string jsonResponse = await FetchProjectUpdateJsonAsync(location, accountId, projectId, updateRequest, accessToken);
                return ParseProjectUpdateResponseJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating project.");
                throw;
            }
        }

        /// <summary>
        /// 指定されたプロジェクトの更新リクエストをAPIに送信し、レスポンスを取得する。
        /// </summary>
        /// <param name="location">Azureリージョンの識別子</param>
        /// <param name="accountId">更新対象のアカウントID</param>
        /// <param name="projectId">更新するプロジェクトのID</param>
        /// <param name="updateRequest">プロジェクトの更新内容</param>
        /// <param name="accessToken">(オプション) アクセストークン</param>
        /// <returns>APIのJSONレスポンス</returns>        
        public async Task<string> FetchProjectUpdateJsonAsync(string location, string accountId, string projectId, ApiProjectUpdateRequestModel updateRequest, string? accessToken)
        {
            var uriBuilder = new UriBuilder(string.Format(_apiResourceConfigurations.ApiEndpoint, location, accountId, projectId));
            var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(accessToken)) queryParams["accessToken"] = accessToken;
            uriBuilder.Query = queryParams.ToString();

            var jsonContent = JsonSerializer.Serialize(updateRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            return await SendApiRequestAsync(HttpMethod.Put, uriBuilder.ToString(), content);
        }

        /// <summary>
        /// APIリクエストを送信し、JSONレスポンスを取得する共通メソッド。
        /// </summary>
        /// <param name="method">HTTPメソッド (GET, POST, PUT など)</param>
        /// <param name="url">APIのリクエストURL</param>
        /// <param name="content">(オプション) HTTPリクエストのボディコンテンツ</param>
        /// <returns>APIからのJSONレスポンス</returns>
        public async Task<string> SendApiRequestAsync(HttpMethod method, string url, HttpContent? content = null)
        {
            using var request = new HttpRequestMessage(method, url) { Content = content };
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSONレスポンスを指定した型にパースするメソッド。
        /// </summary>
        /// <param name="jsonResponse">APIからのJSONレスポンス</param>
        /// <returns>パースされたオブジェクト</returns>
        public ApiProjectUpdateResponseModel ParseProjectUpdateResponseJson(string jsonResponse)
        {
            return JsonSerializer.Deserialize<ApiProjectUpdateResponseModel>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                   ?? throw new JsonException("Failed to parse JSON response.");
        }
    }
}
