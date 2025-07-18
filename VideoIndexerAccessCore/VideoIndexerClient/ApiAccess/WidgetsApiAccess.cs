﻿using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class WidgetsApiAccess : IWidgetsApiAccess
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
        /// Insights Widget のURLを取得します。
        /// Video Indexer API から Insights Widget のリダイレクトURLを取得し返却します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">インサイトを取得する対象のビデオID</param>
        /// <param name="widgetType">（任意）表示するウィジェットの種類（People / Sentiments / Keywords / Search）</param>
        /// <param name="allowEdit">（任意）ウィジェットが編集可能かどうか</param>
        /// <param name="accessToken">（任意）アクセストークン。編集やプライベートビデオの場合に必要</param>
        /// <returns>リダイレクトされた Insights Widget のURL。失敗時は null</returns>
        public async Task<string?> GetVideoInsightsWidgetUrl(string location, string accountId, string videoId, string? widgetType, bool? allowEdit, string? accessToken)
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/InsightsWidget";

            var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(widgetType)) query["widgetType"] = widgetType;
            if (allowEdit.HasValue) query["allowEdit"] = allowEdit.Value.ToString().ToLower();
            if (!string.IsNullOrEmpty(accessToken)) query["accessToken"] = accessToken;
            var finalUrl = $"{url}?{query}";

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            var response = await httpClient.GetAsync(finalUrl) ?? throw new HttpRequestException("The response was null.");

            if (response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently)
            {
                string widgetUrl = response.Headers.Location?.ToString() ?? throw new HttpRequestException("Redirect Location header is missing.");
                _logger.LogInformation("Widget URL: {WidgetUrl}", widgetUrl);
                return widgetUrl;
            }
            else
            {
                _logger.LogWarning("Status code: {StatusCode}", response.StatusCode);
                string error = await response.Content.ReadAsStringAsync();
                _logger.LogWarning("Error: {Error}", error);
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
        public async Task<string> FetchVideoInsightsJsonAsync(string location, string accountId, string videoId, string? widgetType, bool? allowEdit, string? accessToken)
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
                var response = await httpClient.GetAsync(finalUrl) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を
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
        public ApiVideoInsightsWidgetResponseModel? ParseVideoInsightsJson(string json)
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

        // Get Video Player Widget

        /// <summary>
        /// Video Player Widget のリダイレクトURLを取得します。
        /// Video Indexer API から Player Widget のリダイレクトURLを取得し返却します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象のビデオID</param>
        /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
        /// <returns>リダイレクトされた Player Widget のURL。失敗時は null</returns>
        public async Task<string?> GetVideoPlayerWidgetUrl(string location, string accountId, string videoId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PlayerWidget";
                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrEmpty(accessToken)) query["accessToken"] = accessToken;
                var finalUrl = $"{url}?{query}";
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(finalUrl) ?? throw new HttpRequestException("The response was null.");
                if (response.StatusCode == HttpStatusCode.Moved || response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.MovedPermanently)
                {
                    return response.Headers.Location?.ToString() ?? throw new HttpRequestException("Redirect Location header is missing.");
                }
                else
                {
                    _logger.LogWarning("Status code: {StatusCode}", response.StatusCode);
                    string error = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Error: {Error}", error);
                    return null;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while calling Video Player Widget API.");
                return null;
            }
        }

        /// <summary>
        /// 外部公開用メソッド。Video Player Widget の情報を取得します。
        /// </summary>
        /// <param name="location">API呼び出し先のリージョン（例：trial）</param>
        /// <param name="accountId">Video Indexer アカウントの GUID</param>
        /// <param name="videoId">対象のビデオID</param>
        /// <param name="accessToken">（任意）アクセストークン。プライベートビデオなどに必要</param>
        /// <returns>Player Widget URLを格納した <see cref="ApiVideoPlayerWidgetResponseModel"/>。失敗時は null</returns>
        public async Task<ApiVideoPlayerWidgetResponseModel?> GetVideoPlayerWidgetAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            try
            {
                var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Videos/{videoId}/PlayerWidget";

                var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrEmpty(accessToken)) query["accessToken"] = accessToken;

                var finalUrl = $"{url}?{query}";

                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(finalUrl) ?? throw new HttpRequestException("The response was null.");
                // responseがnullなら例外を

                if (response.StatusCode == System.Net.HttpStatusCode.MovedPermanently)
                {
                    var redirectUrl = response.Headers.Location?.ToString();
                    return new ApiVideoPlayerWidgetResponseModel
                    {
                        playerWidgetUrl = redirectUrl
                    };
                }

                response.EnsureSuccessStatusCode();

                // 成功だがリダイレクトじゃない（ありえる？）
                return new ApiVideoPlayerWidgetResponseModel
                {
                    playerWidgetUrl = finalUrl
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error while calling Video Player Widget API.");
                return new ApiVideoPlayerWidgetResponseModel
                {
                    errorType = "HTTP_REQUEST_ERROR",
                    message = ex.Message
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while retrieving video player widget.");
                return new ApiVideoPlayerWidgetResponseModel
                {
                    errorType = "UNEXPECTED_ERROR",
                    message = ex.Message
                };
            }
        }

    }
}

