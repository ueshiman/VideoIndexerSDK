using Microsoft.Extensions.Logging;
using System.Text.Json;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccess.Repositories.VideoItemRepository;

namespace VideoIndexBoot.Boots
{

    public class BootAccounts
    {
        private readonly ILogger<BootAccounts> _logger;
        private readonly IAccountsRepository _accountsRepository;

        public BootAccounts(ILogger<BootAccounts> logger, IAccountsRepository accountsRepository)
        {
            _logger = logger;
            _accountsRepository = accountsRepository;
        }

        public async Task Run()
        {
            await RunAccountMigrationStatusAsync();
        }


        public async Task RunAccountMigrationStatusAsync()
        {
            _logger.LogInformation("Booting Accounts...");
            // ここにアカウントの初期化処理を追加
            // 例: アカウント情報を取得して表示する
            AccountMigrationStatusModel? accounts = await _accountsRepository.GetAccountMigrationStatusAsync();

            if (accounts is null)
            {
                _logger.LogError("No account migration status found.");
                return;
            }

            string jsonResult = JsonSerializer.Serialize<AccountMigrationStatusModel>(accounts);
            _logger.LogInformation("Accounts found: {JsonResult}", jsonResult);
        }

        
        public async Task RunProjectMigrationAsync(string projectId)
        {
            _logger.LogInformation("Booting Project Migration Status...");
            // ここにプロジェクトの初期化処理を追加
            // 例: プロジェクト情報を取得して表示する
            ProjectMigrationModel? project = await _accountsRepository.GetProjectMigrationAsync(projectId);
            if (project is null)
            {
                _logger.LogError("No project migration status found.");
                return;
            }
            string jsonResult = JsonSerializer.Serialize<ProjectMigrationModel>(project);
            _logger.LogInformation("Project found: {JsonResult}", jsonResult);
        }

        public async Task RunProjectMigrationAsync(ProjectsMigrationsModel projects)
        {
            foreach (var project in projects.Results)
            {
                if (project?.ProjectId is null)
                {
                    _logger.LogError("ProjectId is null.");
                    continue;
                }

                await RunProjectMigrationAsync(project.ProjectId);
            }
        }

        public async Task<ProjectsMigrationsModel> RunProjectsMigrationsStatusAsync()
        {
            _logger.LogInformation("Booting Projects Migrations Status...");

            ProjectsMigrationsModel? projects = await _accountsRepository.GetProjectMigrationsAsync();

            if (projects is null)
            {
                _logger.LogError("No projects migration status found.");
                return new ProjectsMigrationsModel();
            }

            string jsonResult = JsonSerializer.Serialize<ProjectsMigrationsModel>(projects);
            _logger.LogInformation("Projects found: {JsonResult}", jsonResult);

            return projects;
        }

        public async Task RunVideoMigrationStatusAsync()
        {
            _logger.LogInformation("Booting Video Migration Status...");
            // ここにビデオの初期化処理を追加
            // 例: ビデオ情報を取得して表示する
            VideoMigrationModel? video = await _accountsRepository.GetVideoMigrationAsync("location", "accountId", "videoId");
            if (video is null)
            {
                _logger.LogError("No video migration status found.");
                return;
            }
            string jsonResult = JsonSerializer.Serialize<VideoMigrationModel>(video);
            _logger.LogInformation("Video found: {JsonResult}", jsonResult);
        }

    }
}
