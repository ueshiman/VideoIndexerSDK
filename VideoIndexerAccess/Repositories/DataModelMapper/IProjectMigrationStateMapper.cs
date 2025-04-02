using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectMigrationStateMapper
{
    ProjectMigrationState MapFrom(ApiProjectMigrationState state);
}