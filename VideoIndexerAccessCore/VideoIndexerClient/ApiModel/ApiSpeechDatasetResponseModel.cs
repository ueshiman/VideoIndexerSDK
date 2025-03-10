using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチデータセットの API レスポンスモデル
    /// </summary>
    public class ApiSpeechDatasetResponseModel
    {
        public string? id { get; set; } // データセットの ID
        public ApiSpeechDatasetPropertiesModel? properties { get; set; } // データセットのプロパティ
        public string? displayName { get; set; } // データセットの表示名
        public string? description { get; set; } // データセットの説明
        public string? locale { get; set; } // 言語ロケール
        public string? kind { get; set; } // データセットの種類
        public int status { get; set; } // データセットのステータス (0:None, 1:Waiting, 2:Processing, 3:Complete, 4:Failed)
        public string? lastActionDateTime { get; set; } // 最終更新日時
        public string? createdDateTime { get; set; } // 作成日時
        public Dictionary<string, string>? customProperties { get; set; } // カスタムプロパティ
    }
}
