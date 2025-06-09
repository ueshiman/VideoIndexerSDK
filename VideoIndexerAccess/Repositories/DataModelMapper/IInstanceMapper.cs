using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IInstanceMapper
{
    Instance MapFrom(InstanceApiModel model);
    InstanceApiModel MapToInstanceApiModel(Instance model);

}