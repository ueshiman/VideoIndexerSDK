using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialAccountQuotaUsageMapper : ITrialAccountQuotaUsageMapper
    {
        public TrialAccountQuotaUsageModel MapFrom(ApiTrialAccountQuotaUsageModel model)
        {
            return new TrialAccountQuotaUsageModel
            {
                DailyUploadCount = model.dailyUploadCount,
                DailyUploadCountLimit = model.dailyUploadCountLimit,
                DailyUploadDurationInSeconds = model.dailyUploadDurationInSeconds,
                DailyUploadDurationLimitInSeconds = model.dailyUploadDurationLimitInSeconds,
                EverUploadDurationInSeconds = model.everUploadDurationInSeconds,
                EverUploadDurationLimitInSeconds = model.everUploadDurationLimitInSeconds,
            };
        }

        public ApiTrialAccountQuotaUsageModel MapToApiTrialAccountQuotaUsageModel(TrialAccountQuotaUsageModel model)
        {
            return new ApiTrialAccountQuotaUsageModel
            {
                dailyUploadCount = model.DailyUploadCount,
                dailyUploadCountLimit = model.DailyUploadCountLimit,
                dailyUploadDurationInSeconds = model.DailyUploadDurationInSeconds,
                dailyUploadDurationLimitInSeconds = model.DailyUploadDurationLimitInSeconds,
                everUploadDurationInSeconds = model.EverUploadDurationInSeconds,
                everUploadDurationLimitInSeconds = model.EverUploadDurationLimitInSeconds,
            };
        }
    }
}
