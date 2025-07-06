using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IVideosRepository
{
    /// <summary>
    /// 指定されたビデオを削除します。
    /// Delete Video
    /// Deletes the specified video and all related insights created from when the video was indexed
    /// </summary>
    /// <param name="videoId">削除対象のビデオID</param>
    /// <returns>削除結果を示す <see cref="DeleteVideoResultModel"/> オブジェクト</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合にスローされます。</exception>
    Task<DeleteVideoResultModel?> DeleteVideoAsync(string videoId);

    /// <summary>
    /// 指定されたロケーション、アカウントID、ビデオIDを使用してビデオを削除します。
    /// Delete Video
    /// Deletes the specified video and all related insights created from when the video was indexed
    /// </summary>
    /// <param name="location">Azureリージョン名</param>
    /// <param name="accountId">Video IndexerアカウントID</param>
    /// <param name="videoId">削除対象のビデオID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>削除結果を示す <see cref="DeleteVideoResultModel"/> オブジェクト</returns>
    Task<DeleteVideoResultModel?> DeleteVideoAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// 指定されたビデオのソースファイルを削除します。
    /// Delete Video Source File
    /// </summary>
    /// <param name="videoId">ソースファイルを削除するビデオのID</param>
    /// <returns>削除に成功した場合は true、失敗や例外発生時には false を返します。</returns>
    Task<bool> DeleteVideoSourceFileAsync(string videoId);

    /// <summary>
    /// 指定された動画のソースファイルとストリーミングアセットを削除します（インサイトは保持）。
    /// Delete Video Source File
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Video-Source-File
    /// </summary>
    /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
    /// <param name="accountId">Video Indexer アカウントの一意な GUID 文字列</param>
    /// <param name="videoId">ソースファイルを削除するビデオの ID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>
    /// 削除に成功した場合は true を返し、失敗や例外発生時には false を返します。
    /// </returns>
    Task<bool> DeleteVideoSourceFileAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// 指定されたビデオIDに対するアーティファクト（例: 字幕、顔データ、ラベルなど）のダウンロードURLを取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、ダウンロードURLを返します。
    /// </summary>
    /// <param name="videoId">ダウンロードURLを取得したいビデオのID</param>
    /// <returns>ダウンロード可能な一時的なURL（文字列）。取得できなければnull。</returns>
    Task<string?> GetArtifactDownloadUrlAsync(string videoId);

    /// <summary>
    /// 指定された動画の指定された種類のアーティファクトのダウンロード URL を取得します。
    /// Get Video Artifact Download Url
    /// Artifacts are intermediate outputs of the indexing process.They are essentially raw outputs of the various AI engines that analyze the videos.For this reason, the output formats may change over time.
    /// We do not recommend that you use data directly from the artifacts folder for production purposes. It is recommended that you use the Get Video Index API for most insights.
    /// This API returns a URL only with a link to the specific resource type you request. An additional GET request must be made to this URL for the specific artifact.
    /// </summary>
    /// <param name="location">Azure リージョン名（例: "japaneast"、"westus" など）</param>
    /// <param name="accountId">Video Indexer アカウントの GUID</param>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="artifactType">アーティファクトの種類（例: Transcript, Faces, Labels など）</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>ダウンロード可能な一時的な URL（文字列）</returns>
    Task<string?> GetArtifactDownloadUrlAsync(string location, string accountId, string videoId, ArtifactType? artifactType = null, string? accessToken = null);

    /// <summary>
    /// 指定された動画のキャプション（字幕）を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、字幕データを返します。
    /// </summary>
    /// <param name="request">字幕取得リクエストモデル（VideoId, IndexId, Format, Language, IncludeAudioEffects, IncludeSpeakers を指定可能）</param>
    /// <returns>取得された字幕データ（文字列）。失敗時は null。</returns>
    Task<string?> GetVideoCaptionsAsync(GetVideoCaptionsRequestModel request);

    /// <summary>
    /// 指定された動画に対して字幕（キャプション）を取得します。
    /// Get Video Captions
    /// Get video captions
    /// </summary>
    /// <param name="location">Azure リージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
    /// <param name="videoId">対象となるビデオ ID</param>
    /// <param name="request">字幕取得リクエストモデル（IndexId, Format, Language, IncludeAudioEffects, IncludeSpeakers を指定可能）</param>
    /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
    /// <returns>取得された字幕データ（文字列）を返します。失敗時は null。</returns>
    Task<string?> GetVideoCaptionsAsync(string location, string accountId, GetVideoCaptionsRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された動画から抽出されたフレーム画像の SAS URL 一覧を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、SAS URL 一覧（JSON文字列）を返します。
    /// </summary>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="urlsLifetimeSeconds">URL の有効期限（秒単位、オプション）</param>
    /// <param name="pageSize">1ページあたりの取得件数（省略可能）</param>
    /// <param name="skip">スキップするフレーム数（省略可能）</param>
    /// <returns>JSON 文字列として返される SAS URL 一覧（または null）</returns>
    Task<string?> GetVideoFramesFilePathsAsync(string videoId, int? urlsLifetimeSeconds = null, int? pageSize = null, int? skip = null);

    /// <summary>
    /// 指定された動画から抽出されたフレーム画像の SAS URL 一覧を取得します。
    /// Get Video Frames File Paths
    /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Frames-File-Paths
    /// </summary>
    /// <param name="location">Azure リージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="urlsLifetimeSeconds">URL の有効期限（秒単位、オプション）</param>
    /// <param name="pageSize">1ページあたりの取得件数（省略可能）</param>
    /// <param name="skip">スキップするフレーム数（省略可能）</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>JSON 文字列として返される SAS URL 一覧（または null）</returns>
    Task<string?> GetVideoFramesFilePathsAsync(string location, string accountId, string videoId, int? urlsLifetimeSeconds = null, int? pageSize = null, int? skip = null, string? accessToken = null);

    /// <summary>
    /// 外部IDからVideo IDを取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、Video IDを返します。
    /// </summary>
    /// <param name="externalId">検索対象の外部ID（ExternalId）</param>
    /// <returns>対応するVideo ID（string）。見つからない場合やエラー時はnull。</returns>
    Task<string?> GetVideoIdByExternalIdAsync(string externalId);

    /// <summary>
    /// 指定されたロケーション・アカウントID・外部IDからVideo IDを取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video IndexerのアカウントID（GUID）</param>
    /// <param name="externalId">検索対象の外部ID（ExternalId）</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>対応するVideo ID（string）。見つからない場合やエラー時はnull。</returns>
    Task<string?> GetVideoIdByExternalIdAsync(string location, string accountId, string externalId, string? accessToken = null);

    /// <summary>
    /// 指定された動画のソースファイル（元動画）のダウンロード用一時 URL を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、ダウンロード URL を返します。
    /// </summary>
    /// <param name="videoId">ダウンロード URL を取得したいビデオの ID</param>
    /// <returns>ダウンロード可能な一時的な URL（文字列）。取得できなければ null。</returns>
    Task<string?> GetVideoSourceFileDownloadUrlAsync(string videoId);

    /// <summary>
    /// 指定された動画のソースファイル（元動画）のダウンロード用一時 URL を取得します。
    /// Get Video Source File Download Url
    /// </summary>
    /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
    /// <returns>ソースファイルのダウンロード URL（SAS 付きの一時 URL）。取得できなければ null。</returns>
    Task<string?> GetVideoSourceFileDownloadUrlAsync(string location, string accountId, string videoId, string? accessToken = null);

    /// <summary>
    /// 指定された動画のストリーミング再生用 URL を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、ストリーミング URL を返します。
    /// </summary>
    /// <param name="request">ストリーミング URL 取得リクエストモデル（VideoId, UseProxy, UrlFormat, TokenLifetimeInMinutes を指定可能）</param>
    /// <returns>ストリーミング URL と JWT トークンを含む ApiStreamingUrlModel オブジェクト。取得できなければ null。</returns>
    Task<StreamingUrlModel?> GetVideoStreamingUrlAsync(GetVideoStreamingUrlRequestModel request);

    /// <summary>
    /// 指定された動画のストリーミング再生用 URL を取得します。
    /// Get Video Streaming URL
    /// </summary>
    /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
    /// <param name="request">ストリーミング URL 取得リクエストモデル（VideoId, UseProxy, UrlFormat, TokenLifetimeInMinutes を指定可能）</param>
    /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
    /// <returns>ストリーミング URL と JWT トークンを含む ApiStreamingUrlModel オブジェクト。取得できなければ null。</returns>
    Task<StreamingUrlModel?> GetVideoStreamingUrlAsync(string location, string accountId, GetVideoStreamingUrlRequestModel request, string? accessToken = null);

    /// <summary>
    /// 指定された動画のサムネイル画像を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、サムネイル画像のバイト配列を返します。
    /// </summary>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="thumbnailId">取得したいサムネイルの ID（GUID）</param>
    /// <param name="format">返却されるサムネイルの形式（"Jpeg" または "Base64"）※省略時はデフォルト形式</param>
    /// <returns>サムネイル画像のバイナリ配列（JPEG）または PNG 。取得に失敗した場合は null。</returns>
    Task<byte[]?> GetVideoThumbnailAsync(string videoId, string thumbnailId, VideoThumbnailFormatType? format = null);

    /// <summary>
    /// 指定された動画のサムネイル画像を取得します。
    /// Get Video Thumbnail
    /// Content-Type：おそらく image/jpeg
    /// </summary>
    /// <param name="location">Azure のリージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video Indexer アカウント ID（GUID）</param>
    /// <param name="videoId">対象のビデオ ID</param>
    /// <param name="thumbnailId">取得したいサムネイルの ID（GUID）</param>
    /// <param name="format">返却されるサムネイルの形式（"Jpeg" または "Base64"）※省略時はデフォルト形式</param>
    /// <param name="accessToken">アクセストークン（省略可能／必要に応じて）</param>
    /// <returns>サムネイル画像のバイナリ配列（JPEG）または PNG 。取得に失敗した場合は null。</returns>
    Task<byte[]?> GetVideoThumbnailAsync(string location, string accountId, string videoId, string thumbnailId, VideoThumbnailFormatType? format = null, string? accessToken = null);

    /// <summary>
    /// 指定された Video Indexer アカウント内の動画一覧を取得します。
    /// アカウント情報とアクセストークンを自動的に取得し、動画一覧を返します。
    /// </summary>
    /// <param name="createdAfter">指定日以降に作成された動画にフィルタリング（RFC3339形式、オプション）</param>
    /// <param name="createdBefore">指定日以前に作成された動画にフィルタリング（RFC3339形式、オプション）</param>
    /// <param name="pageSize">1ページあたりの取得件数（オプション）</param>
    /// <param name="skip">先頭からスキップする動画数（オプション）</param>
    /// <param name="partitions">パーティションに基づいて動画をフィルタ（オプション）</param>
    /// <returns>
    /// <see cref="VideoSearchResultModel"/> オブジェクト。成功時は動画の一覧情報を含みます。失敗時は null を返します。
    /// </returns>
    Task<VideoSearchResultModel?> ListVideosAsync(string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null);

    /// <summary>
    /// 指定された Video Indexer アカウント内の動画一覧を取得します。
    /// List Videos
    /// Search videos and projects in the specified account
    /// </summary>
    /// <param name="location">Azureリージョン。例: "japaneast"</param>
    /// <param name="accountId">Video IndexerアカウントのGUID</param>
    /// <param name="createdAfter">指定日以降に作成された動画にフィルタリング（RFC3339形式）</param>
    /// <param name="createdBefore">指定日以前に作成された動画にフィルタリング（RFC3339形式）</param>
    /// <param name="pageSize">1ページあたりの取得件数</param>
    /// <param name="skip">先頭からスキップする動画数</param>
    /// <param name="partitions">パーティションに基づいて動画をフィルタ</param>
    /// <param name="accessToken">APIアクセス用のアクセストークン</param>
    /// <returns>
    /// <see cref="VideoSearchResultModel"/> オブジェクト。成功時は動画の一覧情報を含みます。失敗時は null を返します。
    /// </returns>
    Task<VideoSearchResultModel?> ListVideosAsync(string location, string accountId, string? createdAfter = null, string? createdBefore = null, int? pageSize = null, int? skip = null, string[]? partitions = null, string? accessToken = null);

    /// <summary>
    /// 指定された条件で動画を検索します。
    /// アカウント情報とアクセストークンを自動的に取得し、検索結果のJSON文字列を返します。
    /// </summary>
    /// <param name="request">
    /// 検索条件を指定する <see cref="SearchVideosResponseModel"/> オブジェクト。
    /// 各プロパティの意味は <see cref="SearchVideosAsync(string, string, SearchVideosResponseModel, int?, int?, string?)"/> を参照。
    /// </param>
    /// <param name="pageSize">1ページあたりの取得件数（省略可能）</param>
    /// <param name="skip">スキップする動画数（省略可能）</param>
    /// <returns>検索結果のJSON文字列</returns>
    Task<string> SearchVideosAsync(SearchVideosResponseModel request, int? pageSize = null, int? skip = null);

    /// <summary>
    /// 指定された条件で動画を検索します。
    /// </summary>
    /// <param name="location">Azureリージョン名（例: "japaneast", "westus" など）</param>
    /// <param name="accountId">Video IndexerアカウントID（GUID）</param>
    /// <param name="request">
    /// 検索条件を指定する <see cref="SearchVideosResponseModel"/> オブジェクト。各プロパティの意味は以下の通り:
    /// - SourceLanguage: ソース言語が一致する動画・プロジェクトのみを含めます（例: ja-JP, en-US など）
    ///     ar-AE: アラビア語（アラブ首長国連邦）  
    ///     ar-BH: アラビア語（バーレーン）  
    ///     ar-EG: アラビア語（エジプト）  
    ///     ar-IL: アラビア語（イスラエル）  
    ///     ar-IQ: アラビア語（イラク）  
    ///     ar-JO: アラビア語（ヨルダン）  
    ///     ar-KW: アラビア語（クウェート）  
    ///     ar-LB: アラビア語（レバノン）  
    ///     ar-OM: アラビア語（オマーン）  
    ///     ar-PS: アラビア語（パレスチナ）  
    ///     ar-QA: アラビア語（カタール）  
    ///     ar-SA: アラビア語（サウジアラビア）  
    ///     ar-SY: アラビア語（シリア）  
    ///     bg-BG: ブルガリア語  
    ///     ca-ES: カタルーニャ語  
    ///     cs-CZ: チェコ語  
    ///     da-DK: デンマーク語  
    ///     de-DE: ドイツ語  
    ///     el-GR: ギリシャ語  
    ///     en-AU: 英語（オーストラリア）  
    ///     en-GB: 英語（イギリス）  
    ///     en-US: 英語（アメリカ）  
    ///     es-ES: スペイン語（スペイン）  
    ///     es-MX: スペイン語（メキシコ）  
    ///     et-EE: エストニア語  
    ///     fa-IR: ペルシャ語  
    ///     fi-FI: フィンランド語  
    ///     fil-PH: フィリピン語  
    ///     fr-CA: フランス語（カナダ）  
    ///     fr-FR: フランス語  
    ///     ga-IE: アイルランド語  
    ///     gu-IN: グジャラート語  
    ///     he-IL: ヘブライ語  
    ///     hi-IN: ヒンディー語  
    ///     hr-HR: クロアチア語  
    ///     hu-HU: ハンガリー語  
    ///     hy-AM: アルメニア語  
    ///     id-ID: インドネシア語  
    ///     is-IS: アイスランド語  
    ///     it-IT: イタリア語  
    ///     ja-JP: 日本語  
    ///     kn-IN: カンナダ語  
    ///     ko-KR: 韓国語  
    ///     lt-LT: リトアニア語  
    ///     lv-LV: ラトビア語  
    ///     ml-IN: マラヤーラム語  
    ///     ms-MY: マレー語  
    ///     nb-NO: ノルウェー語  
    ///     nl-NL: オランダ語  
    ///     pl-PL: ポーランド語  
    ///     pt-BR: ポルトガル語（ブラジル）  
    ///     pt-PT: ポルトガル語（ポルトガル）  
    ///     ro-RO: ルーマニア語  
    ///     ru-RU: ロシア語  
    ///     sk-SK: スロバキア語  
    ///     sl-SI: スロベニア語  
    ///     sv-SE: スウェーデン語  
    ///     ta-IN: タミル語  
    ///     te-IN: テルグ語  
    ///     th-TH: タイ語  
    ///     tr-TR: トルコ語  
    ///     uk-UA: ウクライナ語  
    ///     vi-VN: ベトナム語  
    ///     zh-Hans: 中国語（簡体字）  
    ///     zh-HK: 中国語（広東語、繁体字）  
    /// - HasSourceVideoFile: trueの場合はソース動画ファイルを持つ動画のみ、falseの場合はプロジェクトやソースのない動画も含む
    /// - SourceVideoId: 指定された動画IDに一致する動画、またはその動画を含むプロジェクトのみ
    /// - State: 処理状態でフィルター（Uploaded, Processing, Processed, Failed）
    /// - Privacy: プライバシーレベルでフィルター（Private, Public）
    /// - Id: 検索対象とする動画ID
    /// - Partition: 検索対象とするパーティション
    /// - ExternalId: アップロード時に紐付けた外部IDで検索
    /// - Owner: 動画の所有者で検索
    /// - Face: 検出された顔に基づいて検索
    /// - AnimatedCharacter: 検出されたアニメキャラクターに基づいて検索
    /// - Query: フリーテキストで動画を検索（例: "north america" など）
    /// - TextScope: テキスト検索のスコープ（Transcript, Topics, Ocr, など）
    /// - Language: 検索対象の言語（複数指定可）
    /// - CreatedAfter: 指定日以降に作成されたアイテム（RFC3339形式）
    /// - CreatedBefore: 指定日以前に作成されたアイテム（RFC3339形式）
    /// </param>       /// <param name="pageSize">1ページあたりの取得件数（省略可能）</param>
    /// <param name="skip">スキップする動画数（省略可能）</param>
    /// <param name="accessToken">APIアクセス用のアクセストークン（省略可能）</param>
    /// <returns>検索結果のJSON文字列</returns>
    Task<string> SearchVideosAsync(string location, string accountId, SearchVideosResponseModel request, int? pageSize = null, int? skip = null, string? accessToken = null);

    /// <summary>
    /// 指定された動画のトランスクリプト（VTT形式）をアップロードし、再インデックスを実行します。
    /// アカウント情報とアクセストークンを自動的に取得し、アップロード・再インデックスを行います。
    /// </summary>
    /// <param name="request">トランスクリプト更新リクエストモデル（VideoId, VttContent, Language, SetAsSourceLanguage, CallbackUrl, SendSuccessEmail を指定可能）</param>
    /// <returns>成功時は true、失敗時は false を返します。</returns>
    Task<bool> UpdateVideoTranscriptAsync(UpdateVideoTranscriptRequestModel request);

    /// <summary>
    /// 指定された動画のトランスクリプト（VTT形式）をアップロードし、再インデックスを実行します。
    /// </summary>
    /// <param name="location">APIの地域（例: "japaneast"）</param>
    /// <param name="accountId">Video IndexerアカウントのGUID</param>
    /// <param name="request">トランスクリプト更新リクエストモデル（VideoId, VttContent, Language, SetAsSourceLanguage, CallbackUrl, SendSuccessEmail を指定可能）</param>
    /// <param name="accessToken">アクセストークン（Contributor 権限、オプション）</param>
    /// <returns>成功時は true、失敗時は false を返します。</returns>
    Task<bool> UpdateVideoTranscriptAsync(string location, string accountId, UpdateVideoTranscriptRequestModel request, string? accessToken = null);
}