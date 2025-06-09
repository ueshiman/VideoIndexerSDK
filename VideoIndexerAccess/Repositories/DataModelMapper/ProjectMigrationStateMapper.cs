using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectMigrationStateMapper : IProjectMigrationStateMapper
    {
        public ProjectMigrationState MapFrom(ApiProjectMigrationState state)
        {
            return state switch
            {
                ApiProjectMigrationState.NotStarted => ProjectMigrationState.NotStarted,
                ApiProjectMigrationState.Pending => ProjectMigrationState.Pending,
                ApiProjectMigrationState.InProgress => ProjectMigrationState.InProgress,
                ApiProjectMigrationState.Success => ProjectMigrationState.Success,
                ApiProjectMigrationState.Failed => ProjectMigrationState.Failed,
                ApiProjectMigrationState.NotApplicable => ProjectMigrationState.NotApplicable,
                ApiProjectMigrationState.PendingUserAction => ProjectMigrationState.PendingUserAction,
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}
