using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupResponseMapper
{
    ApiLogoLogoGroupContractModel MapFrom(ApiLogoGroupResponseModel model);
    ApiLogoGroupResponseModel MapToApiLogoGroupResponseModel(ApiLogoLogoGroupContractModel model);
}