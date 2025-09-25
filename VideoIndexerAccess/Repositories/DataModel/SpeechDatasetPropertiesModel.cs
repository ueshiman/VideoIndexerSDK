using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// スピーチデータセットのプロパティ情報
    /// </summary>
    public class SpeechDatasetPropertiesModel
    {
        public int AcceptedLineCount { get; set; } // 承認された行数
        public int RejectedLineCount { get; set; } // 拒否された行数
        public string? Duration { get; set; } // データセットの総時間
        public string? Error { get; set; } // エラーメッセージ (存在する場合)
    }
}
