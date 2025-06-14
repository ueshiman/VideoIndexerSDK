using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITimeRangeMapper
{
    TimeRangeModel MapFrom(ApiTimeRangeModel model);
    ApiTimeRangeModel MapToApiTimeRangeModel(TimeRangeModel model);
}