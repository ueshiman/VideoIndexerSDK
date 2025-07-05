namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TrialAccountModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AccountType { get; set; }
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public bool IsInMoveToArm { get; set; }
        public bool IsArmOnly { get; set; }
        public DateTime? MoveToArmStartedDate { get; set; }
        public string CName { get; set; }
        public string Description { get; set; }
        public DateTime? CreateTime { get; set; }
        public List<TrialAccountUserModel> Owners { get; set; }
        public List<TrialAccountUserModel> Contributors { get; set; }
        public List<string> InvitedContributors { get; set; }
        public List<TrialAccountUserModel> Readers { get; set; }
        public List<string> InvitedReaders { get; set; }
        public TrialAccountQuotaUsageModel QuotaUsage { get; set; }
        public TrialAccountStatisticsModel Statistics { get; set; }
        public TrialLimitedAccessFeaturesModel LimitedAccessFeatures { get; set; }
        public string State { get; set; }
        public bool IsPaid { get; set; }
    }
}
