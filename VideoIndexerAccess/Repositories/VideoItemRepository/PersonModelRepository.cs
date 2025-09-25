using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class PersonModelRepository
    {
        // ロガーインスタンス
        private readonly ILogger<PersonModelRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;
        private readonly ICustomLogosApiAccess _customLogosApiAccess;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        private readonly IPersonModelsApiAccess _personModelsApiAccess;
        private readonly IPersonMapper _personMapper;
        private readonly ICustomPersonMapper _customPersonMapper;
        private readonly IFaceModelMapper _faceModelMapper;

        // 例外スロー時のパラメータ名
        private const string ParamName = "person";

        public PersonModelRepository(ILogger<PersonModelRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IAccountRepository accountRepository, ICustomLogosApiAccess customLogosApiAccess, IApiResourceConfigurations apiResourceConfigurations, IPersonModelsApiAccess personModelsApiAccess, IPersonMapper personMapper, ICustomPersonMapper customPersonMapper, IFaceModelMapper faceModelMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _customLogosApiAccess = customLogosApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _personModelsApiAccess = personModelsApiAccess;
            _personMapper = personMapper;
            _customPersonMapper = customPersonMapper;
            _faceModelMapper = faceModelMapper;
        }

        /// <summary>
        /// 指定した人物モデル内の人物に対して、指定された画像URLを使って顔データを作成します。
        /// </summary>
        /// <remarks>
        /// このメソッドは、指定された人物に対して画像URLを使って顔データ作成のAPIリクエストを送信します。
        /// 操作中にエラーが発生した場合は、例外を記録し再スローします。
        /// </remarks>
        /// <param name="personModelId">人物が所属する人物モデルのID。</param>
        /// <param name="personId">顔データを作成する対象の人物ID。</param>
        /// <param name="imageUrls">追加する顔データを含む画像URLのリスト。</param>
        /// <returns>正常に処理された画像URLのリスト。失敗した場合は <see langword="null"/> を返します。</returns>
        public async Task<List<string>?> CreateCustomFacesAsync(string personModelId, string personId, List<string> imageUrls)
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

            return await CreateCustomFacesAsync(location!, accountId!, personModelId, personId, imageUrls, accessToken);
        }

        /// <summary>
        /// 指定した人物モデル内の人物に対して、指定された画像URLを使って顔データを作成します。
        /// </summary>
        /// <remarks>
        /// このメソッドは、指定された人物に対して画像URLを使って顔データ作成のAPIリクエストを送信します。
        /// 操作中にエラーが発生した場合は、例外を記録し再スローします。
        /// </remarks>
        /// <param name="location">APIエンドポイントの地理的ロケーション。</param>
        /// <param name="accountId">人物モデルに関連付けられたアカウントID。</param>
        /// <param name="personModelId">人物が所属する人物モデルのID。</param>
        /// <param name="personId">顔データを作成する対象の人物ID。</param>
        /// <param name="imageUrls">追加する顔データを含む画像URLのリスト。</param>
        /// <param name="accessToken">認証用のアクセストークン（省略可能）。指定しない場合はデフォルトの認証機構が使用されます。</param>
        /// <returns>正常に処理された画像URLのリスト。失敗した場合は <see langword="null"/> を返します。</returns>
        public async Task<List<string>?> CreateCustomFacesAsync(string location, string accountId, string personModelId, string personId, List<string> imageUrls, string? accessToken = null)
        {
            try
            {
                // APIへ顔データ作成リクエスト送信
                var apiResponse = await _personModelsApiAccess.CreateCustomFacesAsync(location, accountId, personModelId, personId, imageUrls, accessToken);
                return imageUrls;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, imageUrls={ImageUrls}", location, accountId, personModelId, personId, string.Join(",", imageUrls));
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, imageUrls={ImageUrls}", location, accountId, personModelId, personId, string.Join(",", imageUrls));
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating person faces: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, imageUrls={ImageUrls}", location, accountId, personModelId, personId, string.Join(",", imageUrls));
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、新しい Person を作成する
        /// </summary>
        /// <param name="personModelId">Person ModelのID (GUID形式)</param>
        /// <param name="name">作成するPersonの名前 (省略可能)</param>
        /// <param name="description">作成するPersonの説明 (省略可能)</param>
        /// <returns>成功時はAPIからのJSONレスポンス、エラー時は例外をスロー</returns>
        public async Task<PersonModel?> CreatePersonAsync(string personModelId, string? name = null, string? description = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            return await CreatePersonAsync(location!, accountId!, personModelId, name, description, accessTokenFromAccount);
        }

        /// <summary>
        /// API を呼び出し、新しい Person を作成する
        /// </summary>
        /// <param name="location">Azureのリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video IndexerのアカウントID (GUID形式)</param>
        /// <param name="personModelId">Person ModelのID (GUID形式)</param>
        /// <param name="name">作成するPersonの名前 (省略可能)</param>
        /// <param name="description">作成するPersonの説明 (省略可能)</param>
        /// <param name="accessToken">オプションのアクセストークン (省略可能)</param>
        /// <returns>成功時はAPIからのJSONレスポンス、エラー時は例外をスロー</returns>
        public async Task<PersonModel?> CreatePersonAsync(string location, string accountId, string personModelId, string? name = null, string? description = null, string? accessToken = null)
        {
            try
            {
                // APIへPerson作成リクエスト送信
                var apiResponse = await _personModelsApiAccess.CreatePersonAsync(location, accountId, personModelId, name, description, accessToken);
                return apiResponse is null ? null : _personMapper.MapFrom(apiResponse);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}, description={Description}", location, accountId, personModelId, name, description);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}, description={Description}", location, accountId, personModelId, name, description);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating person: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}, description={Description}", location, accountId, personModelId, name, description);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、新しい Person Model を作成する
        /// </summary>
        /// <param name="name">作成する Person Model の名前 (オプション)</param>
        /// <returns>作成された Person Model の情報を含む ApiCustomPersonModel オブジェクト</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="JsonException">JSON の解析に失敗した場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<CustomPersonModel?> CreatePersonModelAsync(string? name = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            return await CreatePersonModelAsync(location!, accountId!, name, accessTokenFromAccount);
        }

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
        public async Task<CustomPersonModel?> CreatePersonModelAsync(string location, string accountId, string? name = null, string? accessToken = null)
        {
            try
            {
                // API へ新しい Person Model 作成リクエスト送信
                var apiResponse = await _personModelsApiAccess.CreatePersonModelAsync(location, accountId, name, accessToken);
                if (apiResponse == null)
                {
                    return null;
                }

                // ApiCustomPersonModel から CustomPersonModel へマッピング
                return _customPersonMapper.MapFrom(apiResponse);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, name={Name}", location, accountId, name);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parse error: location={Location}, accountId={AccountId}, name={Name}", location, accountId, name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while creating person model: location={Location}, accountId={AccountId}, name={Name}", location, accountId, name);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Face を削除する
        /// </summary>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">Person の ID (GUID 形式)</param>
        /// <param name="faceId">削除する Face の ID (GUID 形式)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        public async Task<bool> DeleteCustomFaceAsync(string personModelId, string personId, string faceId)
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
            return await DeleteCustomFaceAsync(location!, accountId!, personModelId, personId, faceId, accessToken);
        }

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
        public async Task<bool> DeleteCustomFaceAsync(string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null)
        {
            try
            {
                // API へ Face 削除リクエスト送信
                await _personModelsApiAccess.DeleteCustomFaceAsync(location, accountId, personModelId, personId, faceId, accessToken);
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Face not found: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting custom face: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person を削除する
        /// </summary>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">削除する Person の ID (GUID 形式)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        public async Task<bool> DeletePersonAsync(string personModelId, string personId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await DeletePersonAsync(locationFromAccount!, accountIdFromAccount!, personModelId, personId, accessTokenFromAccount);
        }

        /// <summary>
        /// API を呼び出し、指定された Person を削除する
        /// </summary>
        /// <param name="location">Azure のリージョン (例: trial, westus, eastasia)</param>
        /// <param name="accountId">Video Indexer のアカウント ID (GUID 形式)</param>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">削除する Person の ID (GUID 形式)</param>
        /// <param name="accessToken">API への認証用アクセストークン (オプション)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        public async Task<bool> DeletePersonAsync(string location, string accountId, string personModelId, string personId, string? accessToken = null)
        {
            try
            {
                // API へ Person 削除リクエスト送信
                await _personModelsApiAccess.DeletePersonAsync(location, accountId, personModelId, personId, accessToken);
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}", location, accountId, personModelId, personId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}", location, accountId, personModelId, personId);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}", location, accountId, personModelId, personId);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Person not found: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}", location, accountId, personModelId, personId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting person: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}", location, accountId, personModelId, personId);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person Model を削除する
        /// </summary>
        /// <param name="personModelId">削除する Person Model の ID (GUID 形式)</param>
        /// <returns>削除成功時は true を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Person Model が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<bool> DeletePersonModelAsync(string personModelId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await DeletePersonModelAsync(locationFromAccount!, accountIdFromAccount!, personModelId, accessTokenFromAccount);
        }

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
        public async Task<bool> DeletePersonModelAsync(string location, string accountId, string personModelId, string? accessToken = null)
        {
            try
            {
                // API へ Person Model 削除リクエスト送信
                await _personModelsApiAccess.DeletePersonModelAsync(location, accountId, personModelId, accessToken);
                return true;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}", location, accountId, personModelId);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}", location, accountId, personModelId);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access: location={Location}, accountId={AccountId}, personModelId={PersonModelId}", location, accountId, personModelId);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Person Model not found: location={Location}, accountId={AccountId}, personModelId={PersonModelId}", location, accountId, personModelId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting person model: location={Location}, accountId={AccountId}, personModelId={PersonModelId}", location, accountId, personModelId);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Face の画像を取得する
        /// </summary>
        /// <param name="personModelId">Person Model の ID (GUID 形式)</param>
        /// <param name="personId">Person の ID (GUID 形式)</param>
        /// <param name="faceId">取得する Face の ID (GUID 形式)</param>
        /// <returns>取得した Face Picture の URL を返す。エラー時は例外をスロー。</returns>
        /// <exception cref="HttpRequestException">HTTP リクエストが失敗した場合</exception>
        /// <exception cref="UnauthorizedAccessException">認証エラーが発生した場合</exception>
        /// <exception cref="KeyNotFoundException">指定された Face Picture が見つからない場合</exception>
        /// <exception cref="Exception">予期しないエラーが発生した場合</exception>
        public async Task<string> GetCustomFacePictureAsync(string personModelId, string personId, string faceId)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await GetCustomFacePictureAsync(locationFromAccount!, accountIdFromAccount!, personModelId, personId, faceId, accessTokenFromAccount);
        }

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
        public async Task<string> GetCustomFacePictureAsync(string location, string accountId, string personModelId, string personId, string faceId, string? accessToken = null)
        {
            try
            {
                // API へ Face Picture 取得リクエスト送信
                return await _personModelsApiAccess.GetCustomFacePictureAsync(location, accountId, personModelId, personId, faceId, accessToken);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Unauthorized access: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError(ex, "Face Picture not found: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting face picture: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, faceId={FaceId}", location, accountId, personModelId, personId, faceId);
                throw;
            }
        }

        /// <summary>
        /// API を呼び出し、指定された Person に紐づく Face 情報を取得する
        /// </summary>
        /// <param name="personModelId">Person Model の ID (GUID)</param>
        /// <param name="personId">Person の ID (GUID)</param>
        /// <param name="pageSize">取得する Face の数 (オプション)</param>
        /// <param name="skip">スキップする件数 (オプション)</param>
        /// <param name="sourceType">Face のソースタイプ (UploadedPicture / UploadedVideo, オプション)</param>
        /// <returns>Face のリスト</returns>
        public async Task<List<FaceModel>> GetCustomFacesAsync(string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await GetCustomFacesAsync(locationFromAccount!, accountIdFromAccount!, personModelId, personId, pageSize, skip, sourceType, accessTokenFromAccount);
        }


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
        public async Task<List<FaceModel>> GetCustomFacesAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null)
        {
            try
            {
                // API へ Face 一覧取得リクエスト送信
                List<ApiFaceModel> apiResponse = await _personModelsApiAccess.GetCustomFacesAsync(location, accountId, personModelId, personId, pageSize, skip, sourceType, accessToken);
                return apiResponse.Select(_faceModelMapper.MapFrom).ToList();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting custom faces: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
        }

        /// <summary>
        /// 指定された人物のすべての顔のスプライト画像のURLを取得します。
        /// </summary>
        /// <param name="personModelId">人物モデルのID (GUID)</param>
        /// <param name="personId">人物ID (GUID)</param>
        /// <param name="pageSize">取得する結果の数 (オプション)</param>
        /// <param name="skip">スキップする結果の数 (オプション)</param>
        /// <param name="sourceType">顔のソースタイプ (UploadedPicture / UploadedVideo) (オプション)</param>
        /// <returns>スプライト画像のURL</returns>
        public async Task<string> GetCustomFacesSpriteAsync(string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await GetCustomFacesSpriteAsync(locationFromAccount!, accountIdFromAccount!, personModelId, personId, pageSize, skip, sourceType, accessTokenFromAccount);
        }


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
        public async Task<string> GetCustomFacesSpriteAsync(string location, string accountId, string personModelId, string personId, int? pageSize = null, int? skip = null, string? sourceType = null, string? accessToken = null)
        {
            try
            {
                // APIへスプライト画像取得リクエスト送信
                return await _personModelsApiAccess.GetCustomFacesSpriteAsync(location, accountId, personModelId, personId, pageSize, skip, sourceType, accessToken);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting custom faces sprite: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, pageSize={PageSize}, skip={Skip}, sourceType={SourceType}", location, accountId, personModelId, personId, pageSize, skip, sourceType);
                throw;
            }
        }

        /// <summary>
        /// 指定されたアカウント内のすべての人物モデルを取得します。
        /// </summary>
        /// <param name="personNamePrefix">検索する人物の名前のプレフィックス (オプション)</param>
        /// <param name="nameFilter">名前フィルター (オプション)</param>
        /// <returns>人物モデルのリスト</returns>
        public async Task<List<CustomPersonModel>> GetPersonModelsAsync(string? personNamePrefix = null, string? nameFilter = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await GetPersonModelsAsync(locationFromAccount!, accountIdFromAccount!, personNamePrefix, nameFilter, accessTokenFromAccount);
        }

        /// <summary>
        /// 指定されたアカウント内のすべての人物モデルを取得します。
        /// </summary>
        /// <param name="location">APIのロケーション (例: "trial")</param>
        /// <param name="accountId">ビデオインデクサーのアカウントID (GUID)</param>
        /// <param name="personNamePrefix">検索する人物の名前のプレフィックス (オプション)</param>
        /// <param name="nameFilter">名前フィルター (オプション)</param>
        /// <param name="accessToken">APIのアクセストークン (オプション)</param>
        /// <returns>人物モデルのリスト</returns>
        public async Task<List<CustomPersonModel>> GetPersonModelsAsync(string location, string accountId, string? personNamePrefix = null, string? nameFilter = null, string? accessToken = null)
        {
            try
            {
                // APIへ人物モデル一覧取得リクエスト送信
                var apiResponse = await _personModelsApiAccess.GetPersonModelsAsync(location, accountId, personNamePrefix, nameFilter, accessToken);
                return apiResponse.Select(_customPersonMapper.MapFrom).ToList();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personNamePrefix={PersonNamePrefix}, nameFilter={NameFilter}", location, accountId, personNamePrefix, nameFilter);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personNamePrefix={PersonNamePrefix}, nameFilter={NameFilter}", location, accountId, personNamePrefix, nameFilter);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting person models: location={Location}, accountId={AccountId}, personNamePrefix={PersonNamePrefix}, nameFilter={NameFilter}", location, accountId, personNamePrefix, nameFilter);
                throw;
            }
        }

        /// <summary>
        /// 指定された人物モデル内で指定のプレフィックスを持つすべての人物を取得します。
        /// </summary>
        /// <param name="personModelId">人物モデルのID (GUID)</param>
        /// <param name="namePrefix">フィルター対象の名前のプレフィックス (オプション)</param>
        /// <param name="nameFilter">フィルター条件 (オプション)</param>
        /// <param name="pageSize">取得する結果の数 (オプション)</param>
        /// <param name="skip">スキップする結果の数 (オプション)</param>
        /// <param name="sort">ソート条件 ('name', '-score' など) (オプション)</param>
        /// <returns>人物情報のリスト</returns>
        public async Task<List<PersonModel>> GetPersonsAsync(string personModelId, string? namePrefix = null, string? nameFilter = null, int? pageSize = null, int? skip = null, string? sort = null)
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
            return await GetPersonsAsync(location!, accountId!, personModelId, namePrefix, nameFilter, pageSize, skip, sort, accessToken);
        }



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
        public async Task<List<PersonModel>> GetPersonsAsync(string location, string accountId, string personModelId, string? namePrefix = null, string? nameFilter = null, int? pageSize = null, int? skip = null, string? sort = null, string? accessToken = null)
        {
            try
            {
                // API へ人物一覧取得リクエスト送信
                var apiResponse = await _personModelsApiAccess.GetPersonsAsync(location, accountId, personModelId, namePrefix, nameFilter, pageSize, skip, sort, accessToken);

                return apiResponse.Select(_personMapper.MapFrom).ToList();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, namePrefix={NamePrefix}, nameFilter={NameFilter}, pageSize={PageSize}, skip={Skip}, sort={Sort}", location, accountId, personModelId, namePrefix, nameFilter, pageSize, skip, sort);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, namePrefix={NamePrefix}, nameFilter={NameFilter}, pageSize={PageSize}, skip={Skip}, sort={Sort}", location, accountId, personModelId, namePrefix, nameFilter, pageSize, skip, sort);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting persons: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, namePrefix={NamePrefix}, nameFilter={NameFilter}, pageSize={PageSize}, skip={Skip}, sort={Sort}", location, accountId, personModelId, namePrefix, nameFilter, pageSize, skip, sort);
                throw;
            }
        }

        /// <summary>
        /// 指定された人物モデルの名前や識別閾値を更新します。
        /// </summary>
        /// <param name="personModelId">人物モデルのID (GUID)</param>
        /// <param name="newName">更新する新しいモデル名 (オプション)</param>
        /// <param name="personIdentificationThreshold">人物識別閾値 (0.0 - 1.0) (オプション)</param>
        /// <returns>更新された人物モデル情報</returns>
        public async Task<CustomPersonModel?> PatchPersonModelAsync(string personModelId, string? newName = null, double? personIdentificationThreshold = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await PatchPersonModelAsync(locationFromAccount!, accountIdFromAccount!, personModelId, newName, personIdentificationThreshold, accessTokenFromAccount);
        }

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
        public async Task<CustomPersonModel?> PatchPersonModelAsync(string location, string accountId, string personModelId, string? newName = null, double? personIdentificationThreshold = null, string? accessToken = null)
        {
            try
            {
                // API へ人物モデル更新リクエスト送信
                ApiCustomPersonModel? model = await _personModelsApiAccess.PatchPersonModelAsync(location, accountId, personModelId, newName, personIdentificationThreshold, accessToken);
                return model == null ? null : _customPersonMapper.MapFrom(model);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, newName={NewName}, personIdentificationThreshold={PersonIdentificationThreshold}", location, accountId, personModelId, newName, personIdentificationThreshold);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, newName={NewName}, personIdentificationThreshold={PersonIdentificationThreshold}", location, accountId, personModelId, newName, personIdentificationThreshold);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while patching person model: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, newName={NewName}, personIdentificationThreshold={PersonIdentificationThreshold}", location, accountId, personModelId, newName, personIdentificationThreshold);
                throw;
            }
        }

        /// <summary>
        /// Video Indexer API で人物情報を更新します。
        /// </summary>
        /// <param name="personModelId">人物モデルの一意の識別子。</param>
        /// <param name="personId">人物の一意の識別子。</param>
        /// <param name="name">任意の新しい名前。</param>
        /// <param name="description">任意の説明。</param>
        /// <returns>更新が成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
        public async Task<PersonModel?> UpdatePersonAsync(string personModelId, string personId, string? name = null, string? description = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await UpdatePersonAsync(locationFromAccount!, accountIdFromAccount!, personModelId, personId, name, description, accessTokenFromAccount);
        }

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
        public async Task<PersonModel?> UpdatePersonAsync(string location, string accountId, string personModelId, string personId, string? name = null, string? description = null, string? accessToken = null)
        {
            try
            {
                // API へ人物更新リクエスト送信
                ApiPersonModel? personModel = await _personModelsApiAccess.UpdatePersonAsync(location, accountId, personModelId, personId, name, description, accessToken);
                return personModel == null ? null : _personMapper.MapFrom(personModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, name={Name}, description={Description}", location, accountId, personModelId, personId, name, description);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, name={Name}, description={Description}", location, accountId, personModelId, personId, name, description);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating person: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, personId={PersonId}, name={Name}, description={Description}", location, accountId, personModelId, personId, name, description);
                throw;
            }
        }

        /// <summary>
        /// Video Indexer API で人物モデルを更新します。
        /// </summary>
        /// <param name="personModelId">人物モデルの一意の識別子。</param>
        /// <param name="name">任意の新しい名前。</param>
        /// <returns>更新が成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
        public async Task<PersonModel?> UpdatePersonModelAsync(string personModelId, string? name = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? locationFromAccount = account.location;
            string? accountIdFromAccount = account.properties?.id;
            // アクセストークンを取得
            string accessTokenFromAccount = await _authenticationTokenizer.GetAccessToken();
            // API呼び出し
            return await UpdatePersonModelAsync(locationFromAccount!, accountIdFromAccount!, personModelId, name, accessTokenFromAccount);
        }


        /// <summary>
        /// Video Indexer API で人物モデルを更新します。
        /// </summary>
        /// <param name="location">API 呼び出しの Azure リージョン。</param>
        /// <param name="accountId">アカウントの一意の識別子。</param>
        /// <param name="personModelId">人物モデルの一意の識別子。</param>
        /// <param name="name">任意の新しい名前。</param>
        /// <param name="accessToken">認証用のアクセストークン（オプション）。</param>
        /// <returns>更新が成功した場合は ApiPersonModel オブジェクト、それ以外は null を返します。</returns>
        public async Task<PersonModel?> UpdatePersonModelAsync(string location, string accountId, string personModelId, string? name = null, string? accessToken = null)
        {
            try
            {
                // API へ人物モデル更新リクエスト送信
                ApiPersonModel? personModel = await _personModelsApiAccess.UpdatePersonModelAsync(location, accountId, personModelId, name, accessToken);
                return personModel == null ? null : _personMapper.MapFrom(personModel);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Argument error: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}", location, accountId, personModelId, name);
                throw;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "API request failed: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}", location, accountId, personModelId, name);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while updating person model: location={Location}, accountId={AccountId}, personModelId={PersonModelId}, name={Name}", location, accountId, personModelId, name);
                throw;
            }
        }
    }
}
