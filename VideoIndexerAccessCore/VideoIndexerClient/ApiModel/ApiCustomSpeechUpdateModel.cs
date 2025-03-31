namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// APIから取得したスピーチモデルを表します。
    /// </summary>
    public class ApiCustomSpeechUpdateModel
    {
        public string id { get; set; } = string.Empty;

        public string displayName { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;
    }
}
