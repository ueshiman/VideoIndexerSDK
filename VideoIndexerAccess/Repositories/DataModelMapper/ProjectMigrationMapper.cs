using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectMigrationMapper : IProjectMigrationMapper
    {
        private readonly IProjectMigrationStateMapper _projectMigrationStateMapper;

        public ProjectMigrationMapper(IProjectMigrationStateMapper projectMigrationStateMapper)
        {
            _projectMigrationStateMapper = projectMigrationStateMapper;
        }

        public ProjectMigrationModel MapFrom(ApiProjectMigrationModel model)
        {
            return new ProjectMigrationModel()
            {
                Status = _projectMigrationStateMapper.MapFrom(model.status),
                Name = model.name,
                Details = model.details,
                ProjectId = model.projectId,
            };
        }
    }
}
