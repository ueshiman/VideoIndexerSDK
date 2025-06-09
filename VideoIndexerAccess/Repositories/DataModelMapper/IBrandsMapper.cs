using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IBrandsMapper
{
    BrandModel MapFrom(ApiBrandModel model);
    ApiBrandModel MapToApiBrandModel(BrandModel model);

}