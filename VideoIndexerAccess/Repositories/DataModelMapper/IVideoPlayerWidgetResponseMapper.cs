using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoPlayerWidgetResponseMapper
{
    VideoPlayerWidgetResponseModel MapFrom(ApiVideoPlayerWidgetResponseModel model);
    ApiVideoPlayerWidgetResponseModel MapToApiVideoPlayerWidgetResponseModel(VideoPlayerWidgetResponseModel model);
}