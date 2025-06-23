using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectRenderResponseMapper
{
    ProjectRenderResponseModel MapFrom(ApiProjectRenderResponseModel model);
    ApiProjectRenderResponseModel MapToApiProjectRenderResponseModel(ProjectRenderResponseModel model);
}