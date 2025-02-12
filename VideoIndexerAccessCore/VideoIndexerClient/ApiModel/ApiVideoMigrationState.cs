namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ビデオ移行の状態を表す列挙型。
    /// </summary>
    public enum ApiVideoMigrationState
    {
        NotStarted = 0,
        Pending = 1,
        InProgress = 2,
        Success = 3,
        Failed = 4,
        NotApplicable = 5,
        PendingUserAction = 6
    }
}
