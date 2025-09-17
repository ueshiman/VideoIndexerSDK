using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ILogoTextVariationMapper
{
    LogoTextVariationModel MapFrom(ApiLogoTextVariationModel model);
    ApiLogoTextVariationModel MapToApiLogoTextVariationModel(LogoTextVariationModel model);
}