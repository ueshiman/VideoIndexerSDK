using Microsoft.Extensions.Logging;
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
        private async Task<string> SendPostRequestForCancelRenderAsync(string location, string accountId, string projectId, string? accessToken)
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
        private ApiProjectRenderOperationModel? ParseRenderOperationJson(string json)
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
    }
}
