using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TimeRangeMapper : ITimeRangeMapper
    {
        public TimeRangeModel MapFrom(ApiTimeRangeModel model)
        {
            return new TimeRangeModel
            {
                Start = model.start,
                End = model.end,
            };
        }

        public ApiTimeRangeModel MapToApiTimeRangeModel(TimeRangeModel model)
        {
            return new ApiTimeRangeModel
            {
                start = model.Start,
                end = model.End,
            };
        }
    }
}
