using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class BrandModelSettingsMapper : IBrandModelSettingsMapper
    {
        public BrandModelSettingsModel MapFrom(ApiBrandModelSettingsModel model)
        {
            return new BrandModelSettingsModel
            {
                state = model.state,
                useBuiltIn = model.useBuiltIn
            };
        }

        public ApiBrandModelSettingsModel MapToApiBrandModelSettingsModel(BrandModelSettingsModel model)
        {
            return new ApiBrandModelSettingsModel
            {
                state = model.state,
                useBuiltIn = model.useBuiltIn
            };
        }
    }
}
