using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoGroupLinkedLogosMapper
{
    LogoGroupLinkedLogosModel MapFrom(ApiLogoGroupLinkedLogosModel apiModel);
    ApiLogoGroupLinkedLogosModel MapToApiLogoGroupLinkedLogosModel(LogoGroupLinkedLogosModel model);
}