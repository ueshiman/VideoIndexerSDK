using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupResponseMapper
{
    LogoGroupResponseModel MapFrom(ApiLogoGroupResponseModel model);
    ApiLogoGroupResponseModel MapToApiLogoGroupResponseModel(LogoGroupResponseModel model);
}