using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ICustomLanguageRequestMapper
{
    CustomLanguageRequestModel MapFrom(ApiCustomLanguageRequestModel model);
    ApiCustomLanguageRequestModel MapToCustomLanguageRequestModel(CustomLanguageRequestModel model);
}