using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoSearchMatchMapper
{
    VideoSearchMatchModel MapFrom(VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiVideoSearchMatchModel model);
    ApiVideoSearchMatchModel MapToApiVideoSearchMatchModel(VideoSearchMatchModel model);
}