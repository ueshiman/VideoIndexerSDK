using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectSearchResultItemMapper
{
    ProjectSearchResultItemModel MapFrom(ApiProjectSearchResultItemModel model);
    ApiProjectSearchResultItemModel MapToApiProjectSearchResultItemModel(ProjectSearchResultItemModel model);
}