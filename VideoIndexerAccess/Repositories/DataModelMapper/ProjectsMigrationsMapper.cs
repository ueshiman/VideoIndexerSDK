using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectsMigrationsMapper : IProjectsMigrationsMapper
    {
        private IProjectMigrationStateMapper _projectMigrationStateMapper;
        private readonly IProjectMigrationMapper _projectMigrationMapper;
        private readonly IPagingInfoMapper _pagingInfoMapper;

        public ProjectsMigrationsMapper(IProjectMigrationStateMapper projectMigrationStateMapper, IProjectMigrationMapper projectMigrationMapper, IPagingInfoMapper pagingInfoMapper)
        {
            _projectMigrationStateMapper = projectMigrationStateMapper;
            _projectMigrationMapper = projectMigrationMapper;
            _pagingInfoMapper = pagingInfoMapper;
        }

        public ProjectsMigrationsModel? MapFrom(ApiProjectsMigrations? dataModel)
        {
            return dataModel is null ? null : new ProjectsMigrationsModel
            {
                Results = dataModel.results?.Select(_projectMigrationMapper.MapFrom).ToList() ?? new List<ProjectMigrationModel>(),
                NextPage = _pagingInfoMapper.MapFrom(dataModel.nextPage),
            };
        }
    }
}
