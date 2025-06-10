using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupContractMapper
{
    LogoGroupContractModel MapFrom(ApiLogoGroupContractModel model);
    ApiLogoGroupContractModel MapToApiLogoGroupContractModel(LogoGroupContractModel model);
}