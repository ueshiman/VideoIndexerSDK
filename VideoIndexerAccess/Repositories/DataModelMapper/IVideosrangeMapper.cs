using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideosRangeMapper
{
    Videosrange MapFrom(VideosrangeApiModel model);
}