using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class AccountMigrationStatusMapper : IAccountMigrationStatusMapper
    {
        public AccountMigrationStatusModel? MapFrom(ApiAccountMigrationStatusModel? model)
        {
            return model is null ? null : new AccountMigrationStatusModel()
            {
                Status = model.status,
                Progress = model.progress,
                VideosLeftToMigrate = model.videosLeftToMigrate,
                VideosMigrated = model.videosMigrated,
                VideosFailedToMigrate = model.videosFailedToMigrate,
                TotalVideosToMigrate = model.totalVideosToMigrate,
                ProjectsLeftToMigrate = model.projectsLeftToMigrate,
                ProjectsMigrated = model.projectsMigrated,
                ProjectsFailedToMigrate = model.projectsFailedToMigrate,
                TotalProjectsToMigrate = model.totalProjectsToMigrate,
                Details = model.details,
                MigrationCompletedDate = model.migrationCompletedDate,
            };
        }
    }
}
