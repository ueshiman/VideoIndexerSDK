using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoUpdateRequestMapper
{
    LogoUpdateRequestModel MapFrom(ApiLogoUpdateRequestModel apiModel);
    ApiLogoUpdateRequestModel MapToApiLogoUpdateRequestModel(LogoUpdateRequestModel domainModel);
}