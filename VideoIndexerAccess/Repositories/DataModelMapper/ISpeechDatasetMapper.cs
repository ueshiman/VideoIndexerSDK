using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeechDatasetMapper
{
    SpeechDatasetModel MapFrom(ApiSpeechDatasetModel apiModel);
    ApiSpeechDatasetModel MapToApiSpeechDatasetModel(SpeechDatasetModel model);
}