using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IUploadVideoResponseMapper
{
    UploadVideoResponseModel MapFrom(ApiUploadVideoResponseModel model);
    ApiUploadVideoResponseModel MapToApiUploadVideoResponseModel(UploadVideoResponseModel model);
}