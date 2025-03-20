namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// APIから取得したスピーチデータセットを表します。
    /// </summary>
    public class ApiSpeechDatasetUpdateModel
    {
        public string id { get; set; } = string.Empty;

        public string displayName { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;
    }
}
