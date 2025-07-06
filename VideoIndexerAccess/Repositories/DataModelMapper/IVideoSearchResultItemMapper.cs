using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoSearchResultItemMapper
{
    VideoSearchResultItemModel MapFrom(ApiVideoSearchResultItemModel model);
    ApiVideoSearchResultItemModel MapToApiVideoSearchResultItemModel(VideoSearchResultItemModel model);
}