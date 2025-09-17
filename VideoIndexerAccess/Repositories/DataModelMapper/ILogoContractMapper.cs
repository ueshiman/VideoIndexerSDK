using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public interface ILogoContractMapper
    {
        LogoContractModel MapFrom(ApiLogoContractModel model);
        ApiLogoContractModel MapToApiLogoContractModel(LogoContractModel model);
    }
}