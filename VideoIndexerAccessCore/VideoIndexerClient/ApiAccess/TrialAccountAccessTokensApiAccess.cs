using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class TrialAccountAccessTokensApiAccess : ITrialAccountAccessTokensApiAccess
    {
        private readonly ILogger<TrialAccountAccessTokensApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // Get ApiTrialAccountModel Access Token

        public TrialAccountAccessTokensApiAccess(ILogger<TrialAccountAccessTokensApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// アカウントアクセストークンを取得する非同期メソッド。
        /// Get ApiTrialAccountModel Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
        /// </summary>
        /// <param name="location">Azureリージョンを示す文字列（例: "japaneast"）。</param>
        /// <param name="accountId">アカウントID（GUID形式）。</param>
        /// <param name="allowEdit">トークンに編集権限を与えるかどうか（true = 編集可, false = 読取専用）。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchAccessTokenJsonAsync(location, accountId, allowEdit, clientRequestId);
                return ParseAccessTokenJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting access token.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading access token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting access token.");
            }

            return null;
        }

        /// <summary>
        /// 指定されたパラメータに基づいて Video Indexer API からアクセストークンを取得する非同期メソッド。
        /// Get ApiTrialAccountModel Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="accountId">アカウント ID。</param>
        /// <param name="allowEdit">編集許可の有無（true または false）。省略可。</param>
        /// <param name="clientRequestId">クライアントからのリクエスト ID。省略可。</param>
        /// <returns>JSON形式のアクセストークンレスポンス文字列。</returns>
        public async Task<string> FetchAccessTokenJsonAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null)
        {
            var endpoint = $"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Accounts/{accountId}/AccessToken";
            var uriBuilder = new UriBuilder(endpoint);

            if (allowEdit.HasValue)
            {
                var query = $"allowEdit={allowEdit.Value.ToString().ToLower()}";
                uriBuilder.Query = query;
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// アクセストークンの JSON 文字列をパースしてトークン文字列を返す。
        /// Get ApiTrialAccountModel Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token
        /// </summary>
        /// <param name="json">APIから取得した JSON 文字列。</param>
        /// <returns>アクセストークンの文字列。null または不正な形式の場合は例外をスロー。</returns>
        public string ParseAccessTokenJson(string json)
        {
            // トークンは文字列としてそのまま返される形式
            return JsonSerializer.Deserialize<string>(json)
                   ?? throw new JsonException("Access token is null or invalid.");
        }

        // Get ApiTrialAccountModel Access Token With Permission (PREVIEW)

        /// <summary>
        /// アカウントアクセストークン（パーミッション指定付き）を取得する非同期メソッド。
        /// Get ApiTrialAccountModel Access Token With Permission (PREVIEW)
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token-With-Permission
        /// </summary>
        /// <param name="location">Azureリージョン。</param>
        /// <param name="accountId">アカウントID（GUID形式）。</param>
        /// <param name="permission">取得するパーミッション（Reader / Contributor / Owner など）。省略可。</param>
        /// <param name="clientRequestId">クライアントリクエストID。省略可。</param>
        /// <returns>アクセストークン文字列。エラーが発生した場合は null。</returns>
        public async Task<string?> GetAccountAccessTokenWithPermissionAsync(string location, string accountId, string? permission = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchAccessTokenWithPermissionJsonAsync(location, accountId, permission, clientRequestId);
                return ParseAccessTokenJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting access token with permission.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading access token with permission.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting access token with permission.");
            }

            return null;
        }

        /// <summary>
        /// 指定されたパーミッションに基づいて Video Indexer API からアクセストークンを取得する非同期メソッド。
        /// Get ApiTrialAccountModel Access Token With Permission (PREVIEW)
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-ApiTrialAccountModel-Access-Token-With-Permission
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="accountId">アカウント ID。</param>
        /// <param name="permission">リクエストするパーミッション。省略可。</param>
        /// <param name="clientRequestId">クライアントからのリクエスト ID。省略可。</param>
        /// <returns>JSON形式のアクセストークンレスポンス文字列。</returns>
        public async Task<string> FetchAccessTokenWithPermissionJsonAsync(string location, string accountId, string? permission = null, string? clientRequestId = null)
        {
            var endpoint = $"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Accounts/{accountId}/AccessTokenWithPermission";
            var uriBuilder = new UriBuilder(endpoint);

            if (!string.IsNullOrWhiteSpace(permission))
            {
                uriBuilder.Query = $"permission={Uri.EscapeDataString(permission)}";
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Get Accounts Authorization

        /// <summary>
        /// アカウントの一覧を取得する非同期メソッド（オプションでアクセストークン付き）。
        /// Get Accounts Authorization
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
        /// </summary>
        /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
        /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を与えるか。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>ApiAccountSlimModel オブジェクトのリスト。失敗時は null。</returns>
        public async Task<List<ApiAccountSlimModel>?> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchAccountsJsonAsync(location, generateAccessTokens, allowEdit, clientRequestId);
                return ParseAccountsJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting accounts.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading accounts list.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting accounts.");
            }

            return null;
        }

        /// <summary>
        /// アカウント一覧の JSON をパースしてオブジェクトに変換するメソッド。
        /// Get Accounts Authorization
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
        /// </summary>
        /// <param name="json">APIから取得したアカウント一覧のJSON文字列。</param>
        /// <returns>ApiAccountSlimModel オブジェクトのリスト。</returns>
        public List<ApiAccountSlimModel>? ParseAccountsJson(string json)
        {
            return JsonSerializer.Deserialize<List<ApiAccountSlimModel>>(json);
        }


        /// <summary>
        /// アカウントの一覧を取得する非同期メソッド（オプションでアクセストークン付き）。
        /// Get Accounts Authorization
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-Authorization
        /// </summary>
        /// <param name="location">Azureリージョン（例: "japaneast"）。</param>
        /// <param name="generateAccessTokens">各アカウントにアクセストークンを含めるかどうか。省略可。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を与えるか。省略可。</param>
        /// <param name="clientRequestId">リクエスト識別用のGUID文字列。省略可。</param>
        /// <returns>ApiAccountSlimModel オブジェクトのリスト。失敗時は null。</returns>
        public async Task<string> FetchAccountsJsonAsync(string location, bool? generateAccessTokens, bool? allowEdit, string? clientRequestId)
        {
            var endpoint = $"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Accounts";
            var queryParams = new List<string>();

            if (generateAccessTokens.HasValue)
                queryParams.Add($"generateAccessTokens={generateAccessTokens.Value.ToString().ToLower()}");

            if (allowEdit.HasValue)
                queryParams.Add($"allowEdit={allowEdit.Value.ToString().ToLower()}");

            var uriBuilder = new UriBuilder(endpoint)
            {
                Query = string.Join("&", queryParams)
            };

            using var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // Get Project Access Token

        /// <summary>
        /// プロジェクトに対するアクセストークンを取得する非同期メソッド。
        /// Get Project Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
        /// </summary>
        /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
        /// <param name="accountId">アカウント ID（GUID形式）。</param>
        /// <param name="projectId">プロジェクト ID。</param>
        /// <param name="allowEdit">編集を許可するかどうか（true で書き込み可）。省略可。</param>
        /// <param name="clientRequestId">リクエストトラッキング用の GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetProjectAccessTokenAsync(string location, string accountId, string projectId, bool? allowEdit = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchProjectAccessTokenJsonAsync(location, accountId, projectId, allowEdit, clientRequestId);
                return ParseProjectAccessTokenJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting project access token.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading project access token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting project access token.");
            }
            return null;
        }

        /// <summary>
        /// プロジェクトアクセストークン取得用の JSON を Video Indexer API から取得する非同期メソッド。
        /// Get Project Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
        /// </summary>
        /// <param name="location">API 呼び出し対象の Azure リージョン（例: "japaneast"）。</param>
        /// <param name="accountId">対象のアカウント ID（GUID形式）。</param>
        /// <param name="projectId">対象のプロジェクト ID。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を含めるか（true: 編集可、false: 読み取り専用）。省略可。</param>
        /// <param name="clientRequestId">リクエストの識別に使用される GUID（任意）。</param>
        /// <returns>API 応答の JSON 文字列。</returns>
        public async Task<string> FetchProjectAccessTokenJsonAsync(string location, string accountId, string projectId, bool? allowEdit, string? clientRequestId)
        {
            var uri = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Accounts/{accountId}/Projects/{projectId}/AccessToken");
            if (allowEdit.HasValue)
            {
                uri.Query = $"allowEdit={allowEdit.Value.ToString().ToLower()}";
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSON 文字列から単一の文字列値をデシリアライズする汎用メソッド。
        /// Get Project Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Project-Access-Token
        /// </summary>
        /// <param name="json">文字列を含む JSON データ。</param>
        /// <returns>デシリアライズされた文字列。null や不正な形式の場合は例外をスロー。</returns>
        public string ParseProjectAccessTokenJson(string json)
        {
            return JsonSerializer.Deserialize<string>(json) ?? throw new JsonException("Expected a JSON string value but got null or invalid.");
        }

        // Get User Access Token

        /// <summary>
        /// ユーザーに対するアクセストークンを取得する非同期メソッド。
        /// Get User Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
        /// </summary>
        /// <param name="location">API 呼び出し対象の Azure リージョン（例: "japaneast"）。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を付与するか（true: 編集可, false: 読み取り専用）。省略可。</param>
        /// <param name="clientRequestId">リクエストを識別する GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。エラー時は null を返す。</returns>
        public async Task<string?> GetUserAccessTokenAsync(string location, bool? allowEdit = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchUserAccessTokenJsonAsync(location, allowEdit, clientRequestId);
                return ParseUserAccessTokenJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting user access token.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading user access token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting user access token.");
            }

            return null;
        }

        /// <summary>
        /// ユーザーアクセストークンを取得する API を呼び出して JSON 文字列を取得する。
        /// Get User Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="allowEdit">編集を許可するか（true または false）。省略可。</param>
        /// <param name="clientRequestId">任意のリクエスト ID。</param>
        /// <returns>API 応答の JSON 文字列。</returns>
        public async Task<string> FetchUserAccessTokenJsonAsync(string location, bool? allowEdit = null, string? clientRequestId = null)
        {
            var uri = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Users/me/AccessToken");
            if (allowEdit.HasValue)
            {
                uri.Query = $"allowEdit={allowEdit.Value.ToString().ToLower()}";
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// ユーザーアクセストークンの JSON を解析してトークン文字列を抽出する。
        /// Get User Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>アクセストークンの文字列。</returns>
        public string ParseUserAccessTokenJson(string json)
        {
            return ParseStringJson(json);
        }


        /// <summary>
        /// JSON 文字列から単一の文字列値をデシリアライズする汎用メソッド。
        /// Get User Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-User-Access-Token
        /// </summary>
        /// <param name="json">文字列を含む JSON データ。</param>
        /// <returns>デシリアライズされた文字列。null や不正な形式の場合は例外をスロー。</returns>
        public string ParseStringJson(string json)
        {
            return JsonSerializer.Deserialize<string>(json) ?? throw new JsonException("Expected a JSON string value but got null or invalid.");
        }

        // Get Video Access Token

        /// <summary>
        /// ビデオに対するアクセストークンを取得する非同期メソッド。
        /// Get Video Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
        /// </summary>
        /// <param name="location">Azure リージョン（例: "japaneast"）。</param>
        /// <param name="accountId">対象のアカウント ID（GUID形式）。</param>
        /// <param name="videoId">対象のビデオ ID。</param>
        /// <param name="allowEdit">アクセストークンに編集権限を含めるか（true: 編集可、false: 読み取り専用）。省略可。</param>
        /// <param name="clientRequestId">リクエストを識別するための GUID（省略可）。</param>
        /// <returns>アクセストークンの文字列。失敗時は null。</returns>
        public async Task<string?> GetVideoAccessTokenAsync(string location, string accountId, string videoId, bool? allowEdit = null, string? clientRequestId = null)
        {
            try
            {
                var json = await FetchVideoAccessTokenJsonAsync(location, accountId, videoId, allowEdit, clientRequestId);
                return ParseVideoAccessTokenJson(json);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API communication error while getting video access token.");
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error while reading video access token.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting video access token.");
            }
            return null;
        }

        /// <summary>
        /// Video Indexer API からビデオアクセストークンの JSON データを取得する非同期メソッド。
        /// Get Video Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
        /// </summary>
        /// <param name="location">API 呼び出し対象の Azure リージョン。</param>
        /// <param name="accountId">アカウント ID（GUID形式）。</param>
        /// <param name="videoId">ビデオ ID。</param>
        /// <param name="allowEdit">編集権限を付与するか（true または false）。省略可。</param>
        /// <param name="clientRequestId">リクエストトラッキング用の GUID（任意）。</param>
        /// <returns>API 応答の JSON 文字列。</returns>
        public async Task<string> FetchVideoAccessTokenJsonAsync(string location, string accountId, string videoId, bool? allowEdit = null, string? clientRequestId = null)
        {
            var uri = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/Auth/{location}/Accounts/{accountId}/Videos/{videoId}/AccessToken");
            if (allowEdit.HasValue)
            {
                uri.Query = $"allowEdit={allowEdit.Value.ToString().ToLower()}";
            }

            using var request = new HttpRequestMessage(HttpMethod.Get, uri.Uri);
            if (!string.IsNullOrEmpty(clientRequestId))
            {
                request.Headers.Add("x-ms-client-request-id", clientRequestId);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.SendAsync(request) ?? throw new HttpRequestException("The response was null.");
            // responseがnullなら例外を
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        /// <summary>
        /// ビデオアクセストークンの JSON を解析してトークン文字列を抽出する。
        /// Get Video Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Access-Token
        /// </summary>
        /// <param name="json">JSON 文字列。</param>
        /// <returns>アクセストークンの文字列。</returns>
        public string ParseVideoAccessTokenJson(string json)
        {
            return ParseStringJson(json);
        }
    }
}
