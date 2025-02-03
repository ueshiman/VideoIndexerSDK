using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakerNumberOfFragmentsMapper
{
    Speakernumberoffragments? MapFrom(SpeakernumberoffragmentsApiModel? model);
}