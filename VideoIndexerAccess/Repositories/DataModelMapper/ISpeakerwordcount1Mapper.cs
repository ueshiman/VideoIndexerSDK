using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakerwordcount1Mapper
{
    Speakerwordcount1 MapFrom(Speakerwordcount1ApiModel model);
}