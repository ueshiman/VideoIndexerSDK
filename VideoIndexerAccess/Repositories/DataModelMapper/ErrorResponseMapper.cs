using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ErrorResponseMapper : IErrorResponseMapper
    {
        public ErrorResponseModel MapFrom(ApiErrorResponseModel model)
        {
            return new ErrorResponseModel
            {
                ErrorType = model.errorType,
                Message = model.message,
            };
        }
        public ApiErrorResponseModel MapToApiErrorResponseModel(ErrorResponseModel model)
        {
            return new ApiErrorResponseModel
            {
                errorType = model.ErrorType,
                message = model.Message,
            };
        }
    }
}
