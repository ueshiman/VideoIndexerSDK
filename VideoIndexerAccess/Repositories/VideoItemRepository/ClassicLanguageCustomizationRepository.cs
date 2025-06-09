using Microsoft.Extensions.Logging;
using System.Net;
using System.Runtime.Versioning;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class ClassicLanguageCustomizationRepository : IClassicLanguageCustomizationRepository
    {
        // ロガーインスタンス
        private readonly ILogger<ClassicLanguageCustomizationRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // カスタム言語モデルのAPIアクセスインターフェース
        private readonly IClassicLanguageCustomizationApiAccess _classicLanguageCustomizationApiAccess;

        // カスタム言語モデルのトレーニングデータファイルのマッパー
        private readonly ICustomLanguageModelTrainingDataFileMapper _customLanguageModelTrainingDataFileMapper;

        // カスタム言語モデル作成リクエストのマッパー
        private readonly ICustomLanguageRequestMapper _customLanguageRequestMapper;

        // カスタム言語モデルのマッパー
        private readonly ICustomLanguageMapper _customLanguageMapper;

        // 言語モデル編集履歴のマッパー
        private readonly ILanguageModelEditMapper _languageModelEditMapper;

        // 言語モデルファイルデータのマッパー
        private readonly ILanguageModelFileDataMapper _languageModelFileDataMapper;

        // 言語モデルファイルメタデータのマッパー
        private readonly ILanguageModelFileMetadataMapper _languageModelFileMetadataMapper;

        // 言語モデルデータのマッパー
        private readonly ILanguageModelDataMapper _languageModelDataMapper;

        // 例外スロー時のパラメータ名
        private const string ParamName = "classicLanguageCustomization";

        public ClassicLanguageCustomizationRepository(ILogger<ClassicLanguageCustomizationRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IClassicLanguageCustomizationApiAccess classicLanguageCustomizationApiAccess, ICustomLanguageModelTrainingDataFileMapper customLanguageModelTrainingDataFileMapper, ICustomLanguageRequestMapper customLanguageRequestMapper, ICustomLanguageMapper customLanguageMapper, ILanguageModelEditMapper languageModelEditMapper, ILanguageModelFileDataMapper languageModelFileDataMapper, ILanguageModelFileMetadataMapper languageModelFileMetadataMapper, ILanguageModelDataMapper languageModelDataMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _apiResourceConfigurations = apiResourceConfigurations;
            _classicLanguageCustomizationApiAccess = classicLanguageCustomizationApiAccess;
            _customLanguageModelTrainingDataFileMapper = customLanguageModelTrainingDataFileMapper;
            _customLanguageRequestMapper = customLanguageRequestMapper;
            _customLanguageMapper = customLanguageMapper;
            _languageModelEditMapper = languageModelEditMapper;
            _languageModelFileDataMapper = languageModelFileDataMapper;
            _languageModelFileMetadataMapper = languageModelFileMetadataMapper;
            _languageModelDataMapper = languageModelDataMapper;
        }

        /// <summary>
        /// 指定したモデルIDのトレーニングモデルをキャンセルします。
        /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
        /// </summary>
        /// <param name="modelId">キャンセル対象のモデルID</param>
        /// <returns>キャンセルが成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> CancelTrainingModelAsync(string modelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            return await CancelTrainingModelAsync(location!, accountId!, modelId, accessToken);
        }

        /// <summary>
        /// 指定したロケーション、アカウントID、モデルID、アクセストークンでトレーニングモデルのキャンセルを実行します。
        /// </summary>
        /// <param name="location">Azureのリージョン</param>
        /// <param name="accountId">Azure Video IndexerのアカウントID</param>
        /// <param name="modelId">キャンセル対象のモデルID</param>
        /// <param name="accessToken">認証用のアクセストークン</param>
        /// <returns>キャンセルが成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> CancelTrainingModelAsync(string location, string accountId, string modelId, string accessToken)
        {
            HttpResponseMessage response = await _classicLanguageCustomizationApiAccess.CancelTrainingModelAsync(location, accountId, modelId, accessToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Training model cancellation successful for model ID: {ModelId}", modelId);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    _logger.LogWarning("Training model cancelled successfully. Model ID: {ModelId}", modelId);
                }
                else
                {
                    _logger.LogWarning("Training model cancellation request was successful but did not return NoContent. Model ID: {ModelId}", modelId);
                }
                return true;
            }
            else
            {
                _logger.LogError("Failed to cancel training model. Status code: {StatusCode}, Model ID: {ModelId}", response.StatusCode, modelId);
                return false;
            }
        }

        /// <summary>
        /// カスタム言語モデルを新規作成します。
        /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
        /// </summary>
        /// <param name="model">作成するカスタム言語モデルのリクエストモデル</param>
        /// <returns>作成されたカスタム言語モデル</returns>
        public async Task<CustomLanguageModel> CreateLanguageModelAsync(CustomLanguageRequestModel model)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の作成処理を呼び出し
            return await CreateLanguageModelAsync(location!, accountId!, model, accessToken);
        }


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
        public async Task<CustomLanguageModel> CreateLanguageModelAsync(string location, string accountId, CustomLanguageRequestModel model, string? accessToken = null)
        {
            try
            {
                ApiCustomLanguageRequestModel apiModel = _customLanguageRequestMapper.MapToCustomLanguageRequestModel(model);

                ApiCustomLanguageModel languageModel = await _classicLanguageCustomizationApiAccess.CreateLanguageModelAsync(location, accountId, apiModel, accessToken);
                return _customLanguageMapper.MapFrom(languageModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, model={Model}", location, accountId, model);
                throw new ArgumentException("Invalid argument(s) provided for creating a custom language model.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, model={Model}", location, accountId, model);
                throw new HttpRequestException("Failed to create custom language model due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating custom language model: location={Location}, accountId={AccountId}, model={Model}", location, accountId, model);
                throw new Exception("An unexpected error occurred while creating the custom language model.", ex);
            }
        }

        /// <summary>
        /// 指定したモデルIDのカスタム言語モデルを削除します。
        /// アカウント情報の取得、検証、アクセストークンの取得を内部で行います。
        /// </summary>
        /// <param name="modelId">削除するモデルのID</param>
        /// <returns>削除が成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> DeleteLanguageModelAsync(string modelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の削除処理を呼び出し
            return await DeleteLanguageModelAsync(location!, accountId!, modelId, accessToken);
        }

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
        public async Task<bool> DeleteLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                var response = await _classicLanguageCustomizationApiAccess.DeleteLanguageModelAsync(location, accountId, modelId, accessToken);

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Successfully deleted language model. ModelId: {ModelId}", modelId);
                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.NoContent: break;
                        default:
                            _logger.LogWarning("Language model deletion request was successful but did not return NoContent. ModelId: {ModelId}", modelId);
                            break;
                    }
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to delete language model. StatusCode: {StatusCode}, ModelId: {ModelId}", response.StatusCode, modelId);
                    return false;
                }
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new ArgumentException("Invalid argument(s) provided for deleting a language model.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new HttpRequestException("Failed to delete language model due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new Exception("An unexpected error occurred while deleting the language model.", ex);
            }
        }

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
        public async Task<bool> DeleteLanguageModelFileAsync(LanguageModelFileRequestModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の削除処理を呼び出し
            return await DeleteLanguageModelFileAsync(location!, accountId!, requestModel, accessToken);
        }

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
        public async Task<bool> DeleteLanguageModelFileAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // APIアクセスを呼び出してファイル削除リクエストを送信
                var response = await _classicLanguageCustomizationApiAccess.DeleteLanguageModelFileAsync(location, accountId, requestModel.ModelId, requestModel.FileId, accessToken);

                if (response.IsSuccessStatusCode)
                {
                    // 削除成功時のログ出力（ModelId, FileIdを出力）
                    _logger.LogInformation("Successfully deleted language model file. ModelId: {ModelId}, FileId: {FileId}", requestModel.ModelId, requestModel.FileId);

                    // ステータスコードがNoContent以外の場合は警告ログを出力
                    if (response.StatusCode != HttpStatusCode.NoContent)
                    {
                        _logger.LogWarning("File deletion succeeded but did not return NoContent. ModelId: {ModelId}, FileId: {FileId}", requestModel.ModelId, requestModel.FileId);
                    }
                    return true;
                }
                else
                {
                    // 削除失敗時のエラーログ出力
                    _logger.LogError("Failed to delete language model file. StatusCode: {StatusCode}, ModelId: {ModelId}, FileId: {FileId}", response.StatusCode, requestModel.ModelId, requestModel.FileId);
                    return false;
                }
            }
            catch (ArgumentException ex)
            {
                // 引数不正時のエラーログ出力
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new ArgumentException("Invalid argument(s) provided for deleting a language model file.", ex);
            }
            catch (HttpRequestException ex)
            {
                // APIリクエスト失敗時のエラーログ出力
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new HttpRequestException("Failed to delete language model file due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                // その他の予期しない例外時のエラーログ出力
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new Exception("An unexpected error occurred while deleting the language model file.", ex);
            }
        }

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
        public async Task<StreamReader> DownloadLanguageModelFileContentAsync(LanguageModelFileRequestModel requestModel)
        {

            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際のダウンロード処理を呼び出し
            return await DownloadLanguageModelFileContentAsync(location!, accountId!, requestModel, accessToken);
        }


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
        public async Task<StreamReader> DownloadLanguageModelFileContentAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // API経由でファイルコンテンツを取得
                var response = await _classicLanguageCustomizationApiAccess.DownloadLanguageModelFileContentAsync(location, accountId, requestModel.ModelId, requestModel.FileId, accessToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to download language model file content. StatusCode: {StatusCode}, ModelId: {ModelId}, FileId: {FileId}", response.StatusCode, requestModel.ModelId, requestModel.FileId);
                    throw new HttpRequestException($"Failed to download file content. StatusCode: {response.StatusCode}");
                }

                // レスポンスのストリームをStreamReaderでラップして返却
                var stream = await response.Content.ReadAsStreamAsync();
                return new StreamReader(stream);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new ArgumentException("Invalid argument(s) provided for downloading a language model file content.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new Exception("An unexpected error occurred while downloading the language model file content.", ex);
            }
        }

        /// <summary>
        /// カスタム言語モデルファイルを指定パスにダウンロード＆保存します。
        /// 認証やアカウント情報の取得も内部で処理されます。
        /// </summary>
        /// <param name="requestModel">ダウンロード対象ファイルの情報（モデルID・ファイルID）</param>
        /// <param name="savePath">保存先のファイルパス</param>
        /// <returns>保存完了後のファイルパス</returns>
        public async Task<string> DownloadLanguageModelFileToPathAsync(LanguageModelFileRequestModel requestModel, string savePath)
        {
            // ファイルストリーム取得（StreamReader経由で）
            using var reader = await DownloadLanguageModelFileContentAsync(requestModel);

            // ディレクトリが存在しない場合は作成
            var directory = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

            // 保存処理
            using var writer = new StreamWriter(savePath, append: false);
            await writer.WriteAsync(await reader.ReadToEndAsync());

            return savePath;
        }

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
        public async Task<string> DownloadLanguageModelFileToPathAsync(string location, string accountId, LanguageModelFileRequestModel requestModel, string savePath, string? accessToken = null)
        {
            using var reader = await DownloadLanguageModelFileContentAsync(location, accountId, requestModel, accessToken);

            // ディレクトリが存在しなければ作る
            var directory = Path.GetDirectoryName(savePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

            // 内容を書き込む
            await using var writer = new StreamWriter(savePath, append: false);
            await writer.WriteAsync(await reader.ReadToEndAsync());

            return savePath;
        }

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
        public async Task<CustomLanguageModel> GetLanguageModelAsync(string modelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の取得処理を呼び出し
            return await GetLanguageModelAsync(location!, accountId!, modelId, accessToken);
        }

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
        public async Task<CustomLanguageModel> GetLanguageModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                // APIからカスタム言語モデル情報を取得
                var apiModel = await _classicLanguageCustomizationApiAccess.GetLanguageModelAsync(location, accountId, modelId, accessToken);

                // APIモデルをアプリ用モデルにマッピング
                return _customLanguageMapper.MapFrom(apiModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new ArgumentException("Invalid argument(s) provided for getting a language model.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new HttpRequestException("Failed to get language model due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new Exception("An unexpected error occurred while getting the language model.", ex);
            }
        }


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
        [RequiresPreviewFeatures]
        public async Task<List<LanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string modelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の取得処理を呼び出し
            return await GetLanguageModelEditsHistoryAsync(location!, accountId!, modelId, accessToken);
        }

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
        [RequiresPreviewFeatures]
        public async Task<List<LanguageModelEditModel>?> GetLanguageModelEditsHistoryAsync(string location, string accountId, string modelId, string accessToken)
        {
            try
            {
                // APIから編集履歴を取得
                List<ApiLanguageModelEditModel>? apiEditList = await _classicLanguageCustomizationApiAccess.GetLanguageModelEditsHistoryAsync(location, accountId, modelId, accessToken);

                if (apiEditList == null)
                {
                    _logger.LogWarning("No edit history found for modelId: {ModelId}", modelId);
                    return null;
                }

                // APIモデルからアプリ用モデルへ変換
                var result = apiEditList.Select(_languageModelEditMapper.MapFrom).ToList();
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new ArgumentException("Invalid argument(s) provided for getting language model edits history.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new HttpRequestException("Failed to get language model edits history due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new Exception("An unexpected error occurred while getting the language model edits history.", ex);
            }
        }


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
        public async Task<LanguageModelFileDataModel?> GetLanguageModelFileDataAsync(CustomLanguageRequestModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の取得処理を呼び出し
            return await GetLanguageModelFileDataAsync(location!, accountId!, requestModel, accessToken);
        }

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
        public async Task<LanguageModelFileDataModel?> GetLanguageModelFileDataAsync(string location, string accountId, CustomLanguageRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // requestModel から modelId, fileId を取得
                var modelId = requestModel.ModelName;
                var fileId = requestModel.Language;

                // APIからファイルデータを取得
                var apiModel = await _classicLanguageCustomizationApiAccess.GetLanguageModelFileDataAsync(location, accountId, modelId, fileId, accessToken);

                if (apiModel == null)
                {
                    _logger.LogWarning("Language model file data not found. location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, modelId, fileId);
                    return null;
                }

                // APIモデルをアプリ用モデルにマッピング
                return _languageModelFileDataMapper.MapFrom(apiModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelName, requestModel.Language);
                throw new ArgumentException("Invalid argument(s) provided for getting language model file data.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelName, requestModel.Language);
                throw new HttpRequestException("Failed to get language model file data due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelName, requestModel.Language);
                throw new Exception("An unexpected error occurred while getting the language model file data.", ex);
            }
        }

        /// <summary>
        /// アカウント情報の取得、検証、アクセストークンの取得を内部で行い、
        /// 言語モデルの一覧を取得します。
        /// </summary>
        /// <returns>言語モデルのリスト。存在しない場合は null。</returns>
        /// <exception cref="ArgumentNullException">アカウント情報が取得できない場合</exception>
        /// <exception cref="ArgumentException">引数が不正な場合</exception>
        /// <exception cref="HttpRequestException">APIリクエストが失敗した場合</exception>
        /// <exception cref="Exception">その他の予期しない例外</exception>
        public async Task<List<LanguageModelDataModel>?> GetLanguageModelsAsync()
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の取得処理を呼び出し
            return await GetLanguageModelsAsync(location!, accountId!, accessToken);
        }

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
        public async Task<List<LanguageModelDataModel>?> GetLanguageModelsAsync(string location, string accountId, string accessToken)
        {
            try
            {
                // APIから言語モデル一覧を取得
                var apiList = await _classicLanguageCustomizationApiAccess.GetLanguageModelsAsync(location, accountId, accessToken);

                if (apiList == null)
                {
                    _logger.LogWarning("No language models found. location={Location}, accountId={AccountId}", location, accountId);
                    return null;
                }

                // APIモデルからアプリ用モデルへ変換
                var result = apiList.Select(apiModel => new LanguageModelDataModel
                {
                    Id = apiModel.id,
                    Name = apiModel.name,
                    Language = apiModel.language,
                    State = apiModel.state,
                    LanguageModelId = apiModel.languageModelId,
                    Files = apiModel.files?.Select(f => new LanguageModelFileMetadataModel
                    {
                        Id = f.id,
                        Name = f.name,
                        Creator = f.creator
                    }).ToList() ?? new List<LanguageModelFileMetadataModel>()
                }).ToList();

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}", location, accountId);
                throw new ArgumentException("Invalid argument(s) provided for getting language models.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}", location, accountId);
                throw new HttpRequestException("Failed to get language models due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error: location={Location}, accountId={AccountId}", location, accountId);
                throw new Exception("An unexpected error occurred while getting the language models.", ex);
            }
        }


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
        public async Task<LanguageModelDataModel?> TrainLanguageModelAsync(string modelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際のトレーニング処理を呼び出し
            return await TrainLanguageModelAsync(location!, accountId!, modelId, accessToken);
        }

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
        public async Task<LanguageModelDataModel?> TrainLanguageModelAsync(string location, string accountId, string modelId, string accessToken)
        {
            try
            {
                // API経由でトレーニングを実行
                var apiModel = await _classicLanguageCustomizationApiAccess.TrainLanguageModelAsync(location, accountId, modelId, accessToken);

                if (apiModel == null)
                {
                    _logger.LogWarning("TrainLanguageModelAsync: API returned null. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                    return null;
                }

                // APIモデルをアプリ用モデルにマッピング
                var result = new LanguageModelDataModel
                {
                    Id = apiModel.id,
                    Name = apiModel.name,
                    Language = apiModel.language,
                    State = apiModel.state,
                    LanguageModelId = apiModel.languageModelId
                };

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "TrainLanguageModelAsync: Argument error. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new ArgumentException("Invalid argument(s) specified for training the language model.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "TrainLanguageModelAsync: API request failed. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new HttpRequestException("Failed to train the language model due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TrainLanguageModelAsync: Unexpected error. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                throw new Exception("An unexpected error occurred while training the language model.", ex);
            }
        }


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
        public async Task<LanguageModelDataModel?> UpdateLanguageModelAsync(LanguageModelRequestModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の更新処理を呼び出し
            return await UpdateLanguageModelAsync(location!, accountId!, requestModel, accessToken);
        }


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
        public async Task<LanguageModelDataModel?> UpdateLanguageModelAsync(string location, string accountId, LanguageModelRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // アクセストークンが未指定の場合は取得
                if (string.IsNullOrEmpty(accessToken)) accessToken = await _authenticationTokenizer.GetAccessToken();

                var apiModel = await _classicLanguageCustomizationApiAccess.UpdateLanguageModelAsync(location, accountId, requestModel.ModelId, accessToken, requestModel.ModelName, requestModel.Enable, requestModel.FileUrls, requestModel.FilePaths);

                if (apiModel is null)
                {
                    _logger.LogWarning("UpdateLanguageModelAsync: API returned null. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, requestModel.ModelId);
                    return null;
                }

                // APIモデルをアプリ用モデルにマッピング

                return _languageModelDataMapper.MapFrom(apiModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelAsync: Argument error. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, requestModel.ModelId);
                throw new ArgumentException("Invalid argument(s) specified for updating the language model.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelAsync: API request failed. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, requestModel.ModelId);
                throw new HttpRequestException("Failed to update the language model due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelAsync: Unexpected error. location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, requestModel.ModelId);
                throw new Exception("An unexpected error occurred while updating the language model.", ex);
            }
        }


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
        public async Task<LanguageModelFileDataModel?> UpdateLanguageModelFileAsync(UpdateLanguageModelFileRequestModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);

            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);

            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;

            // アクセストークンを取得
            string accessToken = await _authenticationTokenizer.GetAccessToken();

            // 実際の更新処理を呼び出し
            return await UpdateLanguageModelFileAsync(location!, accountId!, requestModel, accessToken);
        }

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
        public async Task<LanguageModelFileDataModel?> UpdateLanguageModelFileAsync(string location, string accountId, UpdateLanguageModelFileRequestModel requestModel, string? accessToken = null)
        {
            try
            {
                // アクセストークンが未指定の場合は取得
                if (string.IsNullOrEmpty(accessToken))
                    accessToken = await _authenticationTokenizer.GetAccessToken();

                // API経由でファイル情報を更新
                var apiModel = await _classicLanguageCustomizationApiAccess.UpdateLanguageModelFileAsync(location, accountId, requestModel.ModelId, requestModel.FileId, accessToken, requestModel.FileName, requestModel.Enable
                );

                if (apiModel == null)
                {
                    _logger.LogWarning("UpdateLanguageModelFileAsync: API returned null. location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                    return null;
                }

                // APIモデルをアプリ用モデルにマッピング
                return _languageModelFileDataMapper.MapFrom(apiModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelFileAsync: Argument error. location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new ArgumentException("Invalid argument(s) specified for updating the language model file.", ex);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelFileAsync: API request failed. location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new HttpRequestException("Failed to update the language model file due to an API request error.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateLanguageModelFileAsync: Unexpected error. location={Location}, accountId={AccountId}, modelId={ModelId}, fileId={FileId}", location, accountId, requestModel.ModelId, requestModel.FileId);
                throw new Exception("An unexpected error occurred while updating the language model file.", ex);
            }
        }
    }
}
