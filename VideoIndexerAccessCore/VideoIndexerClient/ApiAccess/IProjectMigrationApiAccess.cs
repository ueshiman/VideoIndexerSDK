using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IProjectMigrationApiAccess
{
    /// <summary>
    /// プロジェクトのマイグレーション情報を取得します。
    /// Get Project Migration
    /// Try it Get project migration
    /// </summary>
    /// <param name="location">ロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <returns>プロジェクトのマイグレーション情報</returns>
    Task<ApiProjectMigrationModel> GetProjectMigrationAsync(string location, string accountId, string projectId, string? accessToken = null);

    /// <summary>
    /// API からプロジェクトのマイグレーションデータを JSON 文字列として取得します。
    /// </summary>
    /// <param name="location">ロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <returns>プロジェクトのマイグレーションデータの JSON 文字列</returns>
    Task<string> GetProjectMigrationJsonAsync(string location, string accountId, string projectId, string? accessToken);

    /// <summary>
    /// 取得した JSON 文字列を ProjectMigrationResponse オブジェクトに変換します。
    /// </summary>
    /// <param name="json">JSON 文字列</param>
    /// <returns>プロジェクトのマイグレーション情報</returns>
    ApiProjectMigrationModel ParseProjectMigrationJson(string json);

    /// <summary>
    /// プロジェクトのマイグレーション情報を取得します。
    /// </summary>
    /// <param name="location">ロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップする項目数</param>
    /// <param name="states">状態フィルター</param>
    /// <returns>プロジェクトのマイグレーション情報のリスト</returns>
    Task<ApiProjectsMigrations> GetProjectMigrationsAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null);

    /// <summary>
    /// API からプロジェクトのマイグレーションデータを JSON 文字列として取得します。
    /// </summary>
    /// <param name="location">ロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン</param>
    /// <param name="pageSize">ページサイズ</param>
    /// <param name="skip">スキップする項目数</param>
    /// <param name="states">状態フィルター</param>
    /// <returns>プロジェクトのマイグレーションデータの JSON 文字列</returns>
    Task<string> GetProjectMigrationsJsonAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null);

    /// <summary>
    /// 取得した JSON 文字列を ApiProjectsMigrations オブジェクトに変換します。
    /// </summary>
    /// <param name="jsonResponse">JSON 文字列</param>
    /// <returns>プロジェクトのマイグレーション情報のリスト</returns>
    ApiProjectsMigrations ParseJsonResponse(string jsonResponse);
}