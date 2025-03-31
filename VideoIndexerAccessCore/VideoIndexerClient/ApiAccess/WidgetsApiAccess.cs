using Azure.Core;
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
    public class WidgetsApiAccess
    {
        private readonly ILogger<WidgetsApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public WidgetsApiAccess(ILogger<WidgetsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        // Get Video Insights Widget

        /// <summary>
        /// 外部公開用メソッド。Insights Widgetの情報を取得します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">インサイトを取得する対象のビデオID</param>
        /// <param name="widgetType">（任意）表示するウィジェットの種類（People / Sentiments / Keywords / Search）</param>
        /// <param name="allowEdit">（任意）ウィジェットが編集可能かどうか</param>
        /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
        /// <returns>ウィジェット情報を格納した <see cref="ApiVideoInsightsWidgetResponseModel"/> インスタンス。失敗時は null</returns>
        public async Task<ApiVideoInsightsWidgetResponseModel?> GetVideoInsightsWidgetAsync(string location, string accountId, string videoId, string? widgetType = null, bool? allowEdit = null, string? accessToken = null)
        {
            try
            {
                var json = await FetchVideoInsightsJsonAsync(location, accountId, videoId, widgetType, allowEdit, accessToken);
                return ParseVideoInsightsJson(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving video insights.");
                return null;
            }
        }

        /// <summary>
        /// Insights Widget API にアクセスし JSON レスポンスを取得します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">インサイトを取得する対象のビデオID</param>
        /// <param name="widgetType">（任意）表示するウィジェットの種類（People / Sentiments / Keywords / Search）</param>
        /// <param name="allowEdit">（任意）ウィジェットが編集可能かどうか</param>
        /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
        /// <returns>APIから取得した生のJSONレスポンス文字列</returns>
        private async Task<string> FetchVideoInsightsJsonAsync(string location, string accountId, string videoId, string? widgetType, bool? allowEdit, string? accessToken)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/InsightsWidget";

                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrEmpty(widgetType)) query["widgetType"] = widgetType;
                if (allowEdit.HasValue) query["allowEdit"] = allowEdit.Value.ToString().ToLower();
                if (!string.IsNullOrEmpty(accessToken)) query["accessToken"] = accessToken;
                var finalUrl = $"{url}?{query}";

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(finalUrl);
                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while calling Video Insights API.");
                throw;
            }
        }

        /// <summary>
        /// JSONレスポンスをオブジェクトに変換します。
        /// </summary>
        /// <param name="json">APIから取得したJSON文字列</param>
        /// <returns>デシリアライズされた <see cref="ApiVideoInsightsWidgetResponseModel"/> オブジェクト。失敗時は null</returns>
        private ApiVideoInsightsWidgetResponseModel? ParseVideoInsightsJson(string json)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiVideoInsightsWidgetResponseModel>(json);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response.");
                return null;
            }
        }
    }
}

