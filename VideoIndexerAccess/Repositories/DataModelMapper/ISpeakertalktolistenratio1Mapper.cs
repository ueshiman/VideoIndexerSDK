using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISpeakertalktolistenratio1Mapper
{
    Speakertalktolistenratio1 MapFrom(Speakertalktolistenratio1ApiModel model);
}