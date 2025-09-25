using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class SpeachCutomizationRepository
    {
        // ロガーインスタンス
        private readonly ILogger<SpeachCutomizationRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly ISpeechCustomizationApiAccess _speechCustomizationApiAccess;
        private readonly ISpeechDatasetMapper _speechDatasetMapper;
        private readonly ICustomSpeechMapper _customSpeechMapper;
        private readonly ISpeechDatasetFileMapper _speechDatasetFileMapper;

        private const string ParamName = "speachCutomization";


        public SpeachCutomizationRepository(ILogger<SpeachCutomizationRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, ISpeechCustomizationApiAccess speechCustomizationApiAccess, ISpeechDatasetMapper speechDatasetMapper, ICustomSpeechMapper customSpeechMapper, ISpeechDatasetFileMapper speechDatasetFileMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _speechCustomizationApiAccess = speechCustomizationApiAccess;
            _speechDatasetMapper = speechDatasetMapper;
            _customSpeechMapper = customSpeechMapper;
            _speechDatasetFileMapper = speechDatasetFileMapper;
        }

        /// <summary>
        /// API を呼び出してスピーチデータセットを作成します。
        /// Create Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Dataset
        /// </summary>
        /// <param name="request">スピーチデータセットのリクエストオブジェクト</param>
        /// <returns>作成したスピーチデータセット情報、エラー時は null</returns>
        public async Task<SpeechDatasetModel?> CreateSpeechDatasetAsync(ApiSpeechDatasetRequestModel request)
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

            return await CreateSpeechDatasetAsync(location!, accountId!, request, accessToken);
        }


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
        public async Task<SpeechDatasetModel?> CreateSpeechDatasetAsync(string location, string accountId, ApiSpeechDatasetRequestModel request, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の CreateSpeechDatasetAsync を呼び出して API へリクエスト
                ApiSpeechDatasetModel?  response = await _speechCustomizationApiAccess.CreateSpeechDatasetAsync(location, accountId, request, accessToken);
                return _speechDatasetMapper.MapFrom(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット作成中に予期せぬエラー: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
        }

        /// <summary>
        /// API を呼び出してスピーチモデルを作成します。
        /// Create Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Create-Speech-Model
        /// </summary>
        /// <param name="request">スピーチモデルのリクエストオブジェクト</param>
        /// <returns>作成したスピーチモデル情報、エラー時は null</returns>
        public async Task<CustomSpeechModel?> CreateSpeechModelAsync(ApiSpeechModelRequestModel request)
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

            return await CreateSpeechModelAsync(location!, accountId!, request, accessToken);
        }

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
        public async Task<CustomSpeechModel?> CreateSpeechModelAsync(string location, string accountId, ApiSpeechModelRequestModel request, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の CreateSpeechModelAsync を呼び出して API へリクエスト
                ApiCustomSpeechModel? response = await _speechCustomizationApiAccess.CreateSpeechModelAsync(location, accountId, request, accessToken);
                return response is null ? null : _customSpeechMapper.MapFrom(response);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチモデル作成中に予期せぬエラー: location={Location}, accountId={AccountId}, request={Request}", location, accountId, request);
                return null;
            }
        }

        /// <summary>
        /// スピーチデータセット削除
        /// Delete Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Dataset
        /// </summary>
        /// <param name="datasetId">削除するデータセットの ID</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechDatasetAsync(string datasetId)
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

            // API呼び出し
            return await DeleteSpeechDatasetAsync(location!, accountId!, datasetId, accessToken);
        }

        /// <summary>
        /// スピーチデータセット削除
        /// Delete Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">削除するデータセットの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の DeleteSpeechDatasetAsync を呼び出して API へリクエスト
                var result = await _speechCustomizationApiAccess.DeleteSpeechDatasetAsync(location, accountId, datasetId, accessToken);
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット削除中に予期せぬエラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return false;
            }
        }

        /// <summary>
        /// スピーチモデル削除リクエストを送信します。
        /// Delete Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Model
        /// </summary>
        /// <param name="modelId">削除するスピーチモデルの ID</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechModelAsync(string modelId)
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

            // API呼び出し
            return await DeleteSpeechModelAsync(location!, accountId!, modelId, accessToken);
        }

        /// <summary>
        /// スピーチモデル削除リクエストを送信します。
        /// Delete Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Delete-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="modelId">削除するスピーチモデルの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>削除成功時は true、失敗時は false</returns>
        public async Task<bool> DeleteSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の DeleteSpeechModelAsync を呼び出して API へリクエスト
                var result = await _speechCustomizationApiAccess.DeleteSpeechModelAsync(location, accountId, modelId, accessToken);
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return false;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチモデル削除中に予期せぬエラー: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return false;
            }
        }

        /// <summary>
        /// スピーチデータセットを取得します。
        /// Get Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
        /// </summary>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <returns>スピーチデータセット情報を含む ApiSpeechDatasetUpdateModel オブジェクト</returns>
        public async Task<SpeechDatasetModel?> GetSpeechDatasetAsync(string datasetId)
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

            // API呼び出し
            return await GetSpeechDatasetAsync(location!, accountId!, datasetId, accessToken);
        }

        /// <summary>
        /// スピーチデータセットを取得します。
        /// Get Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセット情報を含む ApiSpeechDatasetUpdateModel オブジェクト</returns>
        public async Task<SpeechDatasetModel?> GetSpeechDatasetAsync(string location, string accountId, string datasetId, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の GetSpeechDatasetAsync を呼び出して API へリクエスト
                ApiSpeechDatasetModel? result = await _speechCustomizationApiAccess.GetSpeechDatasetAsync(location, accountId, datasetId, accessToken);
                return result is null ? null : _speechDatasetMapper.MapFrom(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット取得中に予期せぬエラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}", location, accountId, datasetId);
                return null;
            }
        }

        /// <summary>
        /// スピーチデータセットのファイル一覧を取得します。
        /// Get Speech Dataset Files
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
        /// </summary>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
        /// <returns>スピーチデータセットのファイルリストを含むリスト。取得できなかった場合は null。</returns>
        public async Task<List<SpeechDatasetFileModel>?> GetSpeechDatasetFilesAsync(string datasetId, int? sasValidityInSeconds = null)
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

            // API呼び出し
            return await GetSpeechDatasetFilesAsync(location!, accountId!, datasetId, sasValidityInSeconds, accessToken);
        }

        /// <summary>
        /// スピーチデータセットのファイル一覧を取得します。
        /// Get Speech Dataset Files
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Dataset-Files
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="datasetId">取得するデータセットの ID</param>
        /// <param name="sasValidityInSeconds">SAS URL の有効時間（秒）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセットのファイルリストを含むリスト。取得できなかった場合は null。</returns>
        public async Task<List<SpeechDatasetFileModel>?> GetSpeechDatasetFilesAsync(string location, string accountId, string datasetId, int? sasValidityInSeconds = null, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の GetSpeechDatasetFilesAsync を呼び出して API へリクエスト
                List<ApiSpeechDatasetFileModel>? apiFiles = await _speechCustomizationApiAccess.GetSpeechDatasetFilesAsync(location, accountId, datasetId, sasValidityInSeconds, accessToken);
                if (apiFiles == null) return null;

                // ApiSpeechDatasetFileModel から SpeechDatasetFileModel へマッピング
                List<SpeechDatasetFileModel> result = apiFiles.Select(_speechDatasetFileMapper.MapFrom).ToList();

                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}, sasValidityInSeconds={SasValidityInSeconds}", location, accountId, datasetId, sasValidityInSeconds);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, datasetId={DatasetId}, sasValidityInSeconds={SasValidityInSeconds}", location, accountId, datasetId, sasValidityInSeconds);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセットファイル一覧取得中に予期せぬエラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}, sasValidityInSeconds={SasValidityInSeconds}", location, accountId, datasetId, sasValidityInSeconds);
                return null;
            }
        }

        /// <summary>
        /// スピーチデータセット一覧を取得します。
        /// Get Speech Datasets
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
        /// </summary>
        /// <param name="locale">取得するデータセットのロケール（省略可、null の場合はすべて取得）</param>
        /// <returns>スピーチデータセットのリスト。取得できなかった場合は null。</returns>
        public async Task<List<SpeechDatasetModel>?> GetSpeechDatasetsAsync(string? locale = null)
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

            // API呼び出し
            return await GetSpeechDatasetsAsync(location!, accountId!, locale, accessToken);
        }

        /// <summary>
        /// スピーチデータセット一覧を取得します。
        /// Get Speech Datasets
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Datasets
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="locale">取得するデータセットのロケール（省略可、null の場合はすべて取得）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチデータセットのリスト。取得できなかった場合は null。</returns>
        public async Task<List<SpeechDatasetModel>?> GetSpeechDatasetsAsync(string location, string accountId, string? locale = null, string? accessToken = null)
        {
            try
            {
                // ISpeechCustomizationApiAccess の GetSpeechDatasetsAsync を呼び出して API へリクエスト
                var apiDatasets = await _speechCustomizationApiAccess.GetSpeechDatasetsAsync(location, accountId, locale, accessToken);
                if (apiDatasets == null) return null;

                // ApiSpeechDatasetModel から SpeechDatasetModel へマッピング
                var result = apiDatasets.Select(_speechDatasetMapper.MapFrom).ToList();
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット一覧取得中に予期せぬエラー: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
        }

        /// <summary>
        /// スピーチモデルを取得します。
        /// Get Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="modelId">取得するスピーチモデルの ID</param>
        /// <returns>スピーチモデル情報。取得できなかった場合は null。</returns>
        public async Task<CustomSpeechModel?> GetSpeechModelAsync(string modelId)
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

            // API呼び出し
            return await GetSpeechModelAsync(location!, accountId!, modelId, accessToken);
        }

        /// <summary>
        /// スピーチモデルを取得します。
        /// Get Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="location">Azure のリージョン</param>
        /// <param name="accountId">アカウント ID</param>
        /// <param name="modelId">取得するスピーチモデルの ID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>スピーチモデル情報。取得できなかった場合は null。</returns>
        public async Task<CustomSpeechModel?> GetSpeechModelAsync(string location, string accountId, string modelId, string? accessToken = null)
        {
            try
            {
                var apiModel = await _speechCustomizationApiAccess.GetSpeechModelAsync(location, accountId, modelId, accessToken);
                if (apiModel == null) return null;

                // ApiCustomSpeechModel から CustomSpeechModel へマッピング
                var customSpeechModel = _customSpeechMapper.MapFrom(apiModel);
                return customSpeechModel;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチモデル取得中に予期せぬエラー: location={Location}, accountId={AccountId}, modelId={ModelId}", location, accountId, modelId);
                return null;
            }
        }

        /// <summary>
        /// スピーチモデルを取得します。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
        /// <returns>取得した <see cref="CustomSpeechModel"/> を含む非同期タスク。</returns>
        public async Task<CustomSpeechModel?> GetSpeechModelsAsync(string? locale = null)
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

            // API呼び出し
            return await GetSpeechModelsAsync(location!, accountId!, locale, accessToken);
        }

        /// <summary>
        /// スピーチモデルを取得します。
        /// Get Speech Models
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Models
        /// </summary>
        /// <param name="location">リクエストをルーティングするAzureリージョン。</param>
        /// <param name="accountId">アカウントのグローバル一意識別子（GUID）。</param>
        /// <param name="locale">（オプション）取得するスピーチモデルのロケール。</param>
        /// <param name="accessToken">（オプション）認証のためのアクセストークン。</param>
        /// <returns>取得した <see cref="CustomSpeechModel"/> を含む非同期タスク。</returns>
        public async Task<CustomSpeechModel?> GetSpeechModelsAsync(string location, string accountId, string? locale = null, string? accessToken = null)
        {
            try
            {
                var apiModel = await _speechCustomizationApiAccess.GetSpeechModelsAsync(location, accountId, locale, accessToken);
                if (apiModel == null) return null;
                var customSpeechModel = _customSpeechMapper.MapFrom(apiModel);
                return customSpeechModel;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチモデル一覧取得中に予期せぬエラー: location={Location}, accountId={AccountId}, locale={Locale}", location, accountId, locale);
                return null;
            }
        }

        /// <summary>
        /// スピーチデータセットを更新します。
        /// Update Speech Dataset
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Update-Speech-Dataset
        /// </summary>
        /// <param name="datasetId">更新するスピーチデータセットのID。</param>
        /// <param name="displayName">更新するデータセットの表示名。</param>
        /// <param name="description">更新するデータセットの説明。</param>
        /// <param name="customProperties">更新するデータセットのカスタムプロパティ。</param>
        /// <returns>更新された <see cref="ApiSpeechDatasetUpdateModel"/> を含む非同期タスク。</returns>
        public async Task<SpeechDatasetModel?> UpdateSpeechDatasetAsync(string datasetId, string? displayName, string? description, Dictionary<string, string>? customProperties)
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

            // API呼び出し
            return await UpdateSpeechDatasetAsync(location!, accountId!, datasetId, displayName, description, customProperties, accessToken);
        }

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
        public async Task<SpeechDatasetModel?> UpdateSpeechDatasetAsync(string location, string accountId, string datasetId, string? displayName, string? description, Dictionary<string, string>? customProperties, string? accessToken = null)
        {
            try
            {
                // API呼び出し
                ApiSpeechDatasetModel? apiResult = await _speechCustomizationApiAccess.UpdateSpeechDatasetAsync(location, accountId, datasetId, displayName, description, customProperties, accessToken);

                // マッピング
                return apiResult is null ? null : _speechDatasetMapper.MapFrom(apiResult);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, datasetId, displayName, description, customProperties);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, datasetId={DatasetId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, datasetId, displayName, description, customProperties);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチデータセット更新中に予期せぬエラー: location={Location}, accountId={AccountId}, datasetId={DatasetId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, datasetId, displayName, description, customProperties);
                return null;
            }
        }

        /// <summary>
        /// スピーチモデルを更新します。
        /// Update Speech Model
        /// https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Speech-Model
        /// </summary>
        /// <param name="modelId">更新するスピーチモデルのID（GUID）。</param>
        /// <param name="displayName">更新するモデルの表示名。</param>
        /// <param name="description">更新するモデルの説明。</param>
        /// <param name="customProperties">更新するモデルのカスタムプロパティ。</param>
        /// <returns>更新されたスピーチモデル情報を含む非同期タスク。</returns>
        public async Task<CustomSpeechModel?> UpdateSpeechModelAsync(string modelId, string? displayName, string? description, Dictionary<string, string>? customProperties)
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

            // API呼び出し
            return await UpdateSpeechModelAsync(location!, accountId!, modelId, displayName, description, customProperties, accessToken);
        }

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
        public async Task<CustomSpeechModel?> UpdateSpeechModelAsync(string location, string accountId, string modelId, string? displayName, string? description, Dictionary<string, string>? customProperties, string? accessToken = null)
        {
            try
            {
                // API へリクエスト
                var apiModel = await _speechCustomizationApiAccess.UpdateSpeechModelAsync(location, accountId, modelId, displayName, description, customProperties, accessToken);
                if (apiModel == null) return null;

                // ApiCustomSpeechModel から CustomSpeechModel へマッピング
                var customSpeechModel = _customSpeechMapper.MapFrom(apiModel);
                return customSpeechModel;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "引数エラー: location={Location}, accountId={AccountId}, modelId={ModelId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, modelId, displayName, description, customProperties);
                return null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "APIリクエスト失敗: location={Location}, accountId={AccountId}, modelId={ModelId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, modelId, displayName, description, customProperties);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "スピーチモデル更新中に予期せぬエラー: location={Location}, accountId={AccountId}, modelId={ModelId}, displayName={DisplayName}, description={Description}, customProperties={CustomProperties}", location, accountId, modelId, displayName, description, customProperties);
                return null;
            }
        }

    }
}
