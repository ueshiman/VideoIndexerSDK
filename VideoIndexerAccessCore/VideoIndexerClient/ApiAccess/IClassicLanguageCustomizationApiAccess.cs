using System.Text.Json;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IClassicLanguageCustomizationApiAccess
{
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
    Task<HttpResponseMessage> CancelTrainingModelAsync(string location, string accountId, string accessToken, string modelId);


    /// <summary>
    /// 新しいカスタム言語モデルを作成する
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Azure Video Indexer のアカウントID</param>
    /// <param name="requestModel">作成リクエストのモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）</param>
    /// <returns>作成されたカスタム言語モデル</returns>
    /// <exception cref="ArgumentException">location, accountId, または requestModel が null または空の場合にスローされる</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合にスローされる</exception>
    Task<ApiCustomLanguageModel> CreateLanguageModelAsync(string location, string accountId, ApiCustomLanguageRequestModel requestModel, string? accessToken = null);


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
    Task<HttpResponseMessage> CreateLanguageModelAsync(string location, string accountId, string modelName, string language, string? accessToken = null);

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
    Task<HttpResponseMessage> DeleteLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null);

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
    Task<HttpResponseMessage> DeleteLanguageModelFileAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null);

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
    Task<HttpResponseMessage> DownloadLanguageModelFileContentAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null);

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
    Task<string> GetLanguageModelJsonAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// 言語モデルを取得しパースする
    /// </summary>
    Task<ApiCustomLanguageModel> GetLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// JSONレスポンスを ApiCustomLanguageModel にパースする
    /// </summary>
    /// <param name="json">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiCustomLanguageModel オブジェクト</returns>
    /// <exception cref="JsonException">JSONのパースに失敗した場合にスローされる</exception>
    ApiCustomLanguageModel ParseLanguageModelJson(string json);

    /// <summary>
    /// Retrieves the language model edits history.
    /// </summary>
    /// <remarks>
    /// This method is in preview and may change in future releases.
    /// </remarks>
    Task<List<ApiLanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// Fetches the JSON data of language model edits history from API.
    /// </summary>
    Task<string> FetchEditsHistoryJsonAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// Deserializes JSON data into ApiLanguageModelEditModel objects.
    /// </summary>
    List<ApiLanguageModelEditModel>? DeserializeEditsHistory(string jsonData);

    /// <summary>
    /// 言語モデルのファイルデータを取得する
    /// </summary>
    // このAPIはプレビュー版
    Task<ApiLanguageModelFileDataModel?> GetLanguageModelFileDataAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null);

    /// <summary>
    /// APIからJSONデータを取得する
    /// </summary>
    Task<string> GetLanguageModelFileDataJsonAsync(string path, string location, string accountId, string? accessToken = null);

    /// <summary>
    /// 言語モデルのファイルデータをデシリアライズする
    /// </summary>
    ApiLanguageModelFileDataModel? DeserializeLanguageModelFileData(string jsonData);

    /// <summary>
    /// 言語モデルの一覧を取得する
    /// </summary>
    Task<List<ApiLanguageModelDataModel>?> GetLanguageModelsAsync(string location, string accountId, string accessToken);

    /// <summary>
    /// APIからJSONデータを取得する
    /// </summary>
    Task<string> FetchJsonAsync(string location, string accountId, string accessToken);

    /// <summary>
    /// 言語モデルの一覧データをデシリアライズする
    /// </summary>
    List<ApiLanguageModelDataModel>? DeserializeLanguageModels(string jsonData);

    /// <summary>
    /// 言語モデルをトレーニングする
    /// </summary>
    Task<ApiLanguageModelDataModel?> TrainLanguageModelAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// API に POST リクエストを送信する
    /// </summary>
    Task<string> PostJsonAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// 言語モデルのデータをデシリアライズする
    /// </summary>
    ApiLanguageModelDataModel? DeserializeLanguageModel(string jsonData);

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
    Task<ApiLanguageModelDataModel?> UpdateLanguageModelAsync(string location, string accountId, string modelId, string accessToken, string? modelName = null, bool? enable = null, Dictionary<string, string>? fileUrls = null, Dictionary<string, string>? filePaths = null);

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
    Task<string> PutFormDataAsync(string location, string accountId, string modelId, string accessToken, string? modelName, bool? enable, Dictionary<string, string>? fileUrls, Dictionary<string, string>? filePaths);

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
    Task<ApiLanguageModelFileDataModel?> UpdateLanguageModelFileAsync(string location, string accountId, string modelId, string fileId, string? accessToken = null, string? fileName = null, bool? enable = null);

    /// <summary>
    /// API から指定された言語モデルファイルの JSON データを取得する。
    /// </summary>
    Task<string> FetchLanguageModelFileJsonAsync(string location, string accountId, string modelId, string fileId, string? accessToken, string? fileName, bool? enable);

    /// <summary>
    /// 取得した JSON データをパースし、言語モデルファイルの情報をオブジェクトとして返す。
    /// </summary>
    ApiLanguageModelFileDataModel? ParseLanguageModelFileJson(string jsonData);
}