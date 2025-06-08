using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IBrandModelMapper
{
    ApiBrandModel MapToApiBrandModel(BrandModel model);
    BrandModel MapFrom(ApiBrandModel model);

}