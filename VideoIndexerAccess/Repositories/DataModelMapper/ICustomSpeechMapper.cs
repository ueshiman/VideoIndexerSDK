using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ICustomSpeechMapper
{
    CustomSpeechModel MapFrom(ApiCustomSpeechModel apiModel);
    ApiCustomSpeechModel MapToApiCustomSpeechModel(CustomSpeechModel model);
}