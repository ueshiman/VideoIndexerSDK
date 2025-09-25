namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチデータセットの更新情報を格納するクラス。
    /// </summary>
    public class ApiUpdateSpeechDatasetModel
    {
        public string? displayName { get; set; }

        public string? description { get; set; }

        public Dictionary<string, string>? customProperties { get; set; }
    }
}
