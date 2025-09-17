using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupUpdateRequestMapper
{
    LogoGroupUpdateRequestModel MapFrom(ApiLogoGroupUpdateRequestModel apiModel);
    ApiLogoGroupUpdateRequestModel MapToApiApiLogoGroupUpdateRequestModel(LogoGroupUpdateRequestModel domainModel);
}