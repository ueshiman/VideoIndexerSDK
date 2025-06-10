using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using ApiLogoContractModel = VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiLogoContractModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoResponseMapper
{
    LogoContractModel MapFrom(ApiLogoContractModel model);
    ApiLogoContractModel MapToApiLogoResponseModel(LogoContractModel model);
}