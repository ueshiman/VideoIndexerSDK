using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// スピーチデータセットのプロパティ情報
    /// </summary>
    public class ApiSpeechDatasetPropertiesModel
    {
        public int acceptedLineCount { get; set; } // 承認された行数
        public int rejectedLineCount { get; set; } // 拒否された行数
        public string? duration { get; set; } // データセットの総時間
        public string? error { get; set; } // エラーメッセージ (存在する場合)
    }
}
