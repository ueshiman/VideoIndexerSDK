using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILanguageModelFileDataMapper
{
    LanguageModelFileDataModel MapFrom(ApiLanguageModelFileDataModel model);
    ApiLanguageModelFileDataModel MapToApiLanguageModelFileDataModel(LanguageModelFileDataModel model);
}