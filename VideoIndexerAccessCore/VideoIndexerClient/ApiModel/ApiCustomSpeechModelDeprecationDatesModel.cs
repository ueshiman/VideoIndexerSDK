using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiCustomSpeechModelDeprecationDatesModel
    {
        /// <summary>
        /// 適応モデルの廃止予定日
        /// </summary>
        public DateTime? adaptationDateTime { get; set; }

        /// <summary>
        /// 音声文字起こしの廃止予定日
        /// </summary>
        public DateTime? transcriptionDateTime { get; set; }
    }
}
