using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IStreamingUrlMapper
{
    StreamingUrlModel MapFrom(ApiStreamingUrlModel model);
    ApiStreamingUrlModel MapTo(StreamingUrlModel model);
}