namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TrialAccountWithTokenModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string AccountType { get; set; }
        public string Url { get; set; }
        public string AccessToken { get; set; }
        public bool IsInMoveToArm { get; set; }
        public bool IsArmOnly { get; set; }
        public DateTime MoveToArmStartedDate { get; set; }
    }
}
