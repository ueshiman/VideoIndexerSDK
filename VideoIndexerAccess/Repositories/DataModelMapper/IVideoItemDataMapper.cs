using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoItemDataMapper
{
    VideoItemDataModel MapFrom(VideoItemApiModel model);
}