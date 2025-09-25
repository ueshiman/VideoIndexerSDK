using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeechDatasetFileMapper
{
    SpeechDatasetFileModel MapFrom(VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiSpeechDatasetFileModel apiModel);
    ApiSpeechDatasetFileModel MapFromApiSpeechDatasetFileModel(SpeechDatasetFileModel model);
}