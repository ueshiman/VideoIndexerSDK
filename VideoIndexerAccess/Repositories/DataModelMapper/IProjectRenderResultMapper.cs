using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectRenderResultMapper
{
    ProjectRenderResultModel? MapFrom(ApiProjectRenderResultModel? model);
    ApiProjectRenderResultModel? MapToApiProjectRenderResultModel(ProjectRenderResultModel model);
}