using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class RedactVideoResponseMapper : IRedactVideoResponseMapper
    {
        public RedactVideoResponseModel MapFrom(ApiRedactVideoResponseModel apiResponse)
        {
            return new RedactVideoResponseModel
            {
                ErrorType = apiResponse.errorType,
                Message = apiResponse.message
            };
        }

        public ApiRedactVideoResponseModel MapToApiRedactVideoResponseModel(RedactVideoResponseModel responseModel)
        {
            return new ApiRedactVideoResponseModel
            {
                errorType = responseModel.ErrorType,
                message = responseModel.Message
            };
        }
    }
}
