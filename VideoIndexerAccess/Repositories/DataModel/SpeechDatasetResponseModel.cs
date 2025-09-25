using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// スピーチデータセットの API レスポンスモデル
    /// </summary>
    public class SpeechDatasetResponseModel
    {
        public Guid? Id { get; set; } // データセットの ID
        public ApiSpeechDatasetPropertiesModel? Properties { get; set; } // データセットのプロパティ
        public string? DisplayName { get; set; } // データセットの表示名
        public string? Description { get; set; } // データセットの説明
        public string? Locale { get; set; } // 言語ロケール
        public string? Kind { get; set; } // データセットの種類
        public int Status { get; set; } // データセットのステータス (0:None, 1:Waiting, 2:Processing, 3:Complete, 4:Failed)
        public string? LastActionDateTime { get; set; } // 最終更新日時
        public string? CreatedDateTime { get; set; } // 作成日時
        public Dictionary<string, string>? CustomProperties { get; set; } // カスタムプロパティ
    }
}
