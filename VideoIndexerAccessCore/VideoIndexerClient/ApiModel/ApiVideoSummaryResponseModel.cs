using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    // Video summary JSON に対応するデータモデル
    public class ApiVideoSummaryResponseModel
    {
        public string? summary { get; set; }
        public string? disclaimer { get; set; }
        public float? sensitiveContentPercent { get; set; }
        public string? deploymentName { get; set; }
        public Guid? id { get; set; }
        public Guid? accountId { get; set; }
        public string? videoId { get; set; }
        public string? state { get; set; }
        public string? modelName { get; set; }
        public string? summaryStyle { get; set; }
        public string? summaryLength { get; set; }
        public string? includedFrames { get; set; }
        public DateTime? createTime { get; set; }
        public DateTime? lastUpdateTime { get; set; }
        public string? failureMessage { get; set; }
        public int? progress { get; set; }
    }
}
