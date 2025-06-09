using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoRequestMapper
{
    LogoRequestModel MapFrom(ApiLogoRequestModel apiModel);
    ApiLogoRequestModel MapToApiLogoRequestModel(LogoRequestModel model);
}