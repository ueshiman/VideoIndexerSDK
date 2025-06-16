using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectMapper
{
    ProjectModel MapFrom(ApiProjectModel apiProjectModel);
    ApiProjectModel MapToApiProjectModel(ProjectModel projectModel);
}