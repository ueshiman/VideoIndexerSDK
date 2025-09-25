using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeechDatasetPropertiesMapper
{
    SpeechDatasetPropertiesModel MapFrom(ApiSpeechDatasetPropertiesModel apiModel);
    ApiSpeechDatasetPropertiesModel MapToApiSpeechDatasetPropertiesModel(SpeechDatasetPropertiesModel domainModel);
}