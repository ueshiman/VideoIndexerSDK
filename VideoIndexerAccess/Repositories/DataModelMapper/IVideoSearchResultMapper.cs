using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoSearchResultMapper
{
    VideoSearchResultModel MapFrom(ApiVideoSearchResultModel model);
    ApiVideoSearchResultModel MapToApiVideoSearchResultModel(VideoSearchResultModel model);
}