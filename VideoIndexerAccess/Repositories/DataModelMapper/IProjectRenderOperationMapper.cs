using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectRenderOperationMapper
{
    ProjectRenderOperationModel MapFrom(ApiProjectRenderOperationModel model);
    ApiProjectRenderOperationModel MapToApiProjectRenderOperationModel(ProjectRenderOperationModel model);
}