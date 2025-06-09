using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class CustomLanguageRequestMapper : ICustomLanguageRequestMapper
    {
        public CustomLanguageRequestModel MapFrom(ApiCustomLanguageRequestModel model)
        {
            return new CustomLanguageRequestModel
            {
                ModelName = model.ModelName,
                Language = model.Language,
            };
        }
        public ApiCustomLanguageRequestModel MapToCustomLanguageRequestModel(CustomLanguageRequestModel model)
        {
            return new ApiCustomLanguageRequestModel
            {
                ModelName = model.ModelName,
                Language = model.Language,
            };
        }
    }
}
