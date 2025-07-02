using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialAccountQuotaUsageMapper
{
    TrialAccountQuotaUsageModel MapFrom(ApiTrialAccountQuotaUsageModel model);
    ApiTrialAccountQuotaUsageModel MapToApiTrialAccountQuotaUsageModel(TrialAccountQuotaUsageModel model);
}