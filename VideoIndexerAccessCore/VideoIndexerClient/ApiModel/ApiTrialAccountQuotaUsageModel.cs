namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    // Accountクラス（全プロパティを定義）
    public class ApiTrialAccountQuotaUsageModel
    {
        public long dailyUploadCount { get; set; }
        public long dailyUploadCountLimit { get; set; }
        public long dailyUploadDurationInSeconds { get; set; }
        public long dailyUploadDurationLimitInSeconds { get; set; }
        public long everUploadDurationInSeconds { get; set; }
        public long everUploadDurationLimitInSeconds { get; set; }
    }
}
