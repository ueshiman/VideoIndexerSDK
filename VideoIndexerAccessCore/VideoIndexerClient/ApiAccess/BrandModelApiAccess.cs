using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;

// ブランドモデルを作成するAPIを呼び出すクラス
namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

/// <summary>
/// ブランドモデルを作成するAPIを呼び出すクラス
/// </summary>
public class BrandModelApiAccess : IBrandModelApiAccess
{
    // HTTPリクエストを送信するためのクライアント
    //private readonly HttpClient _httpClient;
    // ログを記録するためのロガー
    private readonly ILogger<BrandModelApiAccess> _logger;
    private readonly IDurableHttpClient? _durableHttpClient;
    private readonly IApiResourceConfigurations _apiResourceConfigurations;



    // 指定された情報を基に新しいブランドモデルを作成する非同期メソッド
    public BrandModelApiAccess(ILogger<BrandModelApiAccess> logger, IDurableHttpClient? durableHttpClient, IApiResourceConfigurations apiResourceConfigurations)
    {
        _logger = logger;
        _durableHttpClient = durableHttpClient;
        _apiResourceConfigurations = apiResourceConfigurations;
    }

    /// <summary>
    /// 指定された情報を基に新しいブランドモデルを作成する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="brand">作成するブランドモデルの情報</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    public async Task<HttpResponseMessage> CreateApiBrandModelAsync(string location, string accountId, string? accessToken, ApiBrandModel brand)
    {
        HttpResponseMessage? response;
        try
        {
            // APIのエンドポイントを構築
            var url = $"https://{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/ApiBrandModels" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);
            // ブランドモデルのデータをJSON形式にシリアライズ
            var jsonContent = JsonSerializer.Serialize(brand);
            // HTTPリクエストのコンテンツを作成
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            // リクエスト情報をログ出力
            _logger.LogInformation("Sending request to {Url} with payload {Payload}", url, jsonContent);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            // APIリクエストを送信し、レスポンスを取得
            response = await httpClient.PostAsync(url, content);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        // responseがnullなら例外を発生
        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        // responseがnullなら例外を
        if (response == null) throw new HttpRequestException("The response was null.");

        // ステータスコードをチェック、成功ならレスポンスを返す
        if (response.IsSuccessStatusCode) return response;
        // 失敗した場合はエラーハンドリング
        _logger.LogError("Throwing HttpRequestException due to unsuccessful status code: {StatusCode}", response.StatusCode);
        var errorContent = await response.Content.ReadAsStringAsync(); // ここでエラーメッセージを取得
        throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");

    }

    /// <summary>
    /// 指定されたブランドモデルを削除する非同期メソッド
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">削除するブランドのID</param>
    /// <param name="accessToken">認証トークン（オプション）</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    public async Task<HttpResponseMessage> DeleteApiBrandModelJsonAsync(string location, string accountId, int id, string? accessToken)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Brands/{id}" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            _logger.LogInformation("Sending DELETE request to {Url}", url);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.DeleteAsync(url);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);


        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        // responseがnullなら例外を発生
        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        if (response.IsSuccessStatusCode) return response;

        var errorContent = await response.Content.ReadAsStringAsync();
        _logger.LogError("Throwing HttpRequestException due to unsuccessful status code: {StatusCode}", response.StatusCode);
        throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
    }

    /// <summary>
    /// 指定されたブランドを取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    public async Task<string> GetApiBrandModelJsonAsync(string location, string accountId, int id, string? accessToken)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Brands/{id}" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            _logger.LogInformation("Sending GET request to {Url}", url);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();

            response = await httpClient.GetAsync(url);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);


        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        if (response is null) throw new HttpRequestException("The response was null.");

        if (response.IsSuccessStatusCode)
        {
            var jsonResponse = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"Successfully listed videos: {jsonResponse}");
            return jsonResponse;
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        _logger.LogError("Failed to get brand: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
        throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
    }

    /// <summary>
    /// JSONレスポンスを ApiBrandModel にパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiBrandModel オブジェクト</returns>
    public ApiBrandModel? ParseApiBrandModel(string jsonResponse)
    {
        try
        {
            return JsonSerializer.Deserialize<ApiBrandModel>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError("JSONデシリアライズエラー: {Message}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// 指定されたブランドを取得し、ApiBrandModel にパースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデルのオブジェクト</returns>
    public async Task<ApiBrandModel?> GetApiBrandModeAsync(string location, string accountId, int id, string? accessToken)
    {
        var jsonResponse = await GetApiBrandModelJsonAsync(location, accountId, id, accessToken);
        return ParseApiBrandModel(jsonResponse);
    }

    /// <summary>
    /// 指定されたブランドを取得し、ApiBrandModel にパースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデルのリスト</returns>
    public async Task<ApiBrandModel[]?> GetApiBrandsAsync(string location, string accountId, string? accessToken)
    {
        var jsonResponse = await GetApiBrandsJsonAsync(location, accountId, accessToken);
        return ParseApiBrandsJson(jsonResponse);
    }
    /// <summary>
    /// APIからブランドモデルのJSONを取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ブランドモデルのJSON文字列</returns>
    public async Task<string> GetApiBrandsJsonAsync(string location, string accountId, string? accessToken)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Brands" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            _logger.LogInformation("Sending GET request to {Url}", url);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.GetAsync(url);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        // responseがnullなら例外を発生
        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get brands: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
        }

        return await response.Content.ReadAsStringAsync();
    }
    /// <summary>
    /// ブランドモデルのJSONをパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">ブランドモデルのJSON文字列</param>
    /// <returns>パースされたブランドモデルの配列</returns>
    public ApiBrandModel[]? ParseApiBrandsJson(string jsonResponse)
    {
        try
        {
            _logger.LogInformation("Parsing JSON response");
            return JsonSerializer.Deserialize<ApiBrandModel[]>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError("JSON deserialization error: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to deserialize JSON response.", ex);
        }
    }

    /// <summary>
    /// ブランドモデル設定をJSONで取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したJSONレスポンス</returns>
    public async Task<string> GetApiBrandModelSettingsJsonAsync(string location, string accountId, string? accessToken)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/BrandsModelSettings" +
                      (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            _logger.LogInformation("Sending GET request to {Url}", url);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.GetAsync(url);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to get brand model settings: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
        }

        try
        {
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while reading response content: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to read response content.", ex);
        }
    }

    /// <summary>
    /// JSONレスポンスを ApiBrandModelSettingsModel にパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiBrandModelSettingsModel オブジェクト</returns>
    public ApiBrandModelSettingsModel? ParseApiBrandModelSettingsJson(string jsonResponse)
    {
        try
        {
            return JsonSerializer.Deserialize<ApiBrandModelSettingsModel>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            _logger.LogError("JSON deserialization error: {Message}", ex.Message);
            return null;
        }
    }

    /// <summary>
    /// ブランドモデル設定を取得し、パースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデル設定オブジェクト</returns>
    public async Task<ApiBrandModelSettingsModel?> FetchAndParseApiBrandModelSettingsAsync(string location, string accountId, string? accessToken)
    {
        var jsonResponse = await GetApiBrandModelSettingsJsonAsync(location, accountId, accessToken);
        return ParseApiBrandModelSettingsJson(jsonResponse);
    }

    /// <summary>
    /// ブランドを更新する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="brand">更新するブランド情報</param>
    /// <returns>更新されたブランドのJSONレスポンス</returns>
    public async Task<string> UpdateApiBrandModelAsync(string location, string accountId, int id, string? accessToken, ApiBrandModel brand)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/Brands/{id}" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            var jsonContent = JsonSerializer.Serialize(brand);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending PUT request to {Url} with payload {Payload}", url, jsonContent);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.PutAsync(url, content);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);

        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        // responseがnullなら例外を発生
        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to update brand: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
        }

        try
        {
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while reading response content: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to read response content.", ex);
        }
    }

    /// <summary>
    /// ブランドモデル設定を更新する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="settings">更新するブランドモデル設定</param>
    /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
    public async Task<string> UpdateApiBrandModelSettingsAsync(string location, string accountId, string? accessToken, ApiBrandModelSettingsModel settings)
    {
        HttpResponseMessage? response;
        try
        {
            var url = $"{_apiResourceConfigurations.ApiEndpoint}/{location}/Accounts/{accountId}/Customization/BrandsModelSettings" +
                      (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);

            var jsonContent = JsonSerializer.Serialize(settings);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending PUT request to {Url} with payload {Payload}", url, jsonContent);
            HttpClient httpClient = _durableHttpClient?.HttpClient ?? new HttpClient();
            response = await httpClient.PutAsync(url, content);
            _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);

        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred: {Message}", ex.Message);
            throw;
        }

        // responseがnullなら例外を発生
        if (response == null)
        {
            _logger.LogError("Response is null");
            throw new HttpRequestException("Response is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Failed to update brand model settings: {StatusCode} - {ErrorContent}", response.StatusCode, errorContent);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");
        }

        try
        {
            return await response.Content.ReadAsStringAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occurred while reading response content: {Message}", ex.Message);
            throw new InvalidOperationException("Failed to read response content.", ex);
        }
    }
}


// 使用例
/*
/// <summary>
/// APIを実行するプログラムのエントリーポイント
/// </summary>
public class Program
{
    /// <summary>
    /// プログラムのエントリーポイント
    /// </summary>
    /// <param name="args">コマンドライン引数</param>
    public static async Task Main(string[] args)
    {
        var location = "your-location";
        var accountId = "your-account-id";
        var accessToken = "your-access-token";

        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });
        var logger = loggerFactory.CreateLogger<BrandModelApiAccess>();

        var apiClient = new BrandModelApiAccess(logger);
        var newApiBrandModel = new ApiBrandModel
        {
            ReferenceUrl = "https://www.myserver.com/pictures/picture_1",
            Id = 9945,
            Name = "MyApiBrandModel",
            AccountId = accountId,
            LastModifierUserName = "John Snow",
            Create = DateTime.UtcNow,
            LastModified = DateTime.UtcNow,
            Enabled = true,
            Description = "brand description",
            Tags = new[] { "tag1", "tag2" }
        };

        var response = await apiClient.CreateApiBrandModelAsync(location, accountId, accessToken, newApiBrandModel);
        Console.WriteLine($"Response: {response.StatusCode}");
    }
}
*/