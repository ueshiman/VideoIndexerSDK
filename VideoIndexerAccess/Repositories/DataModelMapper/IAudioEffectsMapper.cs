using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IAudioEffectsMapper
{
    AudioEffects MapFrom(AudioEffectsApiModel model);
}