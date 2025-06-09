using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IBrandModelSettingsMapper
{
    BrandModelSettingsModel MapFrom(ApiBrandModelSettingsModel model);
    ApiBrandModelSettingsModel MapToApiBrandModelSettingsModel(BrandModelSettingsModel model);
}