namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TrialAccountQuotaUsageModel
    {
        public long DailyUploadCount { get; set; }
        public long DailyUploadCountLimit { get; set; }
        public long DailyUploadDurationInSeconds { get; set; }
        public long DailyUploadDurationLimitInSeconds { get; set; }
        public long EverUploadDurationInSeconds { get; set; }
        public long EverUploadDurationLimitInSeconds { get; set; }
    }
}
