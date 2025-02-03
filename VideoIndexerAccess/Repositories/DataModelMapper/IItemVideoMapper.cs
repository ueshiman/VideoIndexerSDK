using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IItemVideoMapper
{
    ItemVideo MapFrom(ItemVideoApiModel model);
}