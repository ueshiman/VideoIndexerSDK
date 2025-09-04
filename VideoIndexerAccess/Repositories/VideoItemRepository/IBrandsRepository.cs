using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IBrandsRepository
{
    /// <summary>
    /// 指定されたロケーションとアカウントIDを使用して、新しいブランドモデルを作成します。
    /// </summary>
    /// <param name="location">APIのリージョンを指定します。</param>
    /// <param name="accountId">アカウントIDを指定します。</param>
    /// <param name="brandModel">作成するブランドモデル</param>
    /// <param name="accessToken">アクセストークン（省略可能）。指定しない場合はデフォルトのトークンが使用されます。</param>
    /// <returns>ブランドモデルの作成に成功した場合はtrue、失敗した場合はfalseを返します。</returns>
    Task<bool> CreateBrandAsync(string location, string accountId, BrandModel brandModel, string? accessToken = null);

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
    Task<bool> CreateBrandAsync(BrandModel brandModel);

    /// <summary>
    /// 指定したブランドIDのブランドモデルを削除します。
    /// </summary>
    /// <param name="id">削除対象のブランドID</param>
    /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
    Task<bool> DeleteBrandAsync(int id);

    /// <summary>
    /// 指定されたロケーション、アカウントID、ブランドID、およびアクセストークンを使用して
    /// ブランドモデルをVideo Indexer APIから削除します。
    /// </summary>
    /// <param name="location">APIのリージョン（例: "japaneast" など）</param>
    /// <param name="accountId">Video IndexerアカウントのID</param>
    /// <param name="id">削除対象のブランドモデルのID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>削除に成功した場合はtrue、失敗した場合はfalse</returns>
    Task<bool> DeleteBrandAsync(string location, string accountId, int id, string? accessToken);

    /// <summary>
    /// 指定されたロケーション、アカウントID、ブランドID、アクセストークンを使用して
    /// ブランドモデルを取得します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="id">ブランドID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>取得したブランドモデル。存在しない場合はnull</returns>
    Task<BrandModel?> GetBrandAsync(string location, string accountId, int id, string? accessToken);

    /// <summary>
    /// 指定したブランドIDのブランドモデルを取得します。
    /// </summary>
    /// <param name="id">取得対象のブランドID</param>
    /// <returns>取得したブランドモデル。存在しない場合はnull</returns>
    Task<BrandModel?> GetBrandAsync(int id);

    /// <summary>
    /// Video Indexer API から全ブランドモデルを取得します。
    /// </summary>
    /// <returns>取得したブランドモデルの配列。存在しない場合は null。</returns>
    Task<BrandModel[]?> GetBrandsAsync();

    /// <summary>
    /// 指定されたロケーション、アカウントID、アクセストークンを使用して
    /// Video Indexer API から全ブランドモデルを取得します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>取得したブランドモデルの配列。存在しない場合は null。</returns>
    Task<BrandModel[]?> GetBrandsAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// Video Indexer API からブランドモデル設定を取得し、パースして返します。
    /// </summary>
    /// <returns>取得したブランドモデル設定。存在しない場合は null。</returns>
    Task<BrandModelSettingsModel?> FetchAndParseBrandSettingsAsync();

    /// <summary>
    /// 指定されたロケーション、アカウントID、アクセストークンを使用して
    /// ブランドモデル設定を取得し、パースして返します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <returns>取得したブランドモデル設定。存在しない場合は null。</returns>
    Task<BrandModelSettingsModel?> FetchAndParseBrandSettingsAsync(string location, string accountId, string? accessToken);

    /// <summary>
    /// ブランドモデルを更新します。
    /// </summary>
    /// <param name="brand">更新するブランドモデル</param>
    /// <returns>更新結果のJSONレスポンス。失敗時はnull。</returns>
    Task<string?> UpdateBrandAsync(BrandModel brand);

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
    Task<string?> UpdateBrandAsync(string location, string accountId, int id, BrandModel brand, string? accessToken);

    /// <summary>
    /// ブランドモデル設定を更新します。
    /// </summary>
    /// <param name="settings">更新するブランドモデル設定</param>
    /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
    Task<string> UpdateBrandModelSettingsAsync(BrandModelSettingsModel settings);

    /// <summary>
    /// 指定されたロケーション、アカウントID、アクセストークンを使用して
    /// ブランドモデル設定を更新します。
    /// </summary>
    /// <param name="location">APIのリージョン</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（省略可能）</param>
    /// <param name="settings">更新するブランドモデル設定</param>
    /// <returns>更新されたブランドモデル設定のJSONレスポンス</returns>
    Task<string> UpdateBrandModelSettingsAsync(string location, string accountId, string? accessToken, BrandModelSettingsModel settings);
}