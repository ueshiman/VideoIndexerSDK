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
    /// <summary>
    /// Video Indexer API を利用してブランドモデルの管理を行うリポジトリクラス
    /// </summary>
    public class BrandsRepository : IBrandsRepository
    {
        // ロガーインスタンス
        private readonly ILogger<BrandsRepository> _logger;

        // アクセストークン取得用インターフェース
        private readonly IAuthenticationTokenizer _authenticationTokenizer;

        // アカウント情報取得用インターフェース
        private readonly IAccounApitAccess _accountAccess;

        // アカウント検証用リポジトリ
        private readonly IAccountRepository _accountRepository;

        // ブランドAPIアクセス用インターフェース
        private readonly IBrandsApiAccess _brandslApiAccess;

        // ブランドマッピング用インターフェース
        private readonly IBrandMapper _brandMapper;

        // BrandModel <-> ApiBrandModel マッピング用インターフェース
        private readonly IBrandModelMapper _brandModelMapper;

        // APIリソース設定
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        // ブランドモデル設定マッピング用インターフェース
        // ブランドモデルの設定情報（有効/無効やビルトインブランド利用可否など）のマッピングに使用
        private readonly IBrandModelSettingsMapper _brandModelSettingsMapper;

        // 例外スロー時のパラメータ名
        private const string ParamName = "account";

        /// <summary>
        /// コンストラクタ。依存性注入で各種サービスを受け取る
        /// </summary>
        public BrandsRepository(
            ILogger<BrandsRepository> logger,
            IAuthenticationTokenizer authenticationTokenizer,
            IBrandsApiAccess brandslApiAccess,
            IApiResourceConfigurations apiResourceConfigurations,
            IAccounApitAccess accountAccess,
            IAccountRepository accountRepository,
            IBrandMapper brandMapper,
            IBrandModelMapper brandModelMapper, IBrandModelSettingsMapper brandModelSettingsMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _brandslApiAccess = brandslApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _brandMapper = brandMapper;
            _brandModelMapper = brandModelMapper;
            _brandModelSettingsMapper = brandModelSettingsMapper;
        }

        /// <summary>
        /// 指定されたロケーションとアカウントIDを使用して、新しいブランドモデルを作成します。
        /// </summary>
        /// <param name="location">APIのリージョンを指定します。</param>
        /// <param name="accountId">アカウントIDを指定します。</param>
        /// <param name="brandModel">作成するブランドモデル</param>
        /// <param name="accessToken">アクセストークン（省略可能）。指定しない場合はデフォルトのトークンが使用されます。</param>
        /// <returns>ブランドモデルの作成に成功した場合はtrue、失敗した場合はfalseを返します。</returns>
        public async Task<bool> CreateBrandAsync(string location, string accountId, BrandModel brandModel, string? accessToken = null)
        {
            try
            {
                // BrandModel を API 用モデルにマッピングし、API へリクエスト送信
                var response = await _brandslApiAccess.CreateApiBrandModelAsync(location, accountId, accessToken, _brandModelMapper.MapToApiBrandModel(brandModel));
                if (response.IsSuccessStatusCode)
                {
                    // レスポンスボディを取得し、APIブランドモデルにパース
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var apiBrandModel = _brandslApiAccess.ParseApiBrandModel(jsonResponse);

                    switch (response.StatusCode)
                    {
                        case HttpStatusCode.Created:
                            // 作成成功時はデバッグログ出力
                            _logger.LogDebug("Brand created successfully: {apiBrandModel}", apiBrandModel);
                            return true;
                        default:
                            // その他の成功ステータス時も警告ログを出力しtrueを返す
                            _logger.LogWarning("Brand created with status code: {StatusCode}", response.StatusCode);
                            return true;
                    }
                }
                else
                {
                    // 失敗時はエラーログ出力
                    _logger.LogError("Failed to create brand: {StatusCode}", response?.StatusCode);
                }

                return false;
            }
            catch (Exception exception)
            {
                // 例外発生時はエラーログ出力
                _logger.LogError(exception, "Error occurred while creating brand");
                return false;
            }
        }


        /// <summary>
        /// アカウント情報を取得し、存在しない場合は例外をスローします。
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// BrandsRepository クラスは、Video Indexer API を使用してブランドモデルを管理するためのリポジトリです。
        /// </summary>
        /// 主な機能:
        /// - ブランドモデルの作成
        /// - アカウント情報の取得と検証
        /// - APIとの通信を通じたブランドデータの操作
        /// 
        /// 使用する依存コンポーネント:
        /// - ILogger<BrandsRepository>: ログ出力用
        ///     - IAuthenticationTokenizer: アクセストークンの取得
        ///     - IAccounApitAccess: アカウント情報の取得
        ///     - IAccountRepository: アカウント情報の検証
        ///     - IBrandsApiAccess: ブランドモデルに関するAPI操作
        ///     - IApiResourceConfigurations: APIリソース設定
        /// 
        ///     .NET バージョン: .NET 8
        ///     C# バージョン: 12.0
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> CreateBrandAsync(BrandModel brandModel)
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
            // ブランドモデル作成API呼び出し（brandModel未指定版）
            return await CreateBrandAsync(location!, accountId!, brandModel, accessToken);
        }

        /// <summary>
        /// 指定したブランドIDのブランドモデルを削除します。
        /// </summary>
        /// <param name="id">削除対象のブランドID</param>
        /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> DeleteBrandAsync(int id)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            // ブランドモデル削除API呼び出し
            return await DeleteBrandAsync(location!, accountId!, 0, accessToken);
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、ブランドID、およびアクセストークンを使用して
        /// ブランドモデルをVideo Indexer APIから削除します。
        /// </summary>
        /// <param name="location">APIのリージョン（例: "japaneast" など）</param>
        /// <param name="accountId">Video IndexerアカウントのID</param>
        /// <param name="id">削除対象のブランドモデルのID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> DeleteBrandAsync(string location, string accountId, int id, string? accessToken)
        {
            try
            {
                // ブランドモデル削除API呼び出し
                var response = await _brandslApiAccess.DeleteApiBrandModelAsync(location, accountId, id, accessToken);

                if (response.IsSuccessStatusCode)
                {
                    // 削除成功時は情報ログを出力し、trueを返す
                    _logger.LogInformation("Successfully deleted brand model. location: {Location}, accountId: {AccountId}, id: {Id}, StatusCode: {StatusCode}", location, accountId, id, response.StatusCode);
                    return true;
                }
                else
                {
                    // 削除失敗時は警告ログを出力し、falseを返す
                    _logger.LogWarning("Failed to delete brand model. StatusCode: {StatusCode}, location: {Location}, accountId: {AccountId}, id: {Id}", response.StatusCode, location, accountId, id);
                    return false;
                }
            }
            catch (Exception e)
            {
                // 例外発生時はエラーログを出力し、falseを返す
                _logger.LogError(e, "An error occurred while deleting the brand model. location: {Location}, accountId: {AccountId}, id: {Id}", location, accountId, id);
                return false;
            }
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、ブランドID、アクセストークンを使用して
        /// ブランドモデルを取得します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="id">ブランドID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>取得したブランドモデル。存在しない場合はnull</returns>
        public async Task<BrandModel?> GetBrandAsync(string location, string accountId, int id, string? accessToken)
        {
            try
            {
                _logger.LogDebug("Getting brand model. location: {Location}, accountId: {AccountId}, id: {Id}", location, accountId, id);
                // APIからブランドモデルを取得
                var apiBrandModel = await _brandslApiAccess.GetApiBrandModeAsync(location, accountId, id, accessToken);
                if (apiBrandModel is null)
                {
                    // ブランドモデルが見つからない場合は警告ログを出力しnullを返す
                    _logger.LogWarning("Brand model not found. location: {Location}, accountId:Map {AccountId}, id: {Id}", location, accountId, id);
                    return null;
                }

                // ApiBrandModel から BrandModel へマッピング
                var brandModel = _brandModelMapper.MapFrom(apiBrandModel);
                _logger.LogTrace("Brand model successfully mapped. location: {Location}, accountId: {AccountId}, id: {Id}", location, accountId, id);
                return brandModel;
            }
            catch (Exception e)
            {
                // 例外発生時はエラーログを出力し、再スロー
                _logger.LogError(e, "An error occurred while retrieving the brand model. location: {Location}, accountId: {AccountId}, id: {Id}", location, accountId, id);
                throw;
            }
        }


        /// <summary>
        /// 指定したブランドIDのブランドモデルを取得します。
        /// </summary>
        /// <param name="id">取得対象のブランドID</param>
        /// <returns>取得したブランドモデル。存在しない場合はnull</returns>
        public async Task<BrandModel?> GetBrandAsync(int id)
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

            // ブランドモデル取得API呼び出し

            return await GetBrandAsync(location!, accountId!, id, accessToken);

        }


        /// <summary>
        /// Video Indexer API から全ブランドモデルを取得します。
        /// </summary>
        /// <returns>取得したブランドモデルの配列。存在しない場合は null。</returns>
        public async Task<BrandModel[]?> GetApiBrandsAsync()
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
            // ブランドモデル取得API呼び出し
            return await GetApiBrandsAsync(location!, accountId!, accessToken);
        }


        /// <summary>
        /// 指定されたロケーション、アカウントID、アクセストークンを使用して
        /// Video Indexer API から全ブランドモデルを取得します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>取得したブランドモデルの配列。存在しない場合は null。</returns>
        public async Task<BrandModel[]?> GetApiBrandsAsync(string location, string accountId, string? accessToken)
        {
            try
            {
                // APIからブランドモデルの配列を取得
                var apiBrandModels = await _brandslApiAccess.GetApiBrandsAsync(location, accountId, accessToken);
                if (apiBrandModels == null || apiBrandModels.Length == 0)
                {
                    _logger.LogInformation("No brand models found. location: {Location}, accountId: {AccountId}", location, accountId);
                    return null;
                }

                // ApiBrandModel配列をBrandModel配列にマッピング
                var brandModels = apiBrandModels
                    .Select(apiBrand => _brandModelMapper.MapFrom(apiBrand))
                    .ToArray();

                _logger.LogDebug("Retrieved {Count} brand models. location: {Location}, accountId: {AccountId}", brandModels.Length, location, accountId);
                return brandModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving brand models. location: {Location}, accountId: {AccountId}", location, accountId);
                return null;
            }
        }

        /// <summary>
        /// Video Indexer API からブランドモデル設定を取得し、パースして返します。
        /// </summary>
        /// <returns>取得したブランドモデル設定。存在しない場合は null。</returns>
        public async Task<BrandModelSettingsModel?> FetchAndParseBrandSettingsAsync()
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
            // ブランドモデル設定取得API呼び出し
            return await FetchAndParseBrandSettingsAsync(location!, accountId!, accessToken);
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、アクセストークンを使用して
        /// ブランドモデル設定を取得し、パースして返します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>取得したブランドモデル設定。存在しない場合は null。</returns>
        public async Task<BrandModelSettingsModel?> FetchAndParseBrandSettingsAsync(string location, string accountId, string? accessToken)
        {
            ApiBrandModelSettingsModel? apiBrandModelSettingsModel = await _brandslApiAccess.FetchAndParseApiBrandModelSettingsAsync(location, accountId, accessToken);
            //ApiBrandModelSettingsModel? apiBrandModelSettingsModel = await _brandslApiAccess.FetchAndParseApiBrandModelSettingsAsync(location, accountId, accessToken) ?? throw new InvalidOperationException("Failed to parse API brand model settings from the response.");
            _logger.LogDebug("Fetched and parsed API brand model settings. location: {Location}, accountId: {AccountId}", location, accountId);
            // ApiBrandModelSettingsModel から BrandModelSettingsModel へマッピング
            if (apiBrandModelSettingsModel is null)
            {
                _logger.LogError("No brand model settings found. location: {Location}, accountId: {AccountId}", location, accountId);
                return null;
            }
            else
            {
                return _brandModelSettingsMapper.MapFrom(apiBrandModelSettingsModel);
            }
        }

        /// <summary>
        /// ブランドモデルを更新します。
        /// </summary>
        /// <param name="brand">更新するブランドモデル</param>
        /// <returns>更新結果のJSONレスポンス。失敗時はnull。</returns>
        public async Task<string?> UpdateBrandAsync(BrandModel brand)
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
            // ブランドIDがnullの場合は例外
            if (brand.Id == null)
            {
                throw new ArgumentException("BrandModel.Id is required for update.", nameof(brand));
            }

            // ブランド更新API呼び出し
            return await UpdateBrandAsync(location!, accountId!, brand.Id.Value, brand, accessToken);
        }

        /// <summary>
        /// 指定されたロケーション、アカウントID、ブランドID、アクセストークンを使用して
        /// ブランドモデルを更新します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="id">ブランドID</param>
        /// <param name="brand">更新するブランドモデル</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <returns>更新結果のJSONレスポンス。失敗時はnull。</returns>
        public async Task<string?> UpdateBrandAsync(string location, string accountId, int id, BrandModel brand, string? accessToken)
        {
            try
            {
                // BrandModel を API 用モデルにマッピング
                var apiBrandModel = _brandModelMapper.MapToApiBrandModel(brand);

                // ブランド更新API呼び出し
                var jsonResponse = await _brandslApiAccess.UpdateApiBrandModelAsync(location, accountId, id, accessToken, apiBrandModel);

                // レスポンスの内容をログ出力
                _logger.LogInformation("Brand updated. location: {Location}, accountId: {AccountId}, id: {Id}, response: {Response}", location, accountId, id, jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating brand. location: {Location}, accountId: {AccountId}, id: {Id}", location, accountId, id);
                return null;
            }
        }


        /// <summary>
        /// ブランドモデル設定を更新します。
        /// </summary>
        /// <param name="settings">更新するブランドモデル設定</param>
        /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
        public async Task<string> UpdateApiBrandModelSettingsAsync(BrandModelSettingsModel settings)
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

            // ブランドモデル設定更新API呼び出し
            return await UpdateApiBrandModelSettingsAsync(location!, accountId!, accessToken, settings);
        }


        /// <summary>
        /// 指定されたロケーション、アカウントID、アクセストークンを使用して
        /// ブランドモデル設定を更新します。
        /// </summary>
        /// <param name="location">APIのリージョン</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（省略可能）</param>
        /// <param name="settings">更新するブランドモデル設定</param>
        /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
        public async Task<string> UpdateApiBrandModelSettingsAsync(string location, string accountId, string? accessToken, BrandModelSettingsModel settings)
        {
            try
            {
                // BrandModelSettingsModel を API 用モデルにマッピング
                var apiSettings = _brandModelSettingsMapper.MapToApiBrandModelSettingsModel(settings);

                // API へ更新リクエスト送信
                var jsonResponse = await _brandslApiAccess.UpdateApiBrandModelSettingsAsync(location, accountId, accessToken, apiSettings);

                // レスポンス内容をログ出力
                _logger.LogInformation("Brand model settings updated. location: {Location}, accountId: {AccountId}, response: {Response}", location, accountId, jsonResponse);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating brand model settings. location: {Location}, accountId: {AccountId}", location, accountId);
                throw;
            }
        }
    }
}
