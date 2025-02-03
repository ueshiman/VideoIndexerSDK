using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakerWordCountMapper
{
    Speakerwordcount? MapFrom(SpeakerwordcountApiModel? model);
}