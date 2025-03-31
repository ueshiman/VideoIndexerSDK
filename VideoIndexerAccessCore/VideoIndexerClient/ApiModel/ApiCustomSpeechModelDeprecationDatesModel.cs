namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// カスタムスピーチモデルの非推奨日付を含みます。
    /// </summary>
    public class ApiCustomSpeechModelDeprecationDatesModel
    {
        /// <summary>
        /// 適応モデルの廃止予定日
        /// </summary>
        public DateTime? adaptationDateTime { get; set; }

        /// <summary>
        /// 音声文字起こしの廃止予定日
        /// </summary>
        public DateTime? transcriptionDateTime { get; set; }
    }
}
