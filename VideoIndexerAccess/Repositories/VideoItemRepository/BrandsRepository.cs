using Azure;
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
    public class BrandsRepository
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
            IBrandModelMapper brandModelMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _brandslApiAccess = brandslApiAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountAccess = accountAccess;
            _accountRepository = accountRepository;
            _brandMapper = brandMapper;
            _brandModelMapper = brandModelMapper;
        }
        
        /// <summary>
        /// 指定されたロケーションとアカウントIDを使用して、新しいブランドモデルを作成します。
        /// </summary>
        /// <param name="location">APIのリージョンを指定します。</param>
        /// <param name="accountId">アカウントIDを指定します。</param>
        /// <param name="brandModel">作成するブランドモデル</param>
        /// <param name="accessToken">アクセストークン（省略可能）。指定しない場合はデフォルトのトークンが使用されます。</param>
        /// <returns>ブランドモデルの作成に成功した場合はtrue、失敗した場合はfalseを返します。</returns>
        public async Task<bool> CreateApiBrandModelAsync(string location, string accountId, BrandModel brandModel, string? accessToken = null)
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
        public async Task<bool> CreateApiBrandModelAsync(BrandModel brandModel)
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
            return await CreateApiBrandModelAsync(location!, accountId!, brandModel, accessToken);
        }

        /// <summary>
        /// 指定したブランドIDのブランドモデルを削除します。
        /// </summary>
        /// <param name="id">削除対象のブランドID</param>
        /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
        public async Task<bool> DeleteApiBrandModelAsync(int id)
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
            return await DeleteApiBrandModelAsync(location!, accountId!, 0, accessToken);
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
        public async Task<bool> DeleteApiBrandModelAsync(string location, string accountId, int id, string? accessToken)
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
        public async Task<BrandModel?> GetBrandModeAsync(string location, string accountId, int id, string? accessToken)
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

        public async Task<BrandModel?> GetBrandModeAsync(int id)
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
            return await GetBrandModeAsync(location!, accountId!, id, accessToken);
        }

    }
}
