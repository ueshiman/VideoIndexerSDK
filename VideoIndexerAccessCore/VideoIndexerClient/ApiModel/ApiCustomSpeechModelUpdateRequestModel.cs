namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチモデルの更新情報を格納するクラス。
    /// </summary>
    public class ApiCustomSpeechModelUpdateRequestModel
    {
        public string? displayName { get; set; }

        public string? description { get; set; }

        public Dictionary<string, string>? customProperties { get; set; }
    }
}
