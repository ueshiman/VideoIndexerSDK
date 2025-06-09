using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILanguageModelDataMapper
{
    LanguageModelDataModel MapFrom(ApiLanguageModelDataModel model);
    ApiLanguageModelDataModel MapToApiLanguageModelDataModel(LanguageModelDataModel model);
}