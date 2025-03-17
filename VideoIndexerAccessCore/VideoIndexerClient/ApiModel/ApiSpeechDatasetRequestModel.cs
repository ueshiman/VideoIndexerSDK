namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチデータセットの作成リクエストを表します。
    /// </summary>
    public class ApiSpeechDatasetRequestModel
    {
        public string displayName { get; set; } = string.Empty; // データセットの表示名
        public string locale { get; set; } = string.Empty; // 言語ロケール
        public string kind { get; set; } = string.Empty; // データセットの種類 (例: 発音, 言語)
        public string description { get; set; } = string.Empty; // データセットの説明
        public string contentUrl { get; set; } = string.Empty; // データセットのコンテンツ URL
        public Dictionary<string, string> customProperties { get; set; } = new Dictionary<string, string>(); // カスタムプロパティ
    }
}
