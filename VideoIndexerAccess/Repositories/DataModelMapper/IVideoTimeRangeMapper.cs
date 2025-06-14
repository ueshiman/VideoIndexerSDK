using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoTimeRangeMapper
{
    VideoTimeRangeModel MapFrom(ApiVideoTimeRangeModel model);
    ApiVideoTimeRangeModel MapToApiVideoTimeRangeModel(VideoTimeRangeModel model);
}