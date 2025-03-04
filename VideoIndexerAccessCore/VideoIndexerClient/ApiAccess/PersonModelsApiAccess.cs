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
        public async Task<string> FetchCreatePersonModelJsonAsync(
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
        public ApiCustomPersonModel? ParseCreatePersonModelJson(string jsonResponse)
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

        /// <summary>
        /// API を呼び出し、指定された Face を削除する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">Person の ID (GUID 形式)</param>
        /// <param name="faceId">削除する Face の ID (GUID 形式)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Face が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<bool> DeleteCustomFaceAsync(string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}/Faces/{faceId}";

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                // API に対して DELETE リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

                var response = await httpClient.DeleteAsync(requestUrl);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response is { IsSuccessStatusCode: true, StatusCode: System.Net.HttpStatusCode.NoContent })
                    // ステータスコード 204 (No Content) は削除成功
                {
                    return true;
                }

                // ステータスコードによるエラーハンドリング
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError("Face not found.");
                    throw new KeyNotFoundException("The specified face was not found.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized access.");
                    throw new UnauthorizedAccessException("Access token is not authorized.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogError("Too many requests. Please try again later.");
                    throw new Exception("Too many requests. Please retry later.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Internal server error occurred.");
                    throw new Exception("An internal server error occurred.");
                }

                _logger.LogError($"Unexpected response: {response.StatusCode}");
                throw new Exception($"Unexpected response from the server: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person を削除する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">削除する Person の ID (GUID 形式)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Person が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<bool> DeletePersonAsync(
            string location, string accountId, string personModelId, string personId, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}";

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                // API に対して DELETE リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.DeleteAsync(requestUrl);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response.IsSuccessStatusCode)
                {
                    // ステータスコード 204 (No Content) は削除成功
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return true;
                    }
                }

                // ステータスコードによるエラーハンドリング
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError("Person not found.");
                    throw new KeyNotFoundException("The specified person was not found.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized access.");
                    throw new UnauthorizedAccessException("Access token is not authorized.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogError("Too many requests. Please try again later.");
                    throw new Exception("Too many requests. Please retry later.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Internal server error occurred.");
                    throw new Exception("An internal server error occurred.");
                }

                _logger.LogError($"Unexpected response: {response.StatusCode}");
                throw new Exception($"Unexpected response from the server: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person Model を削除する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="personModelId">削除する Person Model の ID (GUID 形式)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Person Model が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<bool> DeletePersonModelAsync(
            string location, string accountId, string personModelId, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}";

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                // API に対して DELETE リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.DeleteAsync(requestUrl);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response is { IsSuccessStatusCode: true, StatusCode: System.Net.HttpStatusCode.NoContent })
                    // ステータスコード 204 (No Content) は削除成功
                {
                    return true;
                }

                // ステータスコードによるエラーハンドリング
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError("Person Model not found.");
                    throw new KeyNotFoundException("The specified Person Model was not found.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized access.");
                    throw new UnauthorizedAccessException("Access token is not authorized.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogError("Too many requests. Please try again later.");
                    throw new Exception("Too many requests. Please retry later.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Internal server error occurred.");
                    throw new Exception("An internal server error occurred.");
                }

                _logger.LogError($"Unexpected response: {response.StatusCode}");
                throw new Exception($"Unexpected response from the server: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Face の画像を取得する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">Person の ID (GUID 形式)</param>
        /// <param name="faceId">取得する Face の ID (GUID 形式)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>取得した Face Picture の URL を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Face Picture が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<string> GetCustomFacePictureAsync(
            string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null)
        {
            try
            {
                // API エンドポイント URL の構築
                var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}/Faces/{faceId}";

                if (!string.IsNullOrEmpty(accessToken))
                {
                    requestUrl += $"?accessToken={accessToken}";
                }

                // API に対して GET リクエストを送信
                HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
                var response = await httpClient.GetAsync(requestUrl);

                // responseがnullなら例外を
                if (response is null) throw new HttpRequestException("The response was null.");

                if (response.IsSuccessStatusCode)
                {
                    // ステータスコード 200 (OK) の場合、画像の URL を取得
                    return await response.Content.ReadAsStringAsync();
                }

                // ステータスコードによるエラーハンドリング
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    _logger.LogError("Face picture not found.");
                    throw new KeyNotFoundException("The specified face picture was not found.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    _logger.LogError("Unauthorized access.");
                    throw new UnauthorizedAccessException("Access token is not authorized.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    _logger.LogError("Too many requests. Please try again later.");
                    throw new Exception("Too many requests. Please retry later.");
                }
                if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                {
                    _logger.LogError("Internal server error occurred.");
                    throw new Exception("An internal server error occurred.");
                }

                _logger.LogError($"Unexpected response: {response.StatusCode}");
                throw new Exception($"Unexpected response from the server: {response.StatusCode}");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError($"HTTP request error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API から Face の情報を取得し、JSON を返す
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID (GUID)</param>
        /// <param name="personModelId">Person Model の ID (GUID)</param>
        /// <param name="personId">Person の ID (GUID)</param>
        /// <param name="pageSize">取得する Face の数 (オプション)</param>
        /// <param name="skip">スキップする件数 (オプション)</param>
        /// <param name="sourceType">Face のソースタイプ (UploadedPicture / UploadedVideo, オプション)</param>
        /// <param name="accessToken">アクセストークン (オプション)</param>
        /// <returns>取得した JSON 文字列</returns>
        public async Task<string> FetchCustomFacesJsonAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null)
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}/Faces";

            var queryParams = new List<string>();
            if (pageSize.HasValue) queryParams.Add($"pageSize={pageSize.Value}");
            if (skip.HasValue) queryParams.Add($"skip={skip.Value}");
            if (!string.IsNullOrEmpty(sourceType)) queryParams.Add($"sourceType={sourceType}");
            if (!string.IsNullOrEmpty(accessToken)) queryParams.Add($"accessToken={accessToken}");

            if (queryParams.Count > 0)
            {
                requestUrl += "?" + string.Join("&", queryParams);
            }

            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            var response = await httpClient.GetAsync(requestUrl);

            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

            _logger.LogError($"API request failed: {response.StatusCode}");
            throw new HttpRequestException($"Unexpected response from the server: {response.StatusCode}");

        }

        /// <summary>
        /// JSON をパースし、Face のリストを返す
        /// </summary>
        /// <param name="json">API から取得した JSON 文字列</param>
        /// <returns>Face のリスト</returns>
        public List<ApiFaceModel> ParseCustomFacesJson(string json)
        {
            try
            {
                var faceResponse = JsonSerializer.Deserialize<ApiListFacesResponseModel>(json);
                return faceResponse?.Results ?? new List<ApiFaceModel>();
            }
            catch (JsonException ex)
            {
                _logger.LogError($"JSON parsing error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person に紐づく Face 情報を取得する
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID (GUID)</param>
        /// <param name="personModelId">Person Model の ID (GUID)</param>
        /// <param name="personId">Person の ID (GUID)</param>
        /// <param name="pageSize">取得する Face の数 (オプション)</param>
        /// <param name="skip">スキップする件数 (オプション)</param>
        /// <param name="sourceType">Face のソースタイプ (UploadedPicture / UploadedVideo, オプション)</param>
        /// <param name="accessToken">アクセストークン (オプション)</param>
        /// <returns>Face のリスト</returns>
        public async Task<List<ApiFaceModel>> GetCustomFacesAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null)
        {
            var json = await FetchCustomFacesJsonAsync(location, accountId, personModelId, personId, pageSize, skip, sourceType, accessToken);
            return ParseCustomFacesJson(json);
        }

        /// <summary>
        /// 指定された人物のすべての顔のスプライト画像のURLを取得します。
        /// </summary>
        /// <param name="location">APIのロケーション (例: "trial")</param>
        /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
        /// <param name="personModelId">人物モデルのID (GUID)</param>
        /// <param name="personId">人物ID (GUID)</param>
        /// <param name="pageSize">取得する結果の数 (オプション)</param>
        /// <param name="skip">スキップする結果の数 (オプション)</param>
        /// <param name="sourceType">顔のソースタイプ (UploadedPicture / UploadedVideo) (オプション)</param>
        /// <param name="accessToken">APIのアクセストークン (オプション)</param>
        /// <returns>スプライト画像のURL</returns>
        public async Task<string> GetCustomFacesSpriteAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string sourceType = null, string accessToken = null)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels/{personModelId}/Persons/{personId}/Faces/sprite";

                var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (pageSize.HasValue) queryParams["pageSize"] = pageSize.Value.ToString();
                if (skip.HasValue) queryParams["skip"] = skip.Value.ToString();
                if (!string.IsNullOrEmpty(sourceType)) queryParams["sourceType"] = sourceType;
                if (!string.IsNullOrEmpty(accessToken)) queryParams["accessToken"] = accessToken;

                if (queryParams.Count > 0)
                {
                    url += "?" + queryParams.ToString();
                }

                _logger.LogInformation("Fetching custom faces sprite from API: {Url}", url);
                string jsonResponse = await FetchApiResponseAsync(url);
                return ParseSpriteResponse(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw;
            }
        }

        /// <summary>
        /// APIからJSONレスポンスを取得する非同期メソッド。
        /// </summary>
        /// <param name="url">APIのURL</param>
        /// <returns>APIのJSONレスポンス</returns>
        public async Task<string> FetchApiResponseAsync(string url)
        {
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            using var response = await httpClient.GetAsync(url);

            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSONレスポンスを解析し、スプライト画像のURLを取得します。
        /// </summary>
        /// <param name="jsonResponse">APIのJSONレスポンス</param>
        /// <returns>スプライト画像のURL</returns>
        public string ParseSpriteResponse(string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse))
            {
                _logger.LogWarning("Empty response received from API.");
                return string.Empty;
            }
            return jsonResponse.Trim('"'); // JSON内のURLが文字列として返るため、クオートを削除
        }

        /// <summary>
        /// 指定されたアカウント内のすべての人物モデルを取得します。
        /// </summary>
        /// <param name="location">APIのロケーション (例: "trial")</param>
        /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
        /// <param name="personNamePrefix">検索する人物の名前のプレフィックス (オプション)</param>
        /// <param name="nameFilter">名前フィルター (オプション)</param>
        /// <param name="accessToken">APIのアクセストークン (オプション)</param>
        /// <returns>人物モデルのリスト</returns>
        public async Task<List<ApiCustomPersonModel>> GetPersonModelsAsync(
            string location,
            string accountId,
            string personNamePrefix = null,
            string nameFilter = null,
            string accessToken = null)
        {
            try
            {
                string url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/PersonModels";

                var queryParams = System.Web.HttpUtility.ParseQueryString(string.Empty);
                if (!string.IsNullOrEmpty(personNamePrefix)) queryParams["personNamePrefix"] = personNamePrefix;
                if (!string.IsNullOrEmpty(nameFilter)) queryParams["nameFilter"] = nameFilter;
                if (!string.IsNullOrEmpty(accessToken)) queryParams["accessToken"] = accessToken;

                if (queryParams.Count > 0)
                {
                    url += "?" + queryParams.ToString();
                }

                _logger.LogInformation("Fetching person models from API: {Url}", url);
                string jsonResponse = await FetchApiPersonModelsResponseAsync(url);
                return ParsePersonModelsResponse(jsonResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");
                throw;
            }
        }

        /// <summary>
        /// APIからJSONレスポンスを取得する非同期メソッド。
        /// </summary>
        /// <param name="url">APIのURL</param>
        /// <returns>APIのJSONレスポンス</returns>
        private async Task<string> FetchApiPersonModelsResponseAsync(string url)
        {
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            using var response = await httpClient.GetAsync(url);

            // responseがnullなら例外を
            if (response is null) throw new HttpRequestException("The response was null.");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// JSONレスポンスを解析し、人物モデルのリストを取得します。
        /// </summary>
        /// <param name="jsonResponse">APIのJSONレスポンス</param>
        /// <returns>人物モデルのリスト</returns>
        private List<ApiCustomPersonModel> ParsePersonModelsResponse(string jsonResponse)
        {
            if (string.IsNullOrEmpty(jsonResponse))
            {
                _logger.LogWarning("Empty response received from API.");
                return new List<ApiCustomPersonModel>();
            }

            try
            {
                return JsonSerializer.Deserialize<List<ApiCustomPersonModel>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ApiCustomPersonModel>();
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to parse JSON response.");
                throw;
            }
        }
    }
}

