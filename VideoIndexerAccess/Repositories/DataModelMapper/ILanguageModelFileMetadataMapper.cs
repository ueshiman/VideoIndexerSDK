using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILanguageModelFileMetadataMapper
{
    LanguageModelFileMetadataModel MapFrom(ApiLanguageModelFileMetadataModel model);
    ApiLanguageModelFileMetadataModel MapToApiLanguageModelFileMetadataModel(LanguageModelFileMetadataModel model);
}