using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class TrialAccountAccessTokensApiAccess
    {
        private readonly ILogger<TrialAccountAccessTokensApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // Get Account Access Token

        /// <summary>
        /// アカウントアクセストークンを取得する非同期メソッド。
        /// Get Account Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Access-Token
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
        /// Get Account Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Access-Token
        /// </summary>
        /// <param name="location">Azure リージョン。</param>
        /// <param name="accountId">アカウント ID。</param>
        /// <param name="allowEdit">編集許可の有無（true または false）。省略可。</param>
        /// <param name="clientRequestId">クライアントからのリクエスト ID。省略可。</param>
        /// <returns>JSON形式のアクセストークンレスポンス文字列。</returns>
        private async Task<string> FetchAccessTokenJsonAsync(string location, string accountId, bool? allowEdit = null, string? clientRequestId = null)
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
            var response = await httpClient.SendAsync(request);
            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// アクセストークンの JSON 文字列をパースしてトークン文字列を返す。
        /// Get Account Access Token
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Account-Access-Token
        /// </summary>
        /// <param name="json">APIから取得した JSON 文字列。</param>
        /// <returns>アクセストークンの文字列。null または不正な形式の場合は例外をスロー。</returns>
        private string ParseAccessTokenJson(string json)
        {
            // トークンは文字列としてそのまま返される形式
            return JsonSerializer.Deserialize<string>(json)
                   ?? throw new JsonException("Access token is null or invalid.");
        }

    }
}
