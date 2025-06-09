using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILanguageModelEditMapper
{
    LanguageModelEditModel MapFrom(ApiLanguageModelEditModel model);
    ApiLanguageModelEditModel MapToApiLanguageModelEditModel(LanguageModelEditModel model);
}