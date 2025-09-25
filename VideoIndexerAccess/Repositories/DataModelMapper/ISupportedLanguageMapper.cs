using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISupportedLanguageMapper
{
    SupportedLanguageModel MapFrom(ApiSupportedLanguageModel model);
    ApiSupportedLanguageModel MapToApiSupportedLanguageModel(SupportedLanguageModel model);
}