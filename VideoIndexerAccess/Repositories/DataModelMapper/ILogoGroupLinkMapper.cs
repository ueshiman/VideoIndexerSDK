using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupLinkMapper
{
    LogoGroupLinkModel MapFrom(ApiLogoGroupLinkModel model);
    ApiLogoGroupLinkModel MapToApiLogoGroupLinkModel(LogoGroupLinkModel model);
}