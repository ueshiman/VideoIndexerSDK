using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISentiment1Mapper
{
    Sentiment1 MapFrom(Sentiment1ApiModel model);
}