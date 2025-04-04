using Microsoft.Extensions.Logging;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;

namespace VideoIndexerAccess.Repositories.VideoItemRepository
{
    public class Accounts : IAccounts
    {
        private readonly ILogger<Accounts> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer;
        private readonly IAccounApitAccess _accountAccess;

        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        //private readonly IVideoDownloadApiAccess _videoDownload;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccountMigrationStatusApiAccess _accountMigrationStatusApiAccess;
        private readonly IProjectMigrationApiAccess _projectMigrationApiAccess;
        private readonly IVideoMigrationApiAccess _videoMigrationApiAccess;
        private readonly IAccountMigrationStatusMapper _accountMigrationStatusMapper;
        private readonly IProjectMigrationMapper _projectMigrationMapper;
        private readonly IProjectsMigrationsMapper _projectsMigrationsMapper;
        private readonly IVideoMigrationMapper _videoMigrationMapper;
        private readonly IVideoMigrationsListMapper _videoMigrationsListMapper;

        private const string ParamName = "account";

        public Accounts(ILogger<Accounts> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IApiResourceConfigurations apiResourceConfigurations, IAccountRepository accountRepository, IAccountMigrationStatusApiAccess accountMigrationStatusApiAccess, IProjectMigrationApiAccess projectMigrationApiAccess, IVideoMigrationApiAccess videoMigrationApiAccess, IAccountMigrationStatusMapper accountMigrationStatusMapper, IProjectMigrationMapper projectMigrationMapper, IProjectsMigrationsMapper projectsMigrationsMapper, IVideoMigrationMapper videoMigrationMapper, IVideoMigrationsListMapper videoMigrationsListMapper)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountRepository = accountRepository;
            _accountMigrationStatusApiAccess = accountMigrationStatusApiAccess;
            _projectMigrationApiAccess = projectMigrationApiAccess;
            _videoMigrationApiAccess = videoMigrationApiAccess;

            _accountMigrationStatusMapper = accountMigrationStatusMapper;
            _projectMigrationMapper = projectMigrationMapper;
            _projectsMigrationsMapper = projectsMigrationsMapper;
            _videoMigrationMapper = videoMigrationMapper;
            _videoMigrationsListMapper = videoMigrationsListMapper;
        }

        /// <summary>
        /// アカウントの移行ステータスを取得する
        /// Get Account Migration Status
        /// </summary>
        /// <returns>アカウントの移行ステータスモデル</returns>
        /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
        public async Task<AccountMigrationStatusModel?> GetAccountMigrationStatusAsync()
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
            // アカウントの移行ステータスを取得して返す
            return await GetAccountMigrationStatusAsync(location!, accountId!, accessToken);
        }

        /// <summary>
        /// 指定されたロケーションとアカウントIDでアカウントの移行ステータスを取得する
        /// Get Account Migration Status
        /// </summary>
        /// <param name="location">アカウントのロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>アカウントの移行ステータスモデル</returns>
        public async Task<AccountMigrationStatusModel?> GetAccountMigrationStatusAsync(string location, string accountId, string? accessToken = null)
        {
            // APIから取得した移行ステータスをマッピングして返す
            return _accountMigrationStatusMapper.MapFrom(await _accountMigrationStatusApiAccess.GetAccountMigrationStatusAsync(location, accountId, accessToken));
        }

        /// <summary>
        /// プロジェクトの移行ステータスを取得する
        /// Get Project Migration Status
        /// </summary>
        /// <returns>プロジェクトの移行ステータスモデル</returns>
        /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
        public async Task<ProjectMigrationModel?> GetProjectMigrationAsync(string projectId)
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
            return await GetProjectMigrationAsync(location!, accountId!, projectId, accessToken);
        }

        /// <summary>
        /// 指定されたロケーションとアカウントID、プロジェクトIDでプロジェクトの移行ステータスを取得する
        /// Get Project Migration Status
        /// </summary>
        /// <param name="location">アカウントのロケーション</param>
        /// <param name="accountId">アカウントID</param>
        /// <param name="projectId">プロジェクトID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>プロジェクトの移行ステータスモデル</returns>
        public async Task<ProjectMigrationModel?> GetProjectMigrationAsync(string location, string accountId, string projectId, string? accessToken = null)
        {
            return _projectMigrationMapper.MapFrom(await _projectMigrationApiAccess.GetProjectMigrationAsync(location, accountId, projectId, accessToken));
        }

        // Get Project Migrations

        public async Task<ProjectsMigrationsModel?> GetProjectMigrationsAsync(string location, string accountId, string accessToken, int? pageSize = null, int? skip = null, string[]? states = null)
        {
            return _projectsMigrationsMapper.MapFrom(await _projectMigrationApiAccess.GetProjectMigrationsAsync(location, accountId, accessToken, pageSize, skip, states));
        }

        /// <summary>
        /// プロジェクトの移行ステータスのリストを取得する
        /// Get Project Migrations
        /// </summary>
        /// <returns>プロジェクトの移行ステータスモデルのリスト</returns>
        /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
        public async Task<ProjectsMigrationsModel?> GetProjectMigrationsAsync(int? pageSize = null, int? skip = null, string[]? states = null)
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
            return await GetProjectMigrationsAsync(location!, accountId!, accessToken, pageSize, skip, states);
        }

        // Get Video Migration

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
        public async Task<VideoMigrationModel?> GetVideoMigrationAsync(string location, string accountId, string videoId, string? accessToken = null)
        {
            return _videoMigrationMapper.MapFrom(await _videoMigrationApiAccess.GetVideoMigrationAsync(location, accountId, videoId, accessToken));
        }


        /// <summary>
        /// 指定されたビデオIDでビデオの移行ステータスを取得する
        /// Get Video Migration
        /// </summary>
        /// <param name="videoId">ビデオID</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ビデオの移行ステータスモデル</returns>
        /// <exception cref="ArgumentNullException">アカウントが見つからない場合にスローされる例外</exception>
        public async Task<VideoMigrationModel?> GetVideoMigrationAsync(string videoId, string? accessToken = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            accessToken ??= await _authenticationTokenizer.GetAccessToken();
            // ビデオの移行ステータスを取得して返す
            return await GetVideoMigrationAsync(location!, accountId!, videoId, accessToken);
        }

        // Get Video Migrations

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
        public async Task<VideoMigrationsListModel?> GetVideoMigrationsAsync(string location, string accountId, int? pageSize = null, int? skip = null, List<string>? states = null, string? accessToken = null)
        {
            return _videoMigrationsListMapper.MapFrom(await _videoMigrationApiAccess.GetVideoMigrationsAsync(location, accountId, pageSize, skip, states, accessToken));
        }

        /// <summary>
        /// ビデオの移行ステータスのリストを取得する
        /// Get Video Migrations
        /// </summary>
        /// <param name="pageSize">ページサイズ（オプション）</param>
        /// <param name="skip">スキップする項目数（オプション）</param>
        /// <param name="states">状態フィルター（オプション）</param>
        /// <param name="accessToken">アクセストークン（オプション）</param>
        /// <returns>ビデオの移行ステータスモデルのリスト</returns>
        public async Task<VideoMigrationsListModel?> GetVideoMigrationsAsync(int? pageSize = null, int? skip = null, List<string>? states = null, string? accessToken = null)
        {
            // アカウント情報を取得し、存在しない場合は例外をスロー
            var account = await _accountAccess.GetAccountAsync(_apiResourceConfigurations.ViAccountName) ?? throw new ArgumentNullException(paramName: ParamName);
            // アカウント情報のチェック
            _accountRepository.CheckAccount(account);
            // アカウントのロケーションとIDを取得
            string? location = account.location;
            string? accountId = account.properties?.id;
            // アクセストークンを取得
            accessToken ??= await _authenticationTokenizer.GetAccessToken();
            // ビデオの移行ステータスのリストを取得して返す
            return await GetVideoMigrationsAsync(location!, accountId!, pageSize, skip, states, accessToken);
        }

    }
}
