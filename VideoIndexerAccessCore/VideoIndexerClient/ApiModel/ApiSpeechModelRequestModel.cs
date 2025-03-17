namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチモデルの作成リクエストを表します。
    /// </summary>
    public class ApiSpeechModelRequestModel
    {
        public string displayName { get; set; } = string.Empty; // モデルの表示名
        public string locale { get; set; } = string.Empty; // 言語ロケール
        public List<string> datasets { get; set; } = new List<string>(); // 関連データセットの ID リスト
        public string description { get; set; } = string.Empty; // モデルの説明
        public Dictionary<string, string> customProperties { get; set; } = new Dictionary<string, string>(); // カスタムプロパティ
    }
}
