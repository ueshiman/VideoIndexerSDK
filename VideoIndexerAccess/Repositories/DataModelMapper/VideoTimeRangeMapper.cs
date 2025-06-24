using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoTimeRangeMapper : IVideoTimeRangeMapper
    {
        private readonly ITimeRangeMapper _timeRangeMapper;

        public VideoTimeRangeMapper(ITimeRangeMapper timeRangeMapper)
        {
            _timeRangeMapper = timeRangeMapper;
        }

        public VideoTimeRangeModel MapFrom(ApiVideoTimeRangeModel model)
        {
            return new VideoTimeRangeModel
            {
                VideoId = model.videoId,
                Range = _timeRangeMapper.MapFrom(model.range),
            };
        }

        public ApiVideoTimeRangeModel MapToApiVideoTimeRangeModel(VideoTimeRangeModel model)
        {
            return new ApiVideoTimeRangeModel
            {
                videoId = model.VideoId,
                range = _timeRangeMapper.MapToApiTimeRangeModel(model.Range),
            };
        }
    }
}
