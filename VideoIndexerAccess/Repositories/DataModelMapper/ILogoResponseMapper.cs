using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoResponseMapper
{
    LogoResponseModel Map(ApiLogoResponseModel apiModel);
    ApiLogoResponseModel MapToApiLogoResponseModel(LogoResponseModel model);
}