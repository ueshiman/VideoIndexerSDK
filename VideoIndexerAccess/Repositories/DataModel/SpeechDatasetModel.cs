using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// スピーチデータセットのモデル
    /// </summary>
    public class SpeechDatasetModel
    {
        public Guid Id { get; set; } // データセットの ID
        public SpeechDatasetPropertiesModel? Properties { get; set; } // データセットのプロパティ
        public string? DisplayName { get; set; } // データセットの表示名
        public string? Description { get; set; } // データセットの説明
        public string? Locale { get; set; } // 言語ロケール
        public int Kind { get; set; } // データセットの種類
        public int Status { get; set; } // データセットのステータス (0:None, 1:Waiting, 2:Processing, 3:Complete, 4:Failed)
        public DateTimeOffset LastActionDateTime { get; set; } // 最終更新日時
        public DateTimeOffset CreatedDateTime { get; set; } // 作成日時
        public Dictionary<string, string>? CustomProperties { get; set; } // カスタムプロパティ
    }
}
