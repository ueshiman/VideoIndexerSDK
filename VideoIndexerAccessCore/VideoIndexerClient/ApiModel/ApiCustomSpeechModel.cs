using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiCustomSpeechModel
    {
        /// <summary>
        /// スピーチモデルの ID（GUID）
        /// </summary>
        public string id { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルのプロパティ
        /// </summary>
        public ApiCustomSpeechModelPropertiesModel? properties { get; set; }

        /// <summary>
        /// スピーチモデルの表示名
        /// </summary>
        public string displayName { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルの説明
        /// </summary>
        public string description { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルのロケール（言語）
        /// </summary>
        public string locale { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルに関連付けられたデータセット ID のリスト
        /// </summary>
        public List<string> datasets { get; set; } = new();

        /// <summary>
        /// スピーチモデルのステータス
        /// </summary>
        public ApiSpeechObjectState status { get; set; }

        /// <summary>
        /// 最後にアクションが行われた日時
        /// </summary>
        public DateTime? lastActionDateTime { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime? createdDateTime { get; set; }

        /// <summary>
        /// カスタムプロパティ（任意）
        /// </summary>
        public Dictionary<string, string>? customProperties { get; set; }
    }
}
