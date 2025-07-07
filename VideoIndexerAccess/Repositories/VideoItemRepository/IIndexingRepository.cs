using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IIndexingRepository
{
    /// <summary>
    /// 指定したリクエストモデルに基づき、動画から顔情報を削除します。
    /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
    /// </summary>
    /// <param name="request">動画IDおよび顔IDを含む削除リクエストモデル</param>
    /// <returns>削除が成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    Task<bool> DeleteVideoFaceAsync(DeleteVideoFaceRequestModel request);

    /// <summary>
    /// 指定したロケーション・アカウントID・リクエスト・アクセストークンで動画から顔情報を削除します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">動画IDおよび顔IDを含む削除リクエストモデル</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>削除が成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> DeleteVideoFaceAsync(string location, string accountId, DeleteVideoFaceRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定されたリクエストモデルに基づいて、ビデオインデックスを取得します。
    /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
    /// </summary>
    /// <param name="request">ビデオIDおよびオプションのパラメータを含むリクエストモデル</param>
    /// <returns>ビデオインデックスデータモデル</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<VideoItemDataModel> GetVideoIndexAsync(VideoIndexRequestModel request);

    /// <summary>
    /// 指定されたロケーション・アカウントID・リクエスト・アクセストークンでビデオインデックスを取得します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="request">ビデオIDおよびオプションのパラメータを含むリクエストモデル</param>
    /// <param name="accessToken">アクセストークン（省略可）</param>
    /// <returns>ビデオインデックスデータモデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<VideoItemDataModel> GetVideoIndexAsync(string location, string accountId, VideoIndexRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された動画の再インデックスを実行します。
    /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
    /// </summary>
    /// <param name="request">再インデックスリクエストモデル</param>
    /// <returns>再インデックスが成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> ReIndexVideoAsync(ReIndexVideoRequestModel request);

    /// <summary>
    /// 指定された動画の再インデックスを実行します。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Video Indexer アカウント ID</param>
    /// <param name="request">再インデックスリクエストモデル</param>
    /// <param name="accessToken">API アクセストークン（省略可）</param>
    /// <returns>再インデックスが成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> ReIndexVideoAsync(string location, string accountId, ReIndexVideoRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された動画の顔情報を更新します。
    /// </summary>
    /// <param name="request">顔情報更新リクエストモデル</param>
    /// <returns>更新が成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> UpdateVideoFaceAsync(UpdateVideoFaceRequestModel request);

    /// <summary>
    /// 指定された動画の顔情報を更新します。
    /// </summary>
    /// <param name="location">Azure のリージョン</param>
    /// <param name="accountId">Video Indexer アカウント ID</param>
    /// <param name="request">顔情報更新リクエストモデル</param>
    /// <param name="accessToken">API アクセストークン（省略可）</param>
    /// <returns>更新が成功した場合は true、それ以外は false</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> UpdateVideoFaceAsync(string location, string accountId, UpdateVideoFaceRequestModel request, string? accessToken = null);

    /// <summary>
    /// 動画インデックスを更新する非同期メソッド。
    /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
    /// </summary>
    /// <param name="request">更新リクエストモデル</param>
    /// <returns>更新された動画インデックスのレスポンスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<VideoIndexResponseModel?> UpdateVideoIndexAsync(UpdateVideoIndexRequestModel request);

    /// <summary>
    /// 動画インデックスを更新する非同期メソッド。
    /// </summary>
    /// <param name="location">Azure のリージョン。</param>
    /// <param name="accountId">Video Indexer アカウント ID。</param>
    /// <param name="request">更新リクエストモデル。</param>
    /// <param name="accessToken">アクセストークン（省略可）。</param>
    /// <returns>更新された動画インデックスのレスポンスモデル。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合。</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合。</exception>
    /// <exception cref="Exception">その他の予期しない例外。</exception>
    Task<VideoIndexResponseModel?> UpdateVideoIndexAsync(string location, string accountId, UpdateVideoIndexRequestModel request, string? accessToken = null);

    /// <summary>
    /// 動画をアップロードし、インデックス処理を開始します。
    /// アカウント情報の取得・検証、アクセストークンの取得を行い、API呼び出しを実施します。
    /// </summary>
    /// <param name="request">アップロードリクエストモデル</param>
    /// <returns>アップロード結果を表す UploadVideoResponseModel オブジェクト。失敗時は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できなかった場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストに失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<UploadVideoResponseModel?> UploadVideoAsync(UploadVideoRequestModel request);

    /// <summary>
    /// 動画をアップロードし、インデックス処理を開始します。
    /// Azure Video Indexer API へ動画ファイルまたは動画URLをアップロードし、
    /// インデックス作成処理を非同期で開始します。
    /// </summary>
    /// <param name="location">Azure のリージョン（例: eastus, japaneast など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID 形式）</param>
    /// <param name="request">
    /// アップロードリクエストモデル。
    /// 必須: VideoName（動画名）、VideoStream（動画ストリーム）、FileName（ファイル名）
    /// オプション: Privacy（公開/非公開）、Priority（優先度）、ExternalId（外部ID）、ExternalUrl（外部URL）、
    /// CallbackUrl（コールバックURL）、VideoUrl（動画URL）、IndexingPreset（インデックスプリセット）、
    /// StreamingPreset（ストリーミングプリセット）、PersonModelId（人物モデルID）、SendSuccessEmail（成功時メール送信）
    /// </param>
    /// <param name="accessToken">API アクセストークン (省略可能)。指定しない場合は内部で取得されます。</param>
    /// <returns>アップロード結果を表す UploadVideoResponseModel オブジェクト。失敗時は null。</returns>
    Task<UploadVideoResponseModel?> UploadVideoAsync(string location, string accountId, UploadVideoRequestModel request, string? accessToken = null);
}