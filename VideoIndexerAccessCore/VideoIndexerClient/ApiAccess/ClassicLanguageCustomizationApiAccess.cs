using Azure;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

/// <summary>
/// Azure Video Indexer API クライアント
/// 指定されたモデルのトレーニングをキャンセルする機能を提供する。
/// </summary>
public class ClassicLanguageCustomizationApiAccess
{
    private readonly ILogger<BrandModelApiAccess> _logger;
    private readonly IDurableHttpClient? _durableHttpClient;
    private readonly IApiResourceConfigurations _apiResourceConfigurations;

    /// <summary>
    /// ClassicLanguageCustomizationApiAccess のインスタンスを作成
    /// </summary>
    public ClassicLanguageCustomizationApiAccess(ILogger<BrandModelApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
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

    private ApiCustomLanguageModel ParseLanguageModelJson(string json)
    {
        return JsonSerializer.Deserialize<ApiCustomLanguageModel>(json) ?? throw new JsonException("Failed to parse JSON response.");
    }
}




