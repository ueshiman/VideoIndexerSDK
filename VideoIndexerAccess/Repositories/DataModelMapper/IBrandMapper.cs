using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IBrandMapper
{
    Brand MapFrom(BrandApiModel model);
    BrandApiModel MapToBrandApiModel(Brand model);

}