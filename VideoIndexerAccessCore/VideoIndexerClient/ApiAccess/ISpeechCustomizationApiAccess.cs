using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ISpeechCustomizationApiAccess
{
    /// <summary>
    /// API からスピーチデータセット作成の JSON データを取得します。
    /// Create Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>API から取得した JSON 文字列</returns>
    Task<string> FetchCreateSpeechDatasetJsonAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null);

    /// <summary>
    /// 取得した JSON をパースして ApiSpeechDatasetUpdateModel オブジェクトに変換します。
    /// Create Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースした ApiSpeechDatasetUpdateModel オブジェクト、エラー時は null</returns>
    ApiModel.ApiSpeechDatasetResponseModel? ParseSpeechDatasetResponseJson(string json);

    /// <summary>
    /// API を呼び出してスピーチデータセットを作成します。
    /// Create Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>作成したスピーチデータセット情報、エラー時は null</returns>
    Task<ApiModel.ApiSpeechDatasetResponseModel?> CreateSpeechDatasetAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null);

    /// <summary>
    /// API からスピーチモデル作成の JSON データを取得します。
    /// Create Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="request">スピーチモデルのリクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>API から取得した JSON 文字列</returns>
    Task<string> FetchCreateSpeechModelJsonAsync(string location, string accountId, ApiSpeechModelRequestModel request, string? accessToken = null);

    /// <summary>
    /// API を呼び出してスピーチモデルを作成します。
    /// Create Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="request">スピーチモデルのリクエストオブジェクト</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>作成したスピーチモデル情報、エラー時は null</returns>
    Task<ApiSpeechModelResponseModel?> CreateSpeechModelAsync(string location, string accountId, ApiSpeechModelRequestModel request, string? accessToken = null);

    /// <summary>
    /// JSON をパースして ApiSpeechModelResponseModel オブジェクトに変換します。
    /// Create Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>パースした ApiSpeechModelResponseModel オブジェクト、エラー時は null</returns>
    ApiSpeechModelResponseModel? ParseSpeechModelJson(string json);

    /// <summary>
    /// API からスピーチデータセット削除リクエストを送信します。
    /// Delete Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Dataset
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="datasetId">削除するデータセットの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>削除成功時は true、失敗時は false</returns>
    Task<bool> DeleteSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null);

    /// <summary>
    /// API からスピーチモデル削除リクエストを送信します。
    /// Delete Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Model
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="modelId">削除するスピーチモデルの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>削除成功時は true、失敗時は false</returns>
    Task<bool> DeleteSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// API からスピーチデータセットを取得します。
    /// Get Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="datasetId">取得するデータセットの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>スピーチデータセット情報を含む ApiSpeechDatasetUpdateModel オブジェクト</returns>
    Task<ApiModel.ApiSpeechDatasetModel?> GetSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null);

    /// <summary>
    /// API から JSON を取得するメソッド。
    /// Get Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="datasetId">取得するデータセットの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
    Task<string?> FetchSpeechDatasetJsonAsync(string location, string accountId, string datasetId, string? accessToken);

    /// <summary>
    /// JSON を ApiSpeechDatasetUpdateModel オブジェクトにパースするメソッド。
    /// Get Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
    /// </summary>
    /// <param name="jsonContent">JSON 形式のレスポンス</param>
    /// <returns>パースした ApiSpeechDatasetUpdateModel オブジェクト。パースに失敗した場合は null。</returns>
    ApiModel.ApiSpeechDatasetModel? ParseSpeechDatasetJson(string jsonContent);

    /// <summary>
    /// API からスピーチデータセットのファイル一覧を取得します。
    /// Get Speech Dataset Files
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="datasetId">取得するデータセットの ID</param>
    /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>スピーチデータセットのファイルリストを含むリスト。取得できなかった場合は null。</returns>
    Task<List<ApiSpeechDatasetFileModel>?> GetSpeechDatasetFilesAsync(string location, string accountId, string datasetId, int? sasValidityInSeconds = null, string? accessToken = null);

    /// <summary>
    /// API から JSON を取得するメソッド。
    /// Get Speech Dataset Files
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="datasetId">取得するデータセットの ID</param>
    /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
    Task<string?> FetchSpeechDatasetFilesJsonAsync(string location, string accountId, string datasetId, int? sasValidityInSeconds, string? accessToken);

    /// <summary>
    /// JSON を ApiSpeechDatasetFileModel のリストにパースするメソッド。
    /// Get Speech Dataset Files
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
    /// </summary>
    /// <param name="jsonContent">JSON 形式のレスポンス</param>
    /// <returns>パースした ApiSpeechDatasetFileModel のリスト。パースに失敗した場合は null。</returns>
    List<ApiSpeechDatasetFileModel>? ParseSpeechDatasetFilesJson(string jsonContent);

    /// <summary>
    /// API からスピーチデータセット一覧を取得します。
    /// Get Speech Datasets
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="locale">取得するデータセットのロケール（省略可、null の場合はすべて取得）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>スピーチデータセットのリスト。取得できなかった場合は null。</returns>
    Task<List<ApiModel.ApiSpeechDatasetModel>?> GetSpeechDatasetsAsync(string location, string accountId, string? locale = null, string? accessToken = null);

    /// <summary>
    /// API から JSON を取得するメソッド。
    /// Get Speech Datasets
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="locale">取得するデータセットのロケール（省略可）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
    Task<string?> FetchSpeechDatasetsJsonAsync(string location, string accountId, string? locale, string? accessToken);

    /// <summary>
    /// JSON を List＜ApiSpeechDatasetUpdateModel＞? にパースするメソッド。
    /// Get Speech Datasets
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
    /// </summary>
    /// <param name="jsonContent">JSON 形式のレスポンス</param>
    /// <returns>パースしたスピーチデータセットのリスト。パースに失敗した場合は null。</returns>
    List<ApiModel.ApiSpeechDatasetModel>? ParseSpeechDatasetsJson(string jsonContent);

    /// <summary>
    /// API からスピーチモデルを取得します。
    /// Get Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="modelId">取得するスピーチモデルの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>スピーチモデル情報。取得できなかった場合は null。</returns>
    Task<ApiModel.ApiCustomSpeechModel?> GetSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// API から JSON を取得するメソッド。
    /// Get Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="modelId">取得するスピーチモデルの ID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>JSON 形式のレスポンスを文字列として返す。取得できなかった場合は null。</returns>
    Task<string?> FetchSpeechModelJsonAsync(string location, string accountId, string modelId, string? accessToken);

    /// <summary>
    /// JSON を ApiCustomSpeechModel にパースするメソッド。
    /// Get Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="jsonContent">JSON 形式のレスポンス</param>
    /// <returns>パースしたスピーチモデル情報。パースに失敗した場合は null。</returns>
    ApiModel.ApiCustomSpeechModel? ParseCustomSpeechModelJson(string jsonContent);

    /// <summary>
    /// APIからスピーチモデルを取得します。
    /// Get Speech Models
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>取得した <see cref="ApiModel.ApiCustomSpeechModel"/> を含む非同期タスク。</returns>
    Task<ApiModel.ApiCustomSpeechModel?> GetSpeechModelsAsync(string location, string accountId, string? locale = null, string? accessToken = null);

    /// <summary>
    /// スピーチモデルAPIからJSONレスポンスを取得します。
    /// Get Speech Models
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>APIから取得した生のJSONレスポンス（文字列）。</returns>
    Task<string> FetchJsonAsync(string location, string accountId, string? locale, string? accessToken);

    /// <summary>
    /// スピーチモデルを取得するためのリクエストURIを構築します。
    /// Get Speech Models
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>構築されたリクエストURI（文字列）。</returns>
    string BuildRequestUri(string location, string accountId, string? locale, string? accessToken);

    /// <summary>
    /// JSONレスポンスを <see cref="ApiModel.ApiCustomSpeechModel"/> オブジェクトにパースします。
    /// Get Speech Models
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
    /// </summary>
    /// <param name="jsonResponse">APIから取得したJSONレスポンス。</param>
    /// <returns>パースされた <see cref="ApiModel.ApiCustomSpeechModel"/> オブジェクト。</returns>
    ApiModel.ApiCustomSpeechModel? ParseSpeechModel(string jsonResponse);

    /// <summary>
    /// スピーチデータセットを更新します。
    /// Update Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="datasetId">更新するスピーチデータセットのID。</param>
    /// <param name="displayName">更新するデータセットの表示名。</param>
    /// <param name="description">更新するデータセットの説明。</param>
    /// <param name="customProperties">更新するデータセットのカスタムプロパティ。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>更新された <see cref="ApiSpeechDatasetUpdateModel"/> を含む非同期タスク。</returns>
    Task<ApiSpeechDatasetUpdateModel?> UpdateSpeechDatasetAsync(string location, string accountId, string datasetId, string? displayName, string? description, Dictionary<string, string>? customProperties, string? accessToken = null);

    /// <summary>
    /// スピーチデータセットを更新します。
    /// Update Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="datasetId">更新するスピーチデータセットのID。</param>
    /// <param name="updateRequestModelData">更新するデータセットの情報。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>更新された <see cref="ApiSpeechDatasetUpdateModel"/> を含む非同期タスク。</returns>
    Task<ApiSpeechDatasetUpdateModel?> UpdateSpeechDatasetAsync(string location, string accountId, string datasetId, ApiSpeechDatasetUpdateRequestModel updateRequestModelData, string? accessToken = null);

    /// <summary>
    /// スピーチデータセットの更新APIを呼び出し、JSONレスポンスを取得します。
    /// Update Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="datasetId">更新対象のスピーチデータセットのID（GUID）。</param>
    /// <param name="updateRequestModelData">更新内容を含むオブジェクト。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>APIのレスポンスとして返されるJSON文字列。</returns>
    Task<string> FetchUpdateJsonAsync(string location, string accountId, string datasetId, ApiSpeechDatasetUpdateRequestModel updateRequestModelData, string? accessToken);

    /// <summary>
    /// スピーチモデルの更新APIを呼び出し、JSONレスポンスを取得します。
    /// Update Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
    /// <param name="modelId">更新対象のスピーチモデルのID（GUID）。</param>
    /// <param name="updateRequestModelData">更新内容を含むオブジェクト。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
    /// <returns>APIのレスポンスとして返されるJSON文字列。</returns>
    Task<string> FetchUpdateJsonAsync(string location, string accountId, string modelId, ApiCustomSpeechModelUpdateRequestModel updateRequestModelData, string? accessToken);

    /// <summary>
    /// APIから取得したスピーチデータセットのJSONレスポンスを解析し、SpeechDatasetオブジェクトに変換します。
    /// Update Speech Dataset
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset
    /// </summary>
    /// <param name="jsonResponse">APIから返されたJSON文字列。</param>
    /// <returns>解析されたSpeechDatasetオブジェクト。</returns>
    ApiSpeechDatasetUpdateModel? ParseSpeechDataset(string jsonResponse);

    /// <summary>
    /// スピーチモデルを更新します。
    /// Update Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。例: "westus"。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。例: "123e4567-e89b-12d3-a456-426614174000"。</param>
    /// <param name="modelId">更新するスピーチモデルのID（GUID）。</param>
    /// <param name="displayName">更新するモデルの表示名。</param>
    /// <param name="description">更新するモデルの説明。</param>
    /// <param name="customProperties">更新するモデルのカスタムプロパティ。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。指定しない場合、デフォルトの認証が使用される。</param>
    /// <returns>更新されたスピーチモデル情報を含む非同期タスク。</returns>
    Task<ApiCustomSpeechUpdateModel?> UpdateSpeechModelAsync(string location, string accountId, string modelId, string? displayName, string? description, Dictionary<string, string>? customProperties, string? accessToken = null);

    /// <summary>
    /// スピーチモデルを更新します。
    /// Update Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="location">リクエストをルーティングするAzureリージョン。例: "westus"。</param>
    /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。例: "123e4567-e89b-12d3-a456-426614174000"。</param>
    /// <param name="modelId">更新するスピーチモデルのID（GUID）。</param>
    /// <param name="updateRequestModelData">更新するモデルの情報（表示名、説明、カスタムプロパティなど）。</param>
    /// <param name="accessToken">（オプション）認証のためのアクセストークン。指定しない場合、デフォルトの認証が使用される。</param>
    /// <returns>更新されたスピーチモデル情報を含む非同期タスク。</returns>
    Task<ApiCustomSpeechUpdateModel?> UpdateSpeechModelAsync(string location, string accountId, string modelId, ApiCustomSpeechModelUpdateRequestModel updateRequestModelData, string? accessToken = null);

    /// <summary>
    /// APIから取得したスピーチモデルのJSONレスポンスを解析し、ApiCustomSpeechUpdateModelオブジェクトに変換します。
    /// Update Speech Model
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
    /// </summary>
    /// <param name="jsonResponse">APIから返されたJSON文字列。</param>
    /// <returns>解析されたCustomSpeechModelオブジェクト。</returns>
    ApiCustomSpeechUpdateModel? ParseSpeechModels(string jsonResponse);
}