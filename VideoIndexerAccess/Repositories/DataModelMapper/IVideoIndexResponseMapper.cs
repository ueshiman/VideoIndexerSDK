using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoIndexResponseMapper
{
    VideoIndexResponseModel MapFrom(ApiVideoIndexResponseModel apiResponse);
    ApiVideoIndexResponseModel MapToApiVideoIndexResponseModel(VideoIndexResponseModel responseModel);
}