using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IIndexingApiAccess
{
    /// <summary>
    /// 特定の動画から顔情報を削除する。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Video Indexer アカウント ID</param>
    /// <param name="videoId">削除対象の動画 ID</param>
    /// <param name="faceId">削除対象の顔 ID</param>
    /// <param name="accessToken">(オプション) API アクセストークン</param>
    /// <returns>削除が成功した場合は true、それ以外は false</returns>
    Task<bool> DeleteVideoFaceAsync(string location, string accountId, string videoId, int faceId, string? accessToken = null);

    /// <summary>
    /// 指定された動画の再インデックスを実行する。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Video Indexer アカウント ID</param>
    /// <param name="videoId">対象の動画 ID</param>
    /// <param name="accessToken">API アクセストークン</param>
    /// <param name="excludedAI">除外する AI のリスト（カンマ区切り）</param>
    /// <param name="isSearchable">検索可能にするか</param>
    /// <param name="indexingPreset">インデックスプリセット</param>
    /// <param name="streamingPreset">ストリーミングプリセット</param>
    /// <param name="callbackUrl">コールバック URL</param>
    /// <param name="sourceLanguage">ソース言語</param>
    /// <param name="sendSuccessEmail">成功時にメールを送信するか</param>
    /// <param name="linguisticModelId">言語モデル ID</param>
    /// <param name="personModelId">人物モデル ID</param>
    /// <param name="priority">処理優先度</param>
    /// <param name="brandsCategories">ブランドカテゴリ</param>
    /// <param name="customLanguages">カスタム言語</param>
    /// <param name="logoGroupId">ロゴグループ ID</param>
    /// <param name="punctuationMode">句読点モード</param>
    /// <returns>成功時は true、それ以外は false</returns>
    Task<bool> ReIndexVideoAsync(string location, string accountId, string videoId, string? accessToken = null, List<string>? excludedAI = null, bool? isSearchable = null, string? indexingPreset = null, string? streamingPreset = null, string? callbackUrl = null, string? sourceLanguage = null, bool? sendSuccessEmail = null, string? linguisticModelId = null, string? personModelId = null, string? priority = null, string? brandsCategories = null, string? customLanguages = null, string? logoGroupId = null, string? punctuationMode = null);

    /// <summary>
    /// 指定された動画の再インデックスを実行する。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Video Indexer アカウント ID</param>
    /// <param name="videoId">対象の動画 ID</param>
    /// <param name="accessToken">API アクセストークン</param>
    /// <param name="parameters">クエリパラメータの辞書</param>
    /// <returns>成功時は true、それ以外は false</returns>
    Task<bool> _ReIndexVideoAsync(string location, string accountId, string videoId, string? accessToken, Dictionary<string, string>? parameters = null);

    /// <summary>
    /// 指定された動画の顔情報を更新する。
    /// </summary>
    /// <param name="location">Azure のリージョン（例: eastus, westeurope）</param>
    /// <param name="accountId">Video Indexer のアカウント ID（GUID）</param>
    /// <param name="videoId">更新対象の動画 ID</param>
    /// <param name="faceId">更新対象の顔 ID（整数）</param>
    /// <param name="newName">新しい名前（オプション）</param>
    /// <param name="personId">更新する人物 ID（GUID, オプション）</param>
    /// <param name="createNewPerson">新しい人物を作成するかどうか（オプション）</param>
    /// <param name="accessToken">API アクセストークン（オプション, 有効期限 1 時間）</param>
    /// <returns>更新成功時は true、それ以外は false</returns>
    Task<bool> UpdateVideoFaceAsync(string location, string accountId, string videoId, int faceId, string? newName = null, string? personId = null, bool? createNewPerson = null, string? accessToken = null);

    /// <summary>
    /// 指定されたパッチ操作を使用してビデオのインデックスを更新します。
    /// </summary>
    /// <param name="location">リクエストを送信する Azure リージョン。</param>
    /// <param name="accountId">Video Indexer サービスに関連付けられたアカウント ID。</param>
    /// <param name="videoId">インデックスを更新する対象のビデオ ID。</param>
    /// <param name="patchOperations">適用する JSON Patch 操作のリスト。</param>
    /// <param name="language">(オプション) インデックスを取得する言語。</param>
    /// <param name="accessToken">(オプション) 認証に必要なアクセストークン。</param>
    /// <returns>更新が成功した場合はレスポンスの JSON を返し、失敗した場合は null を返します。</returns>
    Task<string?> UpdateVideoIndexJsonAsync(
        string location,
        string accountId,
        string videoId,
        List<ApiPatchOperationModel> patchOperations,
        string? language = null,
        string? accessToken = null);

    /// <summary>
    /// UpdateVideoIndexJsonAsync のレスポンス JSON をプレーンクラスにパースするメソッド。
    /// </summary>
    /// <typeparam name="T">パース対象のプレーンクラスの型。</typeparam>
    /// <param name="json">JSON 文字列。</param>
    /// <returns>パースされたオブジェクト、または null (パース失敗時)。</returns>
    T? ParseVideoIndexResponse<T>(string json) where T : class;

    /// <summary>
    /// UpdateVideoIndexJsonAsync のレスポンス JSON を VideoIndexResponse にパースするメソッド。
    /// </summary>
    /// <param name="json">JSON 文字列。</param>
    /// <returns>パースされた VideoIndexResponse オブジェクト、または null (パース失敗時)。</returns>
    ApiVideoIndexResponseModel? ParseVideoIndexResponse(string json);

    /// <summary>
    /// ビデオインデックスの更新を行い、レスポンスをプレーンクラスに変換して返すメソッド。
    /// </summary>
    /// <param name="location">リクエストを送信する Azure リージョン。</param>
    /// <param name="accountId">Video Indexer サービスに関連付けられたアカウント ID。</param>
    /// <param name="videoId">インデックスを更新する対象のビデオ ID。</param>
    /// <param name="patchOperations">適用する JSON Patch 操作のリスト。</param>
    /// <param name="language">(オプション) インデックスを取得する言語。</param>
    /// <param name="accessToken">(オプション) 認証に必要なアクセストークン。</param>
    /// <returns>更新が成功した場合は VideoIndexResponse を返し、失敗した場合は null を返します。</returns>
    Task<ApiVideoIndexResponseModel?> UpdateVideoIndexAsync(string location, string accountId, string videoId, List<ApiPatchOperationModel> patchOperations, string? language = null, string? accessToken = null);

    /// <summary>
    /// 動画をアップロードし、インデックス処理を開始する
    /// </summary>
    /// <param name="location">Azure リージョン</param>
    /// <param name="accountId">アカウント ID</param>
    /// <param name="videoName">アップロードする動画の名前</param>
    /// <param name="videoStream">動画のストリーム</param>
    /// <param name="fileName">アップロードする動画のファイル名</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <param name="privacy">動画のプライバシーモード（Private/Public）</param>
    /// <param name="priority">処理の優先度（Low/Normal/High）</param>
    /// <param name="description">動画の説明</param>
    /// <param name="partition">動画のパーティション</param>
    /// <param name="externalId">外部 ID</param>
    /// <param name="externalUrl">外部 URL</param>
    /// <param name="callbackUrl">コールバック URL</param>
    /// <param name="metadata">動画のメタデータ</param>
    /// <param name="language">言語設定</param>
    /// <param name="videoUrl">動画の URL</param>
    /// <param name="indexingPreset">インデックスプリセット</param>
    /// <param name="streamingPreset">ストリーミングプリセット</param>
    /// <param name="personModelId">顔認識用のモデル ID</param>
    /// <param name="sendSuccessEmail">成功時のメール送信</param>
    /// <returns>アップロード結果の情報</returns>
    Task<ApiUploadVideoResponseModel?> UploadVideoAsync(string location, string accountId, string videoName, Stream videoStream, string fileName, string? accessToken = null, string? privacy = null, string? priority = null, string? description = null, string? partition = null, string? externalId = null, string? externalUrl = null, string? callbackUrl = null, string? metadata = null, string? language = null, string? videoUrl = null, string? indexingPreset = null, string? streamingPreset = null, string? personModelId = null, bool? sendSuccessEmail = null);

}