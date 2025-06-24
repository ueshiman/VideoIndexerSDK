using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PromptCreateResponseMapper : IPromptCreateResponseMapper
    {
        public PromptCreateResponseModel MapFrom(ApiPromptCreateResponseModel model)
        {
            return new PromptCreateResponseModel()
            {
                ErrorType = model.errorType,
                Message = model.message,
            };
        }

        public ApiPromptCreateResponseModel MapTo(PromptCreateResponseModel model)
        {
            return new ApiPromptCreateResponseModel()
            {
                errorType = model.ErrorType,
                message = model.Message,
            };
        }
    }
}
