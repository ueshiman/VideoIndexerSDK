using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IPersonModelsApiAccess
{
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
    Task<string> FetchPersonFacesJsonAsync(string location, string accountId, string personModelId, string personId, List<string> imageUrls, string? accessToken = null);

    /// <summary>
    /// JSONレスポンスをパースして、顔データのリストを取得する
    /// </summary>
    /// <param name="jsonResponse">APIから取得したJSONレスポンス</param>
    /// <returns>顔データのリスト (失敗時はnull)</returns>
    List<string>? ParsePersonFacesJson(string jsonResponse);

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
    Task<List<string>?> CreatePersonFacesAsync(string location, string accountId, string personModelId, string personId, List<string> imageUrls, string? accessToken = null);

    /// <summary>
    /// API へリクエストを送信し、新しい Person を作成する
    /// </summary>
    Task<string> FetchCreatePersonJsonAsync(
        string location, string accountId, string personModelId, string? name = null, string? description = null, string? accessToken = null);

    /// <summary>
    /// JSON レスポンスを解析し、新しく作成された Person の情報を取得する
    /// </summary>
    ApiPersonModel? ParseCreatePersonJson(string jsonResponse);

    /// <summary>
    /// API を呼び出し、新しい Person を作成する
    /// </summary>
    Task<ApiPersonModel?> CreatePersonAsync(string location, string accountId, string personModelId, string? name = null, string? description = null, string? accessToken = null);

    /// <summary>
    /// API にリクエストを送信し、新しい Person Model を作成する
    /// </summary>
    /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
    /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
    /// <param name="name">作成する Person Model の名前 (オプション)</param>
    /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
    /// <returns>API からの JSON レスポンス文字列</returns>
    /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
    Task<string> FetchCreatePersonModelJsonAsync(
        string location, string accountId, string? name = null, string? accessToken = null);

    /// <summary>
    /// JSON レスポンスを解析し、新しく作成された Person Model の情報を取得する
    /// </summary>
    /// <param name="jsonResponse">API から取得した JSON レスポンス</param>
    /// <returns>作成された Person Model の情報を含む ApiCustomPersonModel オブジェクト</returns>
    /// <exception cref="JsonException">JSON の解析に失敗した場合</exception>
    ApiCustomPersonModel? ParseCreatePersonModelJson(string jsonResponse);

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
    Task<ApiCustomPersonModel?> CreatePersonModelAsync(
        string location, string accountId, string? name = null, string? accessToken = null);

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
    Task<bool> DeleteCustomFaceAsync(string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null);

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
    Task<bool> DeletePersonAsync(
        string location, string accountId, string personModelId, string personId, string? accessToken = null);

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
    Task<bool> DeletePersonModelAsync(
        string location, string accountId, string personModelId, string? accessToken = null);

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
    Task<string> GetCustomFacePictureAsync(
        string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null);

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
    Task<string> FetchCustomFacesJsonAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null);

    /// <summary>
    /// JSON をパースし、Face のリストを返す
    /// </summary>
    /// <param name="json">API から取得した JSON 文字列</param>
    /// <returns>Face のリスト</returns>
    List<ApiFaceModel> ParseCustomFacesJson(string json);

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
    Task<List<ApiFaceModel>> GetCustomFacesAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null);

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
    Task<string> GetCustomFacesSpriteAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null);

    /// <summary>
    /// APIからJSONレスポンスを取得する非同期メソッド。
    /// </summary>
    /// <param name="url">APIのURL</param>
    /// <returns>APIのJSONレスポンス</returns>
    Task<string> FetchApiResponseAsync(string? url);

    /// <summary>
    /// JSONレスポンスを解析し、スプライト画像のURLを取得します。
    /// </summary>
    /// <param name="jsonResponse">APIのJSONレスポンス</param>
    /// <returns>スプライト画像のURL</returns>
    string ParseSpriteResponse(string jsonResponse);

    /// <summary>
    /// 指定されたアカウント内のすべての人物モデルを取得します。
    /// </summary>
    /// <param name="location">APIのロケーション (例: "trial")</param>
    /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
    /// <param name="personNamePrefix">検索する人物の名前のプレフィックス (オプション)</param>
    /// <param name="nameFilter">名前フィルター (オプション)</param>
    /// <param name="accessToken">APIのアクセストークン (オプション)</param>
    /// <returns>人物モデルのリスト</returns>
    Task<List<ApiCustomPersonModel>> GetPersonModelsAsync(string location, string accountId, string? personNamePrefix = null, string? nameFilter = null, string? accessToken = null);

    /// <summary>
    /// APIからJSONレスポンスを取得する非同期メソッド。
    /// </summary>
    /// <param name="url">APIのURL</param>
    /// <returns>APIのJSONレスポンス</returns>
    Task<string> FetchApiPersonModelsResponseAsync(string url);

    /// <summary>
    /// JSONレスポンスを解析し、人物モデルのリストを取得します。
    /// </summary>
    /// <param name="jsonResponse">APIのJSONレスポンス</param>
    /// <returns>人物モデルのリスト</returns>
    List<ApiCustomPersonModel> ParsePersonModelsResponse(string jsonResponse);

    /// <summary>
    /// 指定された人物モデル内で指定のプレフィックスを持つすべての人物を取得します。
    /// </summary>
    /// <param name="location">APIのロケーション (例: "trial")</param>
    /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
    /// <param name="personModelId">人物モデルのID (GUID)</param>
    /// <param name="namePrefix">フィルター対象の名前のプレフィックス (オプション)</param>
    /// <param name="nameFilter">フィルター条件 (オプション)</param>
    /// <param name="pageSize">取得する結果の数 (オプション)</param>
    /// <param name="skip">スキップする結果の数 (オプション)</param>
    /// <param name="sort">ソート条件 ('name', '-score' など) (オプション)</param>
    /// <param name="accessToken">APIのアクセストークン (オプション)</param>
    /// <returns>人物情報のリスト</returns>
    Task<List<ApiPersonModel>> GetPersonsAsync(string location, string accountId, string personModelId, string? namePrefix = null, string? nameFilter = null, int? pageSize = null, int? skip = null, string? sort = null, string? accessToken = null);

    /// <summary>
    /// APIからJSONレスポンスを取得する非同期メソッド。
    /// </summary>
    /// <param name="url">APIのURL</param>
    /// <returns>APIのJSONレスポンス</returns>
    Task<string> FetchApiPersonsResponseAsync(string url);

    /// <summary>
    /// JSONレスポンスを解析し、人物のリストを取得します。
    /// </summary>
    /// <param name="jsonResponse">APIのJSONレスポンス</param>
    /// <returns>人物情報のリスト</returns>
    List<ApiPersonModel> ParsePersonsResponse(string jsonResponse);

    /// <summary>
    /// 指定された人物モデルの名前や識別閾値を更新します。
    /// </summary>
    /// <param name="location">APIのロケーション (例: "trial")</param>
    /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
    /// <param name="personModelId">人物モデルのID (GUID)</param>
    /// <param name="newName">更新する新しいモデル名 (オプション)</param>
    /// <param name="personIdentificationThreshold">人物識別閾値 (0.0 - 1.0) (オプション)</param>
    /// <param name="accessToken">APIのアクセストークン (オプション)</param>
    /// <returns>更新された人物モデル情報</returns>
    Task<ApiCustomPersonModel?> PatchPersonModelAsync(string location, string accountId, string personModelId, string? newName = null, double? personIdentificationThreshold = null, string? accessToken = null);

    /// <summary>
    /// APIにPATCHリクエストを送信し、JSONレスポンスを取得します。
    /// </summary>
    /// <param name="location">APIのロケーション</param>
    /// <param name="accountId">ビデオインデクサーのアカウントID</param>
    /// <param name="personModelId">人物モデルのID</param>
    /// <param name="newName">更新する新しいモデル名 (オプション)</param>
    /// <param name="personIdentificationThreshold">人物識別閾値 (0.0 - 1.0) (オプション)</param>
    /// <param name="accessToken">APIのアクセストークン</param>
    /// <returns>APIのJSONレスポンス</returns>
    Task<string> FetchApiPatchResponseAsync(string location, string accountId, string personModelId, string? newName = null, double? personIdentificationThreshold = null, string? accessToken = null);

    /// <summary>
    /// JSONレスポンスを解析し、更新された人物モデル情報を取得します。
    /// </summary>
    /// <param name="jsonResponse">APIのJSONレスポンス</param>
    /// <returns>更新された人物モデル情報</returns>
    ApiCustomPersonModel? ParsePatchPersonModelResponse(string jsonResponse);

    /// <summary>
    /// Video Indexer API で人物情報を更新します。
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="personModelId">人物モデルの一意の識別子。</param>
    /// <param name="personId">人物の一意の識別子。</param>
    /// <param name="name">任意の新しい名前。</param>
    /// <param name="description">任意の説明。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>更新が成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
    Task<ApiPersonModel?> UpdatePersonAsync(string location, string accountId, string personModelId, string personId, string? name = null, string? description = null, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に PUT リクエストを送信し、JSON レスポンスを取得します。
    /// </summary>
    /// <returns>JSON レスポンスを文字列として返します。</returns>
    Task<string> SendPutRequestAsync(string location, string accountId, string personModelId, string personId, string? name, string? description, string? accessToken);

    /// <summary>
    /// JSON 文字列を ApiPersonModel オブジェクトにパースします。
    /// </summary>
    /// <param name="json">パースする JSON 文字列。</param>
    /// <returns>パースに成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
    ApiPersonModel? ParsePersonJson(string json);

    /// <summary>
    /// Video Indexer API で人物モデルを更新します。
    /// </summary>
    /// <param name="location">API 呼び出しの Azure リージョン。</param>
    /// <param name="accountId">アカウントの一意の識別子。</param>
    /// <param name="personModelId">人物モデルの一意の識別子。</param>
    /// <param name="name">任意の新しい名前。</param>
    /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
    /// <returns>更新が成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
    Task<ApiPersonModel?> UpdatePersonModelAsync(string location, string accountId, string personModelId, string? name = null, string? accessToken = null);

    /// <summary>
    /// Video Indexer API に PUT リクエストを送信し、JSON レスポンスを取得します。
    /// </summary>
    /// <returns>JSON レスポンスを文字列として返します。</returns>
    Task<string> SendPutRequestForPersonModelAsync(string location, string accountId, string personModelId, string? name, string? accessToken);

    /// <summary>
    /// JSON 文字列を ApiPersonModel オブジェクトにパースします。
    /// </summary>
    /// <param name="json">パースする JSON 文字列。</param>
    /// <returns>パースに成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
    ApiPersonModel? ParsePersonModelsJson(string json);
}