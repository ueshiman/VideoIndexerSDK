using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectSearchResultMapper
{
    ProjectSearchResultModel MapFrom(ApiProjectSearchResultModel model);
    ApiProjectSearchResultModel MapToApiProjectSearchResultModel(ProjectSearchResultModel model);
}