using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ビデオ要約のレスポンスモデル。
    /// </summary>
    public class ApiVideoSummaryModel
    {
        public string? id { get; set; }
        public string? accountId { get; set; }
        public string? videoId { get; set; }
        public int state { get; set; }
        public string? modelName { get; set; }
        public int summaryStyle { get; set; }
        public int summaryLength { get; set; }
        public int includedFrames { get; set; }
        public string? createTime { get; set; }
        public string? lastUpdateTime { get; set; }
        public string? failureMessage { get; set; }
        public int progress { get; set; }
        public string? deploymentName { get; set; }
        public string? disclaimer { get; set; }
    }
}
