using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IStatistics1Mapper
{
    Statistics1 MapFrom(Statistics1ApiModel model);
}