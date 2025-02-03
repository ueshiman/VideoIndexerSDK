using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakerTalkToListenRatioMapper
{
    Speakertalktolistenratio? MapFrom(SpeakertalktolistenratioApiModel? model);
}