using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class JobsApiAccess : IJobsApiAccess
    {
        private readonly ILogger<JobsApiAccess>? _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public JobsApiAccess(ILogger<JobsApiAccess>? logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// 指定されたジョブ ID に対するステータスを取得する
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="jobId">ジョブ ID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>ジョブのステータス情報を含むレスポンス</returns>
        public async Task<ApiJobStatusResponseModel?> GetJobStatusAsync(string location, string accountId, string jobId, string? accessToken = null)
        {
            // API エンドポイントの組み立て
            string endpoint = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Jobs/{jobId}";

            if (!string.IsNullOrEmpty(accessToken))
            {
                endpoint += $"?accessToken={accessToken}";
            }

            try
            {
                // API から JSON レスポンスを取得
                string jsonResponse = await FetchJobStatusJsonAsync(endpoint);
                return ParseJobStatusJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error while fetching job status."); // エラーログを記録
                return null;
            }
        }

        /// <summary>
        /// 指定された URL からジョブステータスの JSON を取得する
        /// </summary>
        /// <param name="url">API のエンドポイント URL</param>
        /// <returns>ジョブステータスの JSON 文字列</returns>
        public async Task<string> FetchJobStatusJsonAsync(string url)
        {
            HttpResponseMessage? response;
            try
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("Accept", "application/json");

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                response = await httpClient.SendAsync(request);

                if (response is null) throw new HttpRequestException("The response was null.");

                response.EnsureSuccessStatusCode(); // HTTP エラー時に例外を発生

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger?.LogError(ex, "HTTP request failed: {Url}", url); // HTTP エラーをログに記録
                throw;
            }
        }

        /// <summary>
        /// API から取得した JSON を解析し、JobStatusResponse オブジェクトに変換する
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>解析後の JobStatusResponse オブジェクト</returns>
        public ApiJobStatusResponseModel? ParseJobStatusJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiJobStatusResponseModel>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Failed to parse JSON response: {Json}", json); // JSON パースエラーをログに記録
                return null;
            }
        }
    }
}
