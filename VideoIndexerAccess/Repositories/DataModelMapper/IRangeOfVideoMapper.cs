using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IRangeOfVideoMapper
{
    RangeOfVideo MapFrom(RangeApiModel? model);
}