using Microsoft.Extensions.Logging;
using System.Net;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class ClassicLanguageCustomizationRepository
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

        private readonly IClassicLanguageCustomizationApiAccess _classicLanguageCustomizationApiAccess;
        private readonly ICustomLanguageModelTrainingDataFileMapper _customLanguageModelTrainingDataFileMapper;
        private readonly ICustomLanguageRequestMapper _customLanguageRequestMapper;
        private readonly ICustomLanguageMapper _customLanguageMapper;

        // 例外スロー時のパラメータ名
        private const string ParamName = "account";

        public ClassicLanguageCustomizationRepository(ILogger<ClassicLanguageCustomizationRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, IApiResourceConfigurations apiResourceConfigurations, IClassicLanguageCustomizationApiAccess classicLanguageCustomizationApiAccess, ICustomLanguageModelTrainingDataFileMapper customLanguageModelTrainingDataFileMapper, ICustomLanguageRequestMapper customLanguageRequestMapper, ICustomLanguageMapper customLanguageMapper)
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

        public async Task<bool> DeleteLanguageModelFileAsync(LanguageModelFileRequestModel requestModel)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName)
                          ?? throw new ArgumentNullException(paramName: ParamName);

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

            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName)
                          ?? throw new ArgumentNullException(paramName: ParamName);

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
                var response = await _classicLanguageCustomizationApiAccess
                    .DownloadLanguageModelFileContentAsync(location, accountId, requestModel.ModelId, requestModel.FileId, accessToken);

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
    }
}
