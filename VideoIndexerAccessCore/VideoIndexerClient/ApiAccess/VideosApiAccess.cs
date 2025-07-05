using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class VideosApiAccess : IVideosApiAccess
    {
        private readonly ILogger<VideosApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // Delete Video

        public VideosApiAccess(ILogger<VideosApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// 指定された動画を Video Indexer から削除します。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
        /// <param name="accountId">Video Indexer アカウントの一意な GUID 文字列</param>
        /// <param name="videoId">削除対象のビデオの ID</param>
        /// <param name="accessToken">アクセストークン（省略可能。指定しない場合は認証されないリクエストとなります）</param>
        /// <returns>
        /// 削除結果を示す <see cref="DeleteVideoResult"/> オブジェクト。
        /// 削除に成功した場合は null（NoContent）、一部失敗した場合は失敗アセット情報を含むオブジェクトを返します。
        /// </returns>
        public async Task<ApiDeleteVideoResultModel?> DeleteVideoAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            var json = await FetchDeleteVideoJsonAsync(location, accountId, videoId, accessToken);
            return ParseDeleteVideoJson(json);
        }

        /// <summary>
        /// Video Indexer API に DELETE リクエストを送信し、JSONレスポンスを取得します。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="location">Azure リージョン名</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">削除する動画の ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>
        /// API のレスポンス本文（JSON文字列）。
        /// 削除に成功し NoContent の場合は null を返します。
        /// </returns>
        public async Task<string?> FetchDeleteVideoJsonAsync(string location, string accountId, string videoId, string? accessToken)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}";

                if (!string.IsNullOrWhiteSpace(accessToken))
                    url += $"?accessToken={Uri.EscapeDataString(accessToken)}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);

                // 任意のGUIDでリクエストIDを送信（省略可能）
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("Video deleted successfully.");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Video deletion response: {Json}", json);
                return json;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred.");
                return null;
            }
        }

        /// <summary>
        /// JSON文字列を ApiDeleteVideoResultModel オブジェクトにパースします。
        /// Delete Video
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video
        /// </summary>
        /// <param name="json">API から返された JSON 文字列</param>
        /// <returns>
        /// JSON をデシリアライズした <see cref="ApiDeleteVideoResultModel"/> オブジェクト。
        /// JSON が null またはパースできない場合は null を返します。
        /// </returns>
        public ApiDeleteVideoResultModel? ParseDeleteVideoJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<ApiDeleteVideoResultModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error.");
                return null;
            }
        }

        // Delete Video Source File

        /// <summary>
        /// 指定された動画のソースファイルとストリーミングアセットを削除します（インサイトは保持）。
        /// Delete Video Source File
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video-Source-File
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
        /// <param name="accountId">Video Indexer アカウントの一意な GUID 文字列</param>
        /// <param name="videoId">ソースファイルを削除するビデオの ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>
        /// 削除に成功した場合は true を返し、失敗や例外発生時には false を返します。
        /// </returns>
        public async Task<bool> DeleteVideoSourceFileAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/SourceFile";

                if (!string.IsNullOrWhiteSpace(accessToken))
                    url += $"?accessToken={Uri.EscapeDataString(accessToken)}";

                var request = new HttpRequestMessage(HttpMethod.Delete, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    _logger.LogInformation("Video source file deleted successfully.");
                    return true;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Video source file deletion response: {Json}", json);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while deleting source file.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting source file.");
                return false;
            }
        }

        // Get Video Artifact Download Url

        /// <summary>
        /// 指定された動画の指定された種類のアーティファクトのダウンロード URL を取得します。
        /// Get Video Artifact Download Url
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Artifact-Download-Url
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象のビデオ ID</param>
        /// <param name="artifactType">アーティファクトの種類（例: Transcript, Faces, Labels など）</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>ダウンロード可能な一時的な URL（文字列）</returns>
        public async Task<string?> GetArtifactDownloadUrlAsync(string location, string accountId, string videoId, string? artifactType = null, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/ArtifactUrl";

                var hasQuery = false;
                if (!string.IsNullOrWhiteSpace(artifactType))
                {
                    url += $"?type={Uri.EscapeDataString(artifactType)}";
                    hasQuery = true;
                }

                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    url += hasQuery ? "&" : "?";
                    url += $"accessToken={Uri.EscapeDataString(accessToken)}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (!response.IsSuccessStatusCode)
                {
                    var errorJson = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve artifact URL: {Error}", errorJson);
                    return null;
                }

                var result = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Artifact URL retrieved: {Url}", result);
                return result.Trim('"');
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving artifact URL.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving artifact URL.");
                return null;
            }
        }

        // Get Video Captions

        /// <summary>
        /// 指定された動画に対して字幕（キャプション）を取得します。
        /// Get Video Captions
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Captions
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
        /// <param name="videoId">対象となるビデオ ID</param>
        /// <param name="indexId">インデックス ID（オプション）</param>
        /// <param name="format">字幕フォーマット（例: Vtt, Srt, Txt, Csv など）</param>
        /// <param name="language">字幕の言語（例: ja-JP, en-US など）</param>
        /// <param name="includeAudioEffects">音声効果を含めるか（true/false）</param>
        /// <param name="includeSpeakers">話者情報を含めるか（true/false）</param>
        /// <param name="accessToken">アクセストークン（省略可能だが、必要な場合あり）</param>
        /// <returns>取得された字幕データ（文字列）を返します。失敗時は null。</returns>
        public async Task<string?> GetVideoCaptionsAsync(string location, string accountId, string videoId, string? indexId = null, string? format = null, string? language = null, bool? includeAudioEffects = null, bool? includeSpeakers = null, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Captions";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrWhiteSpace(indexId)) query["indexId"] = indexId;
                if (!string.IsNullOrWhiteSpace(format)) query["format"] = format;
                if (!string.IsNullOrWhiteSpace(language)) query["language"] = language;
                if (includeAudioEffects.HasValue) query["includeAudioEffects"] = includeAudioEffects.Value.ToString().ToLower();
                if (includeSpeakers.HasValue) query["includeSpeakers"] = includeSpeakers.Value.ToString().ToLower();
                if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve captions: {Error}", error);
                    return null;
                }

                var captions = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Captions retrieved successfully.");
                return captions;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving captions.");
                throw;
                //return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving captions.");
                throw;
                //return null;
            }
        }

        // Get Video Frames File Paths

        /// <summary>
        /// 指定された動画から抽出されたフレーム画像の SAS URL 一覧を取得します。
        /// Get Video Frames File Paths
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Frames-File-Paths
        /// </summary>
        /// <param name="location">Azure リージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
        /// <param name="videoId">対象のビデオ ID</param>
        /// <param name="urlsLifetimeSeconds">URL の有効期限（秒単位）</param>
        /// <param name="pageSize">1ページあたりの取得件数（省略可能）</param>
        /// <param name="skip">スキップするフレーム数（省略可能）</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>JSON 文字列として返される SAS URL 一覧（または null）</returns>
        public async Task<string?> GetVideoFramesFilePathsAsync(string location, string accountId, string videoId, int? urlsLifetimeSeconds = null, int? pageSize = null, int? skip = null, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/FramesFilePaths";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (urlsLifetimeSeconds.HasValue) query["urlsLifetimeSeconds"] = urlsLifetimeSeconds.Value.ToString();
                if (pageSize.HasValue) query["pageSize"] = pageSize.Value.ToString();
                if (skip.HasValue) query["skip"] = skip.Value.ToString();
                if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve frame file paths: {Error}", error);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Frame file paths retrieved successfully.");
                return json;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving frame file paths.");
                throw;
                //return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving frame file paths.");
                throw;
                //return null;
            }
        }

        // Get Video Id By External Id

        /// <summary>
        /// 外部 ID を使って、対応する Video ID を取得します。
        /// Get Video Id By External Id
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Id-By-External-Id
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer のアカウント ID（GUID）</param>
        /// <param name="externalId">検索対象の外部 ID（ExternalId）</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>対応する Video ID（string）を返します。見つからない場合やエラー時は null。</returns>
        public async Task<string?> GetVideoIdByExternalIdAsync(string location, string accountId, string externalId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/GetIdByExternalId";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrWhiteSpace(externalId))
                    query["externalId"] = externalId;

                if (!string.IsNullOrWhiteSpace(accessToken))
                    query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve videoId by externalId: {Error}", error);
                    return null;
                }

                var videoId = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Video ID retrieved successfully by externalId.");
                return videoId.Trim('"'); // 念のため JSON の "文字列" を外す
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving video ID by externalId.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving video ID by externalId.");
                return null;
            }
        }

        // Get Video Source File Download Url

        /// <summary>
        /// 指定された動画のソースファイル（元動画）のダウンロード用一時 URL を取得します。
        /// Get Video Source File Download Url
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Source-File-Download-Url
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
        /// <param name="videoId">対象のビデオ ID</param>
        /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
        /// <returns>ソースファイルのダウンロード URL（SAS 付きの一時 URL）。取得できなければ null。</returns>
        public async Task<string?> GetVideoSourceFileDownloadUrlAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/SourceFile/DownloadUrl";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrWhiteSpace(accessToken))
                    query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve video source file URL: {Error}", error);
                    return null;
                }

                var downloadUrl = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Video source file download URL retrieved successfully.");
                return downloadUrl.Trim('"');
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving source file URL.");
                throw;
                //return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving source file URL.");
                throw;
                //return null;
            }
        }

        // Get Video Streaming URL

        /// <summary>
        /// 指定された動画のストリーミング再生用 URL を取得します。
        /// Get Video Streaming URL
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Streaming-URL
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
        /// <param name="videoId">対象のビデオ ID</param>
        /// <param name="useProxy">Safari 対応などのためにプロキシを使用するかどうか（省略可能）</param>
        /// <param name="urlFormat">ストリーミングフォーマット（例: HLS_V4, HLS_V6, MPEG_DASH など）</param>
        /// <param name="tokenLifetimeInMinutes">トークンの有効期限（分）※省略時は既定値（60分）</param>
        /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
        /// <returns>ストリーミング URL と JWT トークンを含む ApiStreamingUrlModel オブジェクト。取得できなければ null。</returns>
        public async Task<ApiStreamingUrlModel?> GetVideoStreamingUrlAsync(string location, string accountId, string videoId, bool? useProxy = null, string? urlFormat = null, int? tokenLifetimeInMinutes = null, string? accessToken = null)
        {
            try
            {
                var json = await FetchStreamingUrlJsonAsync(location, accountId, videoId, useProxy, urlFormat, tokenLifetimeInMinutes, accessToken);
                return ParseStreamingUrlJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving streaming URL.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving streaming URL.");
                return null;
            }
        }

        /// <summary>
        /// ストリーミング URL を取得するための API から JSON を取得します。
        /// Get Video Streaming URL
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Streaming-URL
        /// </summary>
        /// <param name="location">Azure リージョン名</param>
        /// <param name="accountId">Video Indexer アカウント ID</param>
        /// <param name="videoId">対象ビデオ ID</param>
        /// <param name="useProxy">プロキシ使用の有無（Safari対応）</param>
        /// <param name="urlFormat">ストリーミング URL フォーマット</param>
        /// <param name="tokenLifetimeInMinutes">トークン有効期限（分）</param>
        /// <param name="accessToken">アクセストークン（任意）</param>
        /// <returns>ストリーミング URL 情報の JSON テキスト。失敗時は null。</returns>
        public async Task<string?> FetchStreamingUrlJsonAsync(string location, string accountId, string videoId, bool? useProxy, string? urlFormat, int? tokenLifetimeInMinutes, string? accessToken)
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/streaming-url";
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (useProxy.HasValue) query["useProxy"] = useProxy.Value.ToString().ToLower();
            if (!string.IsNullOrWhiteSpace(urlFormat)) query["urlFormat"] = urlFormat;
            if (tokenLifetimeInMinutes.HasValue) query["tokenLifetimeInMinutes"] = tokenLifetimeInMinutes.ToString();
            if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

            if (query.Count > 0)
                url += "?" + query.ToString();

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            //var response = await _httpClient.SendAsync(request);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to retrieve video streaming URL: {Error}", error);
                return null;
            }

            _logger.LogInformation("Video streaming URL JSON retrieved successfully.");
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// ストリーミング URL 情報の JSON テキストを ApiStreamingUrlModel オブジェクトに変換します。
        /// Get Video Streaming URL
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Streaming-URL
        /// </summary>
        /// <param name="json">JSON テキスト</param>
        /// <returns>ApiStreamingUrlModel オブジェクト。失敗時は null。</returns>
        public ApiStreamingUrlModel? ParseStreamingUrlJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<ApiStreamingUrlModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error occurred while parsing streaming URL.");
                return null;
            }
        }

        // Get Video Thumbnail

        /// <summary>
        /// 指定された動画のサムネイル画像を取得します。
        /// Get Video Thumbnail
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Thumbnail
        /// </summary>
        /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
        /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
        /// <param name="videoId">対象のビデオ ID</param>
        /// <param name="thumbnailId">取得したいサムネイルの ID（GUID）</param>
        /// <param name="format">返却されるサムネイルの形式（"Jpeg" または "Base64"）※省略時はデフォルト形式</param>
        /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
        /// <returns>サムネイル画像のバイナリ配列（JPEG）または Base64 文字列。取得に失敗した場合は null。</returns>
        public async Task<byte[]?> GetVideoThumbnailAsync(string location, string accountId, string videoId, string thumbnailId, string? format = null, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Thumbnails/{thumbnailId}";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrWhiteSpace(format)) query["format"] = format;
                if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                //var response = await _httpClient.SendAsync(request);
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to retrieve video thumbnail: {Error}", error);
                    return null;
                }

                _logger.LogInformation("Video thumbnail retrieved successfully.");
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while retrieving video thumbnail.");
                throw;
                //return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving video thumbnail.");
                throw;
                //return null;
            }
        }

        // List Videos

        /// <summary>
        /// 指定された Video Indexer アカウント内の動画一覧を取得します。
        /// List Videos
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos
        /// </summary>
        /// <param name="location">Azureリージョン。例: "japaneast"</param>
        /// <param name="accountId">Video IndexerアカウントのGUID</param>
        /// <param name="createdAfter">指定日以降に作成された動画にフィルタリング（RFC3339形式）</param>
        /// <param name="createdBefore">指定日以前に作成された動画にフィルタリング（RFC3339形式）</param>
        /// <param name="pageSize">1ページあたりの取得件数</param>
        /// <param name="skip">先頭からスキップする動画数</param>
        /// <param name="partitions">パーティションに基づいて動画をフィルタ</param>
        /// <param name="accessToken">APIアクセス用のアクセストークン</param>
        /// <returns>
        /// <see cref="VideoSearchResult"/> オブジェクト。成功時は動画の一覧情報を含みます。失敗時は null を返します。
        /// </returns>
        public async Task<ApiVideoSearchResultModel?> ListVideosAsync(string location, string accountId, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null, string? accessToken = null)
        {
            var url = BuildListVideosUrl(location, accountId, createdAfter, createdBefore, pageSize, skip, partitions, accessToken);
            var json = await FetchVideoListJsonAsync(url);
            return ParseVideoListJson(json);
        }


        /// <summary>
        /// ListVideos API用のURLを生成します。
        /// List Videos
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos
        /// </summary>
        /// <param name="location">APIのルーティング対象となるAzureリージョン（例: "japaneast"）</param>
        /// <param name="accountId">Video IndexerアカウントのGUID</param>
        /// <param name="createdAfter">この日時以降に作成された動画のみを取得（RFC3339形式）</param>
        /// <param name="createdBefore">この日時以前に作成された動画のみを取得（RFC3339形式）</param>
        /// <param name="pageSize">取得する動画件数（最大件数）</param>
        /// <param name="skip">スキップする動画件数（ページネーション用）</param>
        /// <param name="partitions">対象動画のパーティションフィルタ（省略可）</param>
        /// <param name="accessToken">Video Indexer APIのアクセストークン（有効期間は1時間）</param>
        /// <returns>生成されたAPIの完全なリクエストURL</returns>
        public string BuildListVideosUrl(string location, string accountId, string? createdAfter, string? createdBefore, int? pageSize, int? skip, string[]? partitions, string? accessToken)
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos";
            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

            if (!string.IsNullOrWhiteSpace(createdAfter)) query["createdAfter"] = createdAfter;
            if (!string.IsNullOrWhiteSpace(createdBefore)) query["createdBefore"] = createdBefore;
            if (pageSize.HasValue) query["pageSize"] = pageSize.ToString();
            if (skip.HasValue) query["skip"] = skip.ToString();
            if (partitions != null && partitions.Length > 0) query["partitions"] = string.Join(",", partitions);
            if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

            if (query.Count > 0)
                url += "?" + query.ToString();

            return url;
        }

        /// <summary>
        /// 指定した URL に対して GET リクエストを送信し、JSON 結果を文字列として返却します。
        /// List Videos
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos
        /// </summary>
        /// <param name="url">取得対象のAPIエンドポイント</param>
        /// <returns>取得成功時はJSON文字列、失敗時はnull</returns>
        public async Task<string?> FetchVideoListJsonAsync(string url)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                //var response = await _httpClient.SendAsync(request);
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Failed to list videos: {Error}", error);
                    return null;
                }

                _logger.LogInformation("Video list retrieved successfully.");
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while listing videos.");
                throw;
                //return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while listing videos.");
                throw;
                //return null;
            }
        }

        /// <summary>
        /// JSON文字列を ApiVideoSearchResultModel モデルにパースします。
        /// List Videos
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Search-Videos
        /// </summary>
        /// <param name="json">ListVideos APIから返されたJSON文字列</param>
        /// <returns>ApiVideoSearchResultModel モデル、または失敗時は null</returns>
        public ApiVideoSearchResultModel? ParseVideoListJson(string? json)
        {
            if (string.IsNullOrWhiteSpace(json)) return null;

            try
            {
                return JsonSerializer.Deserialize<ApiVideoSearchResultModel>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse video list JSON.");
                return null;
            }
        }

        // Update Video Transcript

        /// <summary>
        /// 指定された動画に新しいトランスクリプト（VTT形式）をアップロードし、再インデックスを実行します。
        /// </summary>
        /// <param name="location">APIの地域（例: "japaneast"）</param>
        /// <param name="accountId">Video IndexerアカウントのGUID</param>
        /// <param name="videoId">対象の動画ID</param>
        /// <param name="vttContent">VTT形式のトランスクリプトテキスト</param>
        /// <param name="language">言語コード（例: "ja-JP"）</param>
        /// <param name="setAsSourceLanguage">指定言語をソース言語として設定するか</param>
        /// <param name="callbackUrl">処理完了時に呼び出されるURL（省略可）</param>
        /// <param name="sendSuccessEmail">成功通知メールを送信するか</param>
        /// <param name="accessToken">アクセストークン（Contributor 権限）</param>
        /// <returns>成功時は true、失敗時は false を返します。</returns>
        public async Task<bool> UpdateVideoTranscriptAsync(string location, string accountId, string videoId, string vttContent, string? language = null, bool? setAsSourceLanguage = null, string? callbackUrl = null, bool? sendSuccessEmail = null, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/Index/Transcript";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (!string.IsNullOrWhiteSpace(language)) query["language"] = language;
                if (setAsSourceLanguage.HasValue) query["setAsSourceLanguage"] = setAsSourceLanguage.Value.ToString().ToLower();
                if (!string.IsNullOrWhiteSpace(callbackUrl)) query["callbackUrl"] = callbackUrl;
                if (sendSuccessEmail.HasValue) query["sendSuccessEmail"] = sendSuccessEmail.Value.ToString().ToLower();
                if (!string.IsNullOrWhiteSpace(accessToken)) query["accessToken"] = accessToken;

                if (query.Count > 0)
                    url += "?" + query.ToString();

                var request = new HttpRequestMessage(HttpMethod.Put, url)
                {
                    Content = new StringContent(vttContent, Encoding.UTF8, "text/vtt")
                };

                request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

                //var response = await _httpClient.SendAsync(request);
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Transcript updated successfully.");
                    return true;
                }

                var error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Failed to update transcript: {Error}", error);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error occurred while updating transcript.");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating transcript.");
                return false;
            }
        }

    }
}

