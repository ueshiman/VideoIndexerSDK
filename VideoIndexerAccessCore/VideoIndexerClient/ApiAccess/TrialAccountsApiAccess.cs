using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class TrialAccountsApiAccess : ITrialAccountsApiAccess
    {
        private readonly ILogger<TrialAccountsApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public TrialAccountsApiAccess(ILogger<TrialAccountsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        // Get ApiTrialAccountModel

        /// <summary>
        /// Video Indexer API からアカウント情報を取得する非同期メソッド
        /// Get ApiTrialAccountModel
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
        /// </summary>
        /// <param name="location">APIの呼び出し先リージョン</param>
        /// <param name="accountId">アカウントID (GUID)</param>
        /// <param name="includeUsage">使用状況情報を含めるか</param>
        /// <param name="includeStatistics">統計情報を含めるか</param>
        /// <param name="accessToken">オプションのアクセストークン</param>
        /// <returns>ApiTrialAccountModel オブジェクトの配列</returns>
        public async Task<ApiTrialAccountModel[]> GetAccountAsync(string location, string accountId, bool? includeUsage = null, bool? includeStatistics = null, string? accessToken = null)
        {
            var json = await FetchAccountJsonAsync(location, accountId, includeUsage, includeStatistics, accessToken);
            return ParseAccountJson(json);
        }

        /// <summary>
        /// API に HTTP GET リクエストを送信し、JSON 形式のアカウント情報を取得する
        /// Get ApiTrialAccountModel
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
        /// </summary>
        /// <param name="location">Azure リージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="includeUsage">使用量情報の有無</param>
        /// <param name="includeStatistics">統計情報の有無</param>
        /// <param name="accessToken">アクセストークン（省略可）</param>
        /// <returns>レスポンスJSON文字列</returns>
        public async Task<string> FetchAccountJsonAsync(string location, string accountId, bool? includeUsage, bool? includeStatistics, string? accessToken)
        {
            try
            {
                var uriBuilder = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}");
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (includeUsage.HasValue)
                    query["includeUsage"] = includeUsage.Value.ToString().ToLower();
                if (includeStatistics.HasValue)
                    query["includeStatistics"] = includeStatistics.Value.ToString().ToLower();
                if (!string.IsNullOrEmpty(accessToken))
                    query["accessToken"] = accessToken;

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                uriBuilder.Query = query.ToString();
                var response = await httpClient.GetAsync(uriBuilder.Uri) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 取得した JSON 文字列を ApiTrialAccountModel オブジェクトの配列に変換する
        /// Get ApiTrialAccountModel
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account
        /// </summary>
        /// <param name="json">JSON文字列</param>
        /// <returns>Account配列</returns>
        public ApiTrialAccountModel[] ParseAccountJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiTrialAccountModel[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON: {Message}", ex.Message);
                throw;
            }
        }

        // Get Accounts With Token

        /// <summary>
        /// Video Indexer API からアカウント一覧を取得します。
        /// Get Accounts With Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-With-Token
        /// </summary>
        /// <param name="location">リクエストをルーティングする Azure リージョン。</param>
        /// <param name="generateAccessTokens">各アカウントに対してアクセストークンを生成するかどうか。</param>
        /// <param name="allowEdit">アクセストークンに書き込み権限（Contributor）を含めるかどうか。</param>
        /// <param name="accessToken">（任意）クエリパラメータまたは Authorization ヘッダーで渡すアクセストークン。</param>
        /// <returns>取得したアカウント情報の配列。</returns>
        public async Task<ApiTrialAccountModel[]> GetAccountsAsync(string location, bool? generateAccessTokens = null, bool? allowEdit = null, string? accessToken = null)
        {
            var json = await FetchAccountsJsonAsync(location, generateAccessTokens, allowEdit, accessToken);
            return ParseAccountJson(json);
        }

        /// <summary>
        /// アカウント一覧を取得する API を呼び出し、JSON文字列を取得します。
        /// Get Accounts With Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Accounts-With-Token
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="generateAccessTokens">アクセストークンを生成するかどうか。</param>
        /// <param name="allowEdit">編集許可付きトークンを生成するかどうか。</param>
        /// <param name="accessToken">（任意）アクセストークン。</param>
        /// <returns>レスポンスとして返却された JSON 文字列。</returns>
        public async Task<string> FetchAccountsJsonAsync(string location, bool? generateAccessTokens, bool? allowEdit, string? accessToken)
        {
            try
            {
                var uriBuilder = new UriBuilder($"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts");
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);

                if (generateAccessTokens.HasValue)
                    query["generateAccessTokens"] = generateAccessTokens.Value.ToString().ToLower();
                if (allowEdit.HasValue)
                    query["allowEdit"] = allowEdit.Value.ToString().ToLower();
                if (!string.IsNullOrEmpty(accessToken))
                    query["accessToken"] = accessToken;

                uriBuilder.Query = query.ToString();
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(uriBuilder.Uri) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred: {Message}", ex.Message);
                throw;
            }
        }
    }
}

