using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialLimitedAccessFeaturesMapper
{
    TrialLimitedAccessFeaturesModel MapFrom(ApiTrialLimitedAccessFeaturesModel model);
    ApiTrialLimitedAccessFeaturesModel MapToApiTrialLimitedAccessFeaturesModel(TrialLimitedAccessFeaturesModel model);
}