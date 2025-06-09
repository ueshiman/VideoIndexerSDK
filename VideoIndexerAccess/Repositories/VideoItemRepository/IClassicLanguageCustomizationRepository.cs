using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IClassicLanguageCustomizationRepository
{
    /// <summary>
    /// 指定したモデルIDのトレーニングモデルをキャンセルします。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="modelId">キャンセル対象のモデルID</param>
    /// <returns>キャンセルが成功した場合はtrue、失敗した場合はfalse</returns>
    Task<bool> CancelTrainingModelAsync(string modelId);

    /// <summary>
    /// 指定したロケーション、アカウントID、モデルID、アクセストークンでトレーニングモデルのキャンセルを実行します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">キャンセル対象のモデルID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <returns>キャンセルが成功した場合はtrue、失敗した場合はfalse</returns>
    Task<bool> CancelTrainingModelAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// カスタム言語モデルを新規作成します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="model">作成するカスタム言語モデルのリクエストモデル</param>
    /// <returns>作成されたカスタム言語モデル</returns>
    Task<CustomLanguageModel> CreateLanguageModelAsync(CustomLanguageRequestModel model);

    /// <summary>
    /// 指定したロケーション、アカウントID、リクエストモデル、アクセストークンでカスタム言語モデルを新規作成します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="model">作成するカスタム言語モデルのリクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>作成されたカスタム言語モデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<CustomLanguageModel> CreateLanguageModelAsync(string location, string accountId, CustomLanguageRequestModel model, string? accessToken = null);

    /// <summary>
    /// 指定したモデルIDのカスタム言語モデルを削除します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="modelId">削除するモデルのID</param>
    /// <returns>削除が成功した場合はtrue、失敗した場合はfalse</returns>
    Task<bool> DeleteLanguageModelAsync(string modelId);

    /// <summary>
    /// 指定したロケーション、アカウントID、モデルID、アクセストークンで言語モデルを削除します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">削除するモデルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>削除が成功した場合はtrue、失敗した場合はfalse</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> DeleteLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// 指定したモデルID・ファイルIDの言語モデルファイルを削除します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="requestModel">削除対象ファイルの情報（モデルID・ファイルIDを含む）</param>
    /// <returns>削除が成功した場合はtrue、失敗した場合はfalse</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> DeleteLanguageModelFileAsync(LanguageModelFileRequestModel requestModel);

    /// <summary>
    /// 指定したロケーション、アカウントID、削除リクエストモデル、アクセストークンで
    /// 言語モデルファイルを削除します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="requestModel">削除対象ファイルの情報（モデルID・ファイルIDを含む）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>削除が成功した場合はtrue、失敗した場合はfalse</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<bool> DeleteLanguageModelFileAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string? accessToken = null);

    /// <summary>
    /// 指定したモデルIDのカスタム言語モデル情報を取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="requestModel">ダウンロード対象ファイルの情報（モデルID・ファイルIDを含む）</param>
    /// <returns>取得したカスタム言語モデル</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    // アカウント情報を取得し、存在しない場合は例外をスロー
    Task<StreamReader> DownloadLanguageModelFileContentAsync(LanguageModelFileRequestModel requestModel);

    /// <summary>
    /// 指定したロケーション、アカウントID、リクエストモデル、アクセストークンで
    /// 言語モデルファイルのコンテンツをダウンロードします。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="requestModel">ダウンロード対象ファイルの情報（モデルID・ファイルIDを含む）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>ファイルコンテンツのStreamReader</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<StreamReader> DownloadLanguageModelFileContentAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string? accessToken = null);

    /// <summary>
    /// カスタム言語モデルファイルを指定パスにダウンロード＆保存します。
    /// 認証やアカウント情報の取得も内部で処理されます。
    /// </summary>
    /// <param name="requestModel">ダウンロード対象ファイルの情報（モデルID・ファイルID）</param>
    /// <param name="savePath">保存先のファイルパス</param>
    /// <returns>保存完了後のファイルパス</returns>
    Task<string> DownloadLanguageModelFileToPathAsync(LanguageModelFileRequestModel requestModel, string savePath);

    /// <summary>
    /// 指定された location, accountId, accessToken を使って、
    /// カスタム言語モデルファイルを指定パスにダウンロード＆保存します。
    /// </summary>
    /// <param name="location">Azure Video Indexer のリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="requestModel">モデルID・ファイルIDなどのファイル情報</param>
    /// <param name="savePath">保存先ファイルパス</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>保存されたファイルのパス</returns>
    Task<string> DownloadLanguageModelFileToPathAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string savePath, string? accessToken = null);

    /// <summary>
    /// 指定したモデルIDのカスタム言語モデル情報を取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="modelId">取得するモデルのID</param>
    /// <returns>取得したカスタム言語モデル</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<CustomLanguageModel> GetLanguageModelAsync(string modelId);

    /// <summary>
    /// 指定したロケーション、アカウントID、モデルID、アクセストークンでカスタム言語モデル情報を取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">取得するモデルのID</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>取得したカスタム言語モデル</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<CustomLanguageModel> GetLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null);

    /// <summary>
    /// 指定したモデルIDの言語モデル編集履歴を取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="modelId">編集履歴を取得するモデルのID</param>
    /// <returns>編集履歴のリスト（存在しない場合はnull）</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<List<LanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string modelId);

    /// <summary>
    /// 指定したロケーション、アカウントID、モデルID、アクセストークンで言語モデルの編集履歴を取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">編集履歴を取得するモデルのID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <returns>編集履歴のリスト（存在しない場合はnull）</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<List<LanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// 指定したカスタム言語モデルリクエストモデルに基づき、言語モデルファイルデータを取得します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="requestModel">取得対象のカスタム言語モデルリクエストモデル</param>
    /// <returns>取得した言語モデルファイルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelFileDataModel?> GetLanguageModelFileDataAsync(CustomLanguageRequestModel requestModel);

    /// <summary>
    /// 指定したロケーション、アカウントID、カスタム言語モデルリクエストモデル、アクセストークンで
    /// 言語モデルファイルデータを取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="requestModel">取得対象のカスタム言語モデルリクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>取得した言語モデルファイルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelFileDataModel?> GetLanguageModelFileDataAsync(string location, string accountId, CustomLanguageRequestModel requestModel, string? accessToken = null);

    /// <summary>
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行い、
    /// 言語モデルの一覧を取得します。
    /// </summary>
    /// <returns>言語モデルのリスト。存在しない場合は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<List<LanguageModelDataModel>?> GetLanguageModelsAsync();

    /// <summary>
    /// 指定したロケーション、アカウントID、アクセストークンで言語モデルの一覧を取得します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <returns>言語モデルのリスト。存在しない場合は null。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<List<LanguageModelDataModel>?> GetLanguageModelsAsync(string location, string accountId, string accessToken);

    /// <summary>
    /// 指定したモデルIDの言語モデルをトレーニングします。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="modelId">トレーニング対象のモデルID</param>
    /// <returns>トレーニング後の言語モデルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelDataModel?> TrainLanguageModelAsync(string modelId);

    /// <summary>
    /// 指定したロケーション、アカウントID、モデルID、アクセストークンで言語モデルのトレーニングを実行します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="modelId">トレーニング対象のモデルID</param>
    /// <param name="accessToken">認証用のアクセストークン</param>
    /// <returns>トレーニング後の言語モデルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelDataModel?> TrainLanguageModelAsync(string location, string accountId, string modelId, string accessToken);

    /// <summary>
    /// 指定したリクエストモデルに基づき言語モデルを更新します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="requestModel">更新内容を含むリクエストモデル</param>
    /// <returns>更新後の言語モデルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelDataModel?> UpdateLanguageModelAsync(LanguageModelRequestModel requestModel);

    /// <summary>
    /// 指定したロケーション、アカウントID、リクエストモデル、アクセストークンで言語モデルを更新します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="requestModel">更新内容を含むリクエストモデル</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>更新後の言語モデルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelDataModel?> UpdateLanguageModelAsync(string location, string accountId, LanguageModelRequestModel requestModel, string? accessToken = null);

    /// <summary>
    /// 指定したリクエストモデルに基づき言語モデルファイルを更新します。
    /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
    /// </summary>
    /// <param name="requestModel">更新内容を含むリクエストモデル（モデルID・ファイルID・ファイル名・有効/無効）</param>
    /// <returns>更新後の言語モデルファイルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelFileDataModel?> UpdateLanguageModelFileAsync(UpdateLanguageModelFileRequestModel requestModel);

    /// <summary>
    /// 指定したロケーション、アカウントID、ファイル更新リクエストモデル、アクセストークンで
    /// 言語モデルファイルの情報（ファイル名や有効/無効状態）を更新します。
    /// </summary>
    /// <param name="location">Azureのリージョン</param>
    /// <param name="accountId">Azure Video IndexerのアカウントID</param>
    /// <param name="requestModel">更新内容を含むリクエストモデル（モデルID・ファイルID・ファイル名・有効/無効）</param>
    /// <param name="accessToken">認証用のアクセストークン（省略可）</param>
    /// <returns>更新後の言語モデルファイルデータ。存在しない場合は null。</returns>
    /// <exception cref="ArgumentException">引数が不正な場合</exception>
    /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
    /// <exception cref="Exception">その他の予期しない例外</exception>
    Task<LanguageModelFileDataModel?> UpdateLanguageModelFileAsync(string location, string accountId, UpdateLanguageModelFileRequestModel requestModel, string? accessToken = null);
}