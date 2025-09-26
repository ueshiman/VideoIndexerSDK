using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{

    public class ApiAOAITextualSummarizationJobWithSummaryContentContractModel
    {
            public string? deploymentName { get; set; }

            public string? addToEndOfSummaryInstructions { get; set; }

            public string? disclaimer { get; set; }

            public Guid id { get; set; }

            public Guid accountId { get; set; }

            public string? videoId { get; set; }

            public int state { get; set; }

            public string? modelName { get; set; }

            public int summaryStyle { get; set; }

            public int summaryLength { get; set; }

            public int includedFrames { get; set; }

            public DateTimeOffset createTime { get; set; }

            public DateTimeOffset lastUpdateTime { get; set; }

            public string? failureMessage { get; set; }

            public int? progress { get; set; }

            public string? summary { get; set; }

            public double sensitiveContentPercent { get; set; }
    }
}
