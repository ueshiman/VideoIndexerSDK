using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IBrandModelApiAccess
{
    /// <summary>
    /// 指定された情報を基に新しいブランドモデルを作成する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="brand">作成するブランドモデルの情報</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    Task<HttpResponseMessage> CreateApiBrandModelAsync(string location, string accountId, string? accessToken, ApiBrandModel brand);

    /// <summary>
    /// 指定されたブランドモデルを削除する非同期メソッド
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">削除するブランドのID</param>
    /// <param name="accessToken">認証トークン（オプション）</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    Task<HttpResponseMessage> DeleteApiBrandModelJsonAsync(string location, string accountId, int id, string? accessToken);

    /// <summary>
    /// 指定されたブランドを取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>HTTPレスポンスメッセージ</returns>
    Task<string> GetApiBrandModelJsonAsync(string location, string accountId, int id, string? accessToken);

    /// <summary>
    /// JSONレスポンスを ApiBrandModel にパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiBrandModel オブジェクト</returns>
    ApiBrandModel? ParseApiBrandModel(string jsonResponse);

    /// <summary>
    /// 指定されたブランドを取得し、ApiBrandModel にパースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデルのオブジェクト</returns>
    Task<ApiBrandModel?> GetApiBrandModeAsync(string location, string accountId, int id, string? accessToken);

    /// <summary>
    /// 指定されたブランドを取得し、ApiBrandModel にパースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデルのリスト</returns>
    Task<ApiBrandModel[]?> GetApiBrandsAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// APIからブランドモデルのJSONを取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ブランドモデルのJSON文字列</returns>
    Task<string> GetApiBrandsJsonAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// ブランドモデルのJSONをパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">ブランドモデルのJSON文字列</param>
    /// <returns>パースされたブランドモデルの配列</returns>
    ApiBrandModel[]? ParseApiBrandsJson(string jsonResponse);

    /// <summary>
    /// ブランドモデル設定をJSONで取得する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したJSONレスポンス</returns>
    Task<string> GetApiBrandModelSettingsJsonAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// JSONレスポンスを ApiBrandModelSettingsModel にパースするメソッド
    /// </summary>
    /// <param name="jsonResponse">JSON形式のレスポンス</param>
    /// <returns>パースされた ApiBrandModelSettingsModel オブジェクト</returns>
    ApiBrandModelSettingsModel? ParseApiBrandModelSettingsJson(string jsonResponse);

    /// <summary>
    /// ブランドモデル設定を取得し、パースして返す非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>取得したブランドモデル設定オブジェクト</returns>
    Task<ApiBrandModelSettingsModel?> FetchAndParseApiBrandModelSettingsAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// ブランドを更新する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="brand">更新するブランド情報</param>
    /// <returns>更新されたブランドのJSONレスポンス</returns>
    Task<string> UpdateApiBrandModelAsync(string location, string accountId, int id, string? accessToken, ApiBrandModel brand);

    /// <summary>
    /// ブランドモデル設定を更新する非同期メソッド
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <param name="settings">更新するブランドモデル設定</param>
    /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
    Task<string> UpdateApiBrandModelSettingsAsync(string location, string accountId, string? accessToken, ApiBrandModelSettingsModel settings);
}