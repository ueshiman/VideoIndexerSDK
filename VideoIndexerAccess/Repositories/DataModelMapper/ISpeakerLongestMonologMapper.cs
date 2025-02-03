using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakerLongestMonologMapper
{
    Speakerlongestmonolog? MapFrom(SpeakerlongestmonologApiModel? model);
}