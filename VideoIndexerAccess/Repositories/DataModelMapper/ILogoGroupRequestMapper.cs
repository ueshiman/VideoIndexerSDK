using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupRequestMapper
{
    LogoGroupRequestModel MapFrom(ApiLogoGroupRequestModel model);
    ApiLogoGroupRequestModel MapToApiLogoGroupRequestModel(LogoGroupRequestModel model);
}