using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoDetailsMapper
{
    VideoDetailsModel MapFrom(ApiVideoDetailsModel apiVideoDetailsModel);
    ApiVideoDetailsModel MapToApiVideoDetailsModel(VideoDetailsModel videoDetailsModel);
}