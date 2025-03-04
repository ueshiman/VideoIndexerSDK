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
    public class PersonModelsApiAccess
    {
        private readonly ILogger<PersonModelsApiAccess> _logger;
        private readonly IDurableHttpClient? _durableHttpClient;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;


        public PersonModelsApiAccess(ILogger<PersonModelsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
        {
            _logger = logger;
            _durableHttpClient = durableHttpClient;
            _apiResourceConfigurations = apiResourceConfigurations;
        }

        /// <summary>
        /// APIにリクエストを送信し、顔データの追加処理を実行する
        /// </summary>
        /// <param name="location">Azureのリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video IndexerのアカウントID (GUID形式)</param>
        /// <param name="personModelId">Person ModelのID (GUID形式)</param>
        /// <param name="personId">PersonのID (GUID形式)</param>
        /// <param name="imageUrls">追加する顔画像のURLリスト</param>
        /// <param name="accessToken">オプションのアクセストークン (省略可能)</param>
        /// <returns>APIからのレスポンスJSON (成功時は追加された顔データのリスト)</returns>
        public async Task<string> FetchPersonFacesJsonAsync(string location, string accountId, string personModelId, string personId, List<string> imageUrls, string? accessToken = null)
        {
            try
            {
                // APIエンドポイントのURLを組み立てる
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}/Faces";

                // アクセストークンが指定されている場合、URLに追加
                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                // リクエストボディとして送信するJSONデータを作成
                var requestData = new { urls = imageUrls };
                var jsonContent = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

                // APIにPOSTリクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.PostAsync(requestUrl, jsonContent);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                response.EnsureSuccessStatusCode(); // HTTPステータスコードがエラーの場合、例外をスロー

                // レスポンスの内容を取得し、JSONとして返す
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}"); // HTTPリクエストのエラーをログに記録
                throw;
            }
        }

        /// <summary>
        /// JSONレスポンスをパースして、顔データのリストを取得する
        /// </summary>
        /// <param name="jsonResponse">APIから取得したJSONレスポンス</param>
        /// <returns>顔データのリスト (失敗時はnull)</returns>
        public List<string>? ParsePersonFacesJson(string jsonResponse)
        {
            try
            {
                // JSONをパースしてList<string>として返す
                return JsonSerializer.Deserialize<List<string>>(jsonResponse);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}"); // JSONパースエラーをログに記録
                throw;
            }
        }

        /// <summary>
        /// 顔データをAPIに登録する
        /// </summary>
        /// <param name="location">Azureのリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video IndexerのアカウントID (GUID形式)</param>
        /// <param name="personModelId">Person ModelのID (GUID形式)</param>
        /// <param name="personId">PersonのID (GUID形式)</param>
        /// <param name="imageUrls">追加する顔画像のURLリスト</param>
        /// <param name="accessToken">オプションのアクセストークン (省略可能)</param>
        /// <returns>成功時は追加された顔データのリスト、エラー時は例外をスロー</returns>
        public async Task<List<string>?> CreatePersonFacesAsync(string location, string accountId, string personModelId, string personId, List<string> imageUrls, string? accessToken = null)
        {
            try
            {
                // APIからJSONレスポンスを取得
                var jsonResponse = await FetchPersonFacesJsonAsync(location, accountId, personModelId, personId, imageUrls, accessToken);

                // JSONをパースして、顔データのリストを返す
                return ParsePersonFacesJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}"); // 予期しないエラーをログに記録
                throw;
            }
        }

        /// <summary>
        /// API へリクエストを送信し、新しい Person を作成する
        /// </summary>
        public async Task<string> FetchCreatePersonJsonAsync(
            string location, string accountId, string personModelId, string? name = null, string? description = null, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons";

                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
                if (!string.IsNullOrEmpty(description)) queryParams.Add($"description={Uri.EscapeDataString(description)}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    requestUrl += "?" + string.Join("&", queryParams);
                }

                // API に対して POST リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.PostAsync(requestUrl, null);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// JSON レスポンスを解析し、新しく作成された Person の情報を取得する
        /// </summary>
        public ApiPersonModel? ParseCreatePersonJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiPersonModel>(jsonResponse);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、新しい Person を作成する
        /// </summary>
        public async Task<ApiPersonModel?> CreatePersonAsync(string location, string accountId, string personModelId, string? name = null, string? description = null, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await FetchCreatePersonJsonAsync(location, accountId, personModelId, name, description, accessToken);
                return ParseCreatePersonJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API にリクエストを送信し、新しい Person Model を作成する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="name">作成する Person Model の名前 (オプション)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>API からの JSON レスポンス文字列</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        private async Task<string> FetchCreatePersonModelJsonAsync(
            string location, string accountId, string? name = null, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels";

                var queryParams = new List<string>();
                if (!string.IsNullOrEmpty(name)) queryParams.Add($"name={Uri.EscapeDataString(name)}");
                if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

                if (queryParams.Count > 0)
                {
                    requestUrl += "?" + string.Join("&", queryParams);
                }

                // API に対して POST リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.PostAsync(requestUrl, null);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                response.EnsureSuccessStatusCode(); // HTTP ステータスコードがエラーなら例外をスロー

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// JSON レスポンスを解析し、新しく作成された Person Model の情報を取得する
        /// </summary>
        /// <param name="jsonResponse">API から取得した JSON レスポンス</param>
        /// <returns>作成された Person Model の情報を含む ApiCustomPersonModel オブジェクト</returns>
        /// <exception cref="JsonException">JSON の解析に失敗した場合</exception>
        private ApiCustomPersonModel? ParseCreatePersonModelJson(string jsonResponse)
        {
            try
            {
                return JsonSerializer.Deserialize<ApiCustomPersonModel>(jsonResponse);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、新しい Person Model を作成する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="name">作成する Person Model の名前 (オプション)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>作成された Person Model の情報を含む ApiCustomPersonModel オブジェクト</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="JsonException">JSON の解析に失敗した場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<ApiCustomPersonModel?> CreatePersonModelAsync(
            string location, string accountId, string? name = null, string? accessToken = null)
        {
            try
            {
                var jsonResponse = await FetchCreatePersonModelJsonAsync(location, accountId, name, accessToken);
                return ParseCreatePersonModelJson(jsonResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
                throw;
            }
        }
    }
}

