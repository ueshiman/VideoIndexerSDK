using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IErrorResponseMapper
{
    ErrorResponseModel? MapFrom(ApiErrorResponseModel? model);
    ApiErrorResponseModel? MapToApiErrorResponseModel(ErrorResponseModel? model);
}