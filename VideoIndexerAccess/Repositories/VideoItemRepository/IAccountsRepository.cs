using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.VideoItemRepository;

public interface IAccountsRepository
{
    /// <summary>
    /// アカウントの移行ステータスを取得する
    /// Get Account Migration Status
    /// </summary>
    /// <returns>アカウントの移行ステータスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
    Task<AccountMigrationStatusModel?> GetAccountMigrationStatusAsync();

    /// <summary>
    /// 指定されたロケーションとアカウントIDでアカウントの移行ステータスを取得する
    /// Get Account Migration Status
    /// </summary>
    /// <param name="location">アカウントのロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>アカウントの移行ステータスモデル</returns>
    Task<AccountMigrationStatusModel?> GetAccountMigrationStatusAsync(string location, string accountId, string? accessToken = null);

    /// <summary>
    /// プロジェクトの移行ステータスを取得する
    /// Get Project Migration Status
    /// </summary>
    /// <returns>プロジェクトの移行ステータスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
    Task<ProjectMigrationModel?> GetProjectMigrationAsync(string projectId);

    /// <summary>
    /// 指定されたロケーションとアカウントID、プロジェクトIDでプロジェクトの移行ステータスを取得する
    /// Get Project Migration Status
    /// </summary>
    /// <param name="location">アカウントのロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="projectId">プロジェクトID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>プロジェクトの移行ステータスモデル</returns>
    Task<ProjectMigrationModel?> GetProjectMigrationAsync(string location, string accountId, string projectId, string? accessToken = null);

    Task<ProjectsMigrationsModel?> GetProjectMigrationsAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null);

    /// <summary>
    /// プロジェクトの移行ステータスのリストを取得する
    /// Get Project Migrations
    /// </summary>
    /// <returns>プロジェクトの移行ステータスモデルのリスト</returns>
    /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
    Task<ProjectsMigrationsModel?> GetProjectMigrationsAsync(int? pageSize = null, int? skip = null, string[]? states = null);

    /// <summary>
    /// 指定されたビデオIDでビデオの移行ステータスを取得する
    /// Get Video Migration
    /// </summary>
    /// <param name="location">アカウントのロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="videoId">ビデオID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ビデオの移行ステータスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
    Task<VideoMigrationModel?> GetVideoMigrationAsync(string location, string accountId, string videoId, string? accessToken = null);   

    /// <summary>
    /// 指定されたビデオIDでビデオの移行ステータスを取得する
    /// Get Video Migration
    /// </summary>
    /// <param name="videoId">ビデオID</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ビデオの移行ステータスモデル</returns>
    /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
    Task<VideoMigrationModel?> GetVideoMigrationAsync(string videoId);  

    /// <summary>
    /// ビデオの移行ステータスのリストを取得する
    /// Get Video Migrations
    /// </summary>
    /// <param name="location">アカウントのロケーション</param>
    /// <param name="accountId">アカウントID</param>
    /// <param name="pageSize">ページサイズ（オプション）</param>
    /// <param name="skip">スキップする項目数（オプション）</param>
    /// <param name="states">状態フィルター（オプション）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ビデオの移行ステータスモデルのリスト</returns>
    Task<VideoMigrationsListModel?> GetVideoMigrationsAsync(string location, string accountId, int? pageSize = null, int? skip = null, List<string>? states = null, string? accessToken = null);

    /// <summary>
    /// ビデオの移行ステータスのリストを取得する
    /// Get Video Migrations
    /// </summary>
    /// <param name="pageSize">ページサイズ（オプション）</param>
    /// <param name="skip">スキップする項目数（オプション）</param>
    /// <param name="states">状態フィルター（オプション）</param>
    /// <param name="accessToken">アクセストークン（オプション）</param>
    /// <returns>ビデオの移行ステータスモデルのリスト</returns>
    Task<VideoMigrationsListModel?> GetVideoMigrationsAsync(int? pageSize = null, int? skip = null, List<string>? states = null);
}