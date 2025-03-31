namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチモデルの API レスポンスモデル
    /// </summary>
    public class ApiSpeechModelResponseModel
    {
        public string? id { get; set; } // モデルの ID
        public ApiSpeechModelPropertiesModel? properties { get; set; } // モデルのプロパティ
        public string? displayName { get; set; } // モデルの表示名
        public string? description { get; set; } // モデルの説明
        public string? locale { get; set; } // 言語ロケール
        public List<string>? datasets { get; set; } // データセットの ID リスト
        public int status { get; set; } // モデルのステータス (0:None, 1:Waiting, 2:Processing, 3:Complete, 4:Failed)
        public string? lastActionDateTime { get; set; } // 最終更新日時
        public string? createdDateTime { get; set; } // 作成日時
        public Dictionary<string, string>? customProperties { get; set; } // カスタムプロパティ
    }
}
