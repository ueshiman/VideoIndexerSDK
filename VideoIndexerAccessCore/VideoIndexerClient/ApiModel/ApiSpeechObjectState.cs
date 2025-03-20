namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{

    /// <summary>
    /// スピーチオブジェクトの状態を表す列挙型
    /// </summary>
    public enum ApiSpeechObjectState
    {
        None = 0,
        Waiting = 1,
        Processing = 2,
        Complete = 3,
        Failed = 4
    }
}
