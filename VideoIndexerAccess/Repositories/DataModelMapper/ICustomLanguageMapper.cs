using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ICustomLanguageMapper
{
    CustomLanguageModel MapFrom(ApiCustomLanguageModel model);
    ApiCustomLanguageModel MapToApiCustomLanguageModel(CustomLanguageModel model);
}