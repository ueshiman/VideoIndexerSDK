using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Runtime.Versioning;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

/// <summary>
/// Azure Video Indexer API クライアント
/// 指定されたモデルのトレーニングをキャンセルする機能を提供する。
/// </summary>
public class ClassicLanguageCustomizationApiAccess : IClassicLanguageCustomizationApiAccess
{
    private readonly ILogger<BrandsApiAccess> _logger;
    private readonly IDurableHttpClient? _durableHttpClient;
    private readonly IApiResourceConfigurations _apiResourceConfigurations;

    /// <summary>
    /// ClassicLanguageCustomizationApiAccess のインスタンスを作成
    /// </summary>
    public ClassicLanguageCustomizationApiAccess(ILogger<BrandsApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
    {
        _logger = logger;
        _durableHttpClient = durableHttpClient;
        _apiResourceConfigurations = apiResourceConfigurations;
    }

    /// <summary>
    /// トレーニングモデルをキャンセルする
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <param name="modelId">キャンセル対象のモデルID</param>
    /// <returns>APIからのレスポンス</returns>
    /// <exception cref="ArgumentException">modelId が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<HttpResponseMessage> CancelTrainingModelAsync(string location, string accountId, string accessToken, string modelId)
    {
        if (string.IsNullOrEmpty(modelId)) throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));

        // APIエンドポイントのURLを生成
        string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}/Train?accessToken={accessToken}";

        using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;

        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            // APIにDELETEリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }

        // responseがnullなら例外を
        if (response == null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (response.IsSuccessStatusCode) return response;

        var errorMessage = await response.Content.ReadAsStringAsync();

        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
    }

    /// <summary>
    /// 新しいカスタム言語モデルを作成し、作成結果をパースして返す。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="requestModel">作成するカスタム言語モデルのリクエスト情報</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>作成されたカスタム言語モデルの情報</returns>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<ApiCustomLanguageModel> CreateLanguageModelAsync(string location, string accountId, ApiCustomLanguageRequestModel requestModel, string? accessToken = null)
    {
        HttpResponseMessage message = await CreateLanguageModelAsync(location, accountId, requestModel.ModelName, requestModel.Language, accessToken);

        if (message.IsSuccessStatusCode)
        {
            string jsonResponse = await message.Content.ReadAsStringAsync();
            return ParseLanguageModelJson(jsonResponse);
        }
        else
        {
            string errorMessage = await message.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to create language model: {errorMessage}");
        }
    }


    /// <summary>
    /// 新しい言語モデルを作成する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <param name="modelName">作成するモデルの名前</param>
    /// <param name="language">モデルの言語コード</param>
    /// <returns>APIからのレスポンス</returns>
    /// <exception cref="ArgumentException">modelName または language が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<HttpResponseMessage> CreateLanguageModelAsync(string location, string accountId, string modelName, string language, string? accessToken = null)
    {
        if (string.IsNullOrEmpty(modelName)) throw new ArgumentException("Model name cannot be null or empty.", nameof(modelName));
        if (string.IsNullOrEmpty(language)) throw new ArgumentException("Language cannot be null or empty.", nameof(language));

        // APIエンドポイントのURLを生成
        string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language?modelName={modelName}&language={language}" + (accessToken != null ? $"&accessToken={accessToken}" : "");

        using var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            // APIにPOSTリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }

        // responseがnullなら例外を
        if (response == null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (response.IsSuccessStatusCode) return response;

        string errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
    }

    /// <summary>
    /// 言語モデルを削除する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="modelId">削除するモデルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>APIからのレスポンス</returns>
    /// <exception cref="ArgumentException">modelId が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<HttpResponseMessage> DeleteLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null)
    {
        if (string.IsNullOrEmpty(modelId)) throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));

        // APIエンドポイントのURLを生成
        string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            // APIにDELETEリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (response.IsSuccessStatusCode) return response;

        var errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
    }

    /// <summary>
    /// 言語モデルファイルを削除する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="modelId">モデルのID</param>
    /// <param name="fileId">削除するファイルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>APIからのレスポンス</returns>
    /// <exception cref="ArgumentException">modelId または fileId が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<HttpResponseMessage> DeleteLanguageModelFileAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null)
    {
        if (string.IsNullOrEmpty(modelId))
        {
            throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));
        }
        if (string.IsNullOrEmpty(fileId))
        {
            throw new ArgumentException("File ID cannot be null or empty.", nameof(fileId));
        }

        // APIエンドポイントのURLを生成
        var requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}/Files/{fileId}" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        using var request = new HttpRequestMessage(HttpMethod.Delete, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            // APIにDELETEリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }


        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (response.IsSuccessStatusCode) return response;

        string errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
    }

    /// <summary>
    /// 言語モデルファイルのコンテンツをダウンロードする
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="modelId">モデルのID</param>
    /// <param name="fileId">ダウンロードするファイルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>APIからのレスポンス（ファイルコンテンツ）</returns>
    /// <exception cref="ArgumentException">modelId または fileId が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<HttpResponseMessage> DownloadLanguageModelFileContentAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null)
    {
        if (string.IsNullOrEmpty(modelId)) throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));
        if (string.IsNullOrEmpty(fileId)) throw new ArgumentException("File ID cannot be null or empty.", nameof(fileId));

        // APIエンドポイントのURLを生成
        string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}/Files/{fileId}/download" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            // APIにGETリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (response.IsSuccessStatusCode) return response;

        string errorMessage = await response.Content.ReadAsStringAsync();
        throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");

    }

    /// <summary>
    /// 言語モデルを取得する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="modelId">取得するモデルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>APIからのレスポンス（言語モデル情報）</returns>
    /// <exception cref="ArgumentException">modelId が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    public async Task<string> GetLanguageModelJsonAsync(string location, string accountId, string modelId, string? accessToken = null)
    {
        if (string.IsNullOrEmpty(modelId))
        {
            throw new ArgumentException("Model ID cannot be null or empty.", nameof(modelId));
        }

        // APIエンドポイントのURLを生成
        string requestUri = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString()); // 一意のリクエストIDを追加

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            // APIにGETリクエストを送信
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            throw new HttpRequestException("An error occurred while sending the request.", ex);
        }
        catch (TaskCanceledException ex)
        {
            throw new HttpRequestException("The request was canceled or timed out.", ex);
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        // レスポンスが成功ステータスでない場合は例外をスロー
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorMessage}");
        }

        string responseData = await response.Content.ReadAsStringAsync();
        return responseData;
    }

    /// <summary>
    /// 言語モデルを取得しパースする
    /// </summary>
    public async Task<ApiCustomLanguageModel> GetLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null)
    {
        string json = await GetLanguageModelJsonAsync(location, accountId, modelId, accessToken);
        return ParseLanguageModelJson(json);
    }

    /// <summary>
    /// JSONレスポンスを ApiCustomLanguageModel にパースする
    /// </summary>
    /// <param name="json">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiCustomLanguageModel オブジェクト</returns>
    /// <exception cref="JsonException">JSONのパースに失敗した場合にスローされる</exception>
    public ApiCustomLanguageModel ParseLanguageModelJson(string json)
    {
        return JsonSerializer.Deserialize<ApiCustomLanguageModel>(json) ?? throw new JsonException("Failed to parse JSON response.");
    }

    /// <summary>
    /// Retrieves the language model edits history.
    /// </summary>
    /// <remarks>
    /// This method is in preview and may change in future releases.
    /// </remarks>
    [RequiresPreviewFeatures]
    public async Task<List<ApiLanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string location, string accountId, string modelId, string accessToken)
    {
        try
        {
            // Fetch JSON data from API
            var jsonData = await FetchEditsHistoryJsonAsync(location, accountId, modelId, accessToken);

            // Deserialize JSON into objects
            return DeserializeEditsHistory(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve language model edits history.");
            throw;
        }
    }

    /// <summary>
    /// Fetches the JSON data of language model edits history from API.
    /// </summary>
    [RequiresPreviewFeatures]
    public async Task<string> FetchEditsHistoryJsonAsync(string location, string accountId, string modelId, string? accessToken = null)
    {
        var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}/edits" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            throw new HttpRequestException("An unexpected error occurred.", ex);
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("API Error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
        throw new HttpRequestException($"API Error: {response.StatusCode}");
    }

    /// <summary>
    /// Deserializes JSON data into ApiLanguageModelEditModel objects.
    /// </summary>
    public List<ApiLanguageModelEditModel>? DeserializeEditsHistory(string jsonData)
    {
        try
        {
            return JsonSerializer.Deserialize<List<ApiLanguageModelEditModel>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON.");
            return null;
        }
    }

    /// <summary>
    /// 言語モデルのファイルデータを取得する
    /// </summary>
    [RequiresPreviewFeatures] // このAPIはプレビュー版
    public async Task<ApiLanguageModelFileDataModel?> GetLanguageModelFileDataAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null)
    {
        try
        {
            var jsonData = await GetLanguageModelFileDataJsonAsync($"Customization/Language/{modelId}/Files/{fileId}", location, accountId, accessToken);
            return DeserializeLanguageModelFileData(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve language model file data.");
            throw;
        }
    }

    /// <summary>
    /// APIからJSONデータを取得する
    /// </summary>
    public async Task<string> GetLanguageModelFileDataJsonAsync(string path, string location, string accountId, string? accessToken = null)
    {
        var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/{path}" + (accessToken != null ? $"?accessToken={accessToken}" : "");

        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (!response.IsSuccessStatusCode)
        {
            var errorResponse = await response.Content.ReadAsStringAsync();
            _logger.LogError("API Error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
            throw new HttpRequestException($"API Error: {response.StatusCode}");
        }

        return await response.Content.ReadAsStringAsync();
    }
    /// <summary>
    /// 言語モデルのファイルデータをデシリアライズする
    /// </summary>
    public ApiLanguageModelFileDataModel? DeserializeLanguageModelFileData(string jsonData)
    {
        try
        {
            return JsonSerializer.Deserialize<ApiLanguageModelFileDataModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize language model file data.");
            return null;
        }
    }

    /// <summary>
    /// 言語モデルの一覧を取得する
    /// </summary>
    public async Task<List<ApiLanguageModelDataModel>?> GetLanguageModelsAsync(string location, string accountId, string accessToken)
    {
        try
        {
            // API から JSON を取得
            var jsonData = await FetchJsonAsync(location, accountId, accessToken);

            // JSON をデシリアライズ
            return DeserializeLanguageModels(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve language models.");
            throw;
        }
    }

    /// <summary>
    /// APIからJSONデータを取得する
    /// </summary>
    public async Task<string> FetchJsonAsync(string location, string accountId, string accessToken)
    {
        var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language?accessToken={accessToken}";

        var request = new HttpRequestMessage(HttpMethod.Get, requestUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("API error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
        throw new HttpRequestException($"API error: {response.StatusCode}");
    }

    /// <summary>
    /// 言語モデルの一覧データをデシリアライズする
    /// </summary>
    public List<ApiLanguageModelDataModel>? DeserializeLanguageModels(string jsonData)
    {
        try
        {
            return JsonSerializer.Deserialize<List<ApiLanguageModelDataModel>>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize language models.");
            return null;
        }
    }

    /// <summary>
    /// 言語モデルをトレーニングする
    /// </summary>
    public async Task<ApiLanguageModelDataModel?> TrainLanguageModelAsync(string location, string accountId, string modelId, string accessToken)
    {
        try
        {
            var jsonData = await PostJsonAsync($"Customization/Language/{modelId}/Train", location, accountId, accessToken);
            return DeserializeLanguageModel(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to train language model.");
            throw;
        }
    }

    /// <summary>
    /// API に POST リクエストを送信する
    /// </summary>
    public async Task<string> PostJsonAsync(string location, string accountId, string modelId, string accessToken)
    {
        var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}/Train?accessToken={accessToken}";

        var request = new HttpRequestMessage(HttpMethod.Post, requestUrl);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("API error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
        throw new HttpRequestException($"API error: {response.StatusCode}");

    }

    /// <summary>
    /// 言語モデルのデータをデシリアライズする
    /// </summary>
    public ApiLanguageModelDataModel? DeserializeLanguageModel(string jsonData)
    {
        try
        {
            return JsonSerializer.Deserialize<ApiLanguageModelDataModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize language model.");
            return null;
        }
    }

    /// <summary>
    /// 言語モデルを更新し、オプションでファイルをアップロードする。
    /// </summary>
    /// <param name="location">APIリクエストを送信するAzureリージョン</param>
    /// <param name="accountId">グローバルに一意なアカウント識別子</param>
    /// <param name="modelId">グローバルに一意なモデル識別子</param>
    /// <param name="accessToken">認証のためのアクセストークン</param>
    /// <param name="modelName">新しいモデル名（オプション）</param>
    /// <param name="enable">モデルのファイルを有効化/無効化するフラグ（オプション）</param>
    /// <param name="fileUrls">リモートファイルのURLリスト（オプション）</param>
    /// <param name="filePaths">ローカルファイルのパスリスト（オプション）</param>
    /// <returns>更新された言語モデルのデータ</returns>
    public async Task<ApiLanguageModelDataModel?> UpdateLanguageModelAsync(string location, string accountId, string modelId, string accessToken, string? modelName = null, bool? enable = null, Dictionary<string, string>? fileUrls = null, Dictionary<string, string>? filePaths = null)
    {
        try
        {
            var jsonData = await PutFormDataAsync(location, accountId, modelId, accessToken, modelName, enable, fileUrls, filePaths);
            return DeserializeLanguageModel(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "言語モデルの更新に失敗しました。");
            throw;
        }
    }

    /// <summary>
    /// マルチパートフォームデータを含むPUTリクエストを送信し、言語モデルを更新する。
    /// </summary>
    /// <param name="location">APIリクエストを送信するAzureリージョン</param>
    /// <param name="accountId">グローバルに一意なアカウント識別子</param>
    /// <param name="modelId">グローバルに一意なモデル識別子</param>
    /// <param name="accessToken">認証のためのアクセストークン</param>
    /// <param name="modelName">新しいモデル名（オプション）</param>
    /// <param name="enable">モデルのファイルを有効化/無効化するフラグ（オプション）</param>
    /// <param name="fileUrls">リモートファイルのURLリスト（オプション）</param>
    /// <param name="filePaths">ローカルファイルのパスリスト（オプション）</param>
    /// <returns>APIレスポンスのJSON文字列</returns>
    public async Task<string> PutFormDataAsync(string location, string accountId, string modelId, string accessToken, string? modelName, bool? enable, Dictionary<string, string>? fileUrls, Dictionary<string, string>? filePaths)
    {
        using var formData = new MultipartFormDataContent();

        // モデル名の追加
        if (!string.IsNullOrEmpty(modelName)) formData.Add(new StringContent(modelName), "modelName");

        // 有効化/無効化フラグの追加
        if (enable.HasValue) formData.Add(new StringContent(enable.Value.ToString().ToLower()), "enable");

        // リモートファイルURLの追加
        if (fileUrls != null) foreach (var file in fileUrls) formData.Add(new StringContent(file.Value), file.Key);

        // ローカルファイルの追加
        if (filePaths != null)
        {
            foreach (var file in filePaths)
            {
                var stream = File.OpenRead(file.Value);
                var streamContent = new StreamContent(stream);
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                formData.Add(streamContent, file.Key, Path.GetFileName(file.Value));
            }
        }

        var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Language/{modelId}?accessToken={accessToken}";
        var request = new HttpRequestMessage(HttpMethod.Put, requestUrl) { Content = formData };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

        HttpResponseMessage? response;
        try
        {
            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("API error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
        throw new HttpRequestException($"API error: {response.StatusCode}");
    }

    /// <summary>
    /// 言語モデルのファイルを更新するメソッド。
    /// 指定されたファイルの名前を変更したり、有効/無効の状態を切り替えることができます。
    /// </summary>
    /// <param name="location">Azureリージョンの指定</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">対象の言語モデルのID</param>
    /// <param name="fileId">更新するファイルのID</param>
    /// <param name="accessToken">認証に使用するアクセストークン（オプション）</param>
    /// <param name="fileName">新しいファイル名（オプション、指定がない場合は変更なし）</param>
    /// <param name="enable">ファイルを有効化/無効化するフラグ（オプション）</param>
    /// <returns>更新されたファイルの情報を含むオブジェクト</returns>
    public async Task<ApiLanguageModelFileDataModel?> UpdateLanguageModelFileAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null, string? fileName = null, bool? enable = null)
    {
        try
        {
            var jsonData = await FetchLanguageModelFileJsonAsync(location, accountId, modelId, fileId, accessToken, fileName, enable);
            return ParseLanguageModelFileJson(jsonData);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to update language model file.");
            throw;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse JSON response.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            throw;
        }
    }

    /// <summary>
    /// API から指定された言語モデルファイルの JSON データを取得する。
    /// </summary>
    public async Task<string> FetchLanguageModelFileJsonAsync(string location, string accountId, string modelId, string fileId, string? accessToken, string? fileName, bool? enable)
    {
        HttpResponseMessage? response;
        try
        {
            var requestUrl = $"{_apiResourceConfigurations.ApiEndpoint}{location}/Accounts/{accountId}/Customization/Language/{modelId}/Files/{fileId}?accessToken={accessToken ?? ""}";

            if (!string.IsNullOrEmpty(fileName)) requestUrl += $"&fileName={Uri.EscapeDataString(fileName)}";

            if (enable.HasValue) requestUrl += $"&enable={enable.Value.ToString().ToLower()}";

            var request = new HttpRequestMessage(HttpMethod.Put, requestUrl);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-ms-client-request-id", Guid.NewGuid().ToString());

            var httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.SendAsync(request);

        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP request failed.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during API request.");
            throw;
        }

        // responseがnullなら例外を
        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode) return await response.Content.ReadAsStringAsync();

        var errorResponse = await response.Content.ReadAsStringAsync();
        _logger.LogError("API error: {StatusCode}, Response: {Response}", response.StatusCode, errorResponse);
        throw new HttpRequestException($"API error: {response.StatusCode}");

    }

    /// <summary>
    /// 取得した JSON データをパースし、言語モデルファイルの情報をオブジェクトとして返す。
    /// </summary>
    public ApiLanguageModelFileDataModel? ParseLanguageModelFileJson(string jsonData)
    {
        try
        {
            return JsonSerializer.Deserialize<ApiLanguageModelFileDataModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize JSON response.");
            throw;
        }
    }

}
