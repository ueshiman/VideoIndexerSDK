using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectsMigrationsMapper
{
    ProjectsMigrationsModel? MapFrom(ApiProjectsMigrations? dataModel);
}