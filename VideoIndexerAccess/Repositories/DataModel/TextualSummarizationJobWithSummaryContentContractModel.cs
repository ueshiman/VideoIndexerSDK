using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TextualSummarizationJobWithSummaryContentContractModel
    {
        public string? DeploymentName { get; set; }

        public string? AddToEndOfSummaryInstructions { get; set; }

        public string? Disclaimer { get; set; }

        public Guid Id { get; set; }

        public Guid AccountId { get; set; }

        public string? VideoId { get; set; }

        public int State { get; set; }

        public string? ModelName { get; set; }

        public int SummaryStyle { get; set; }

        public int SummaryLength { get; set; }

        public int IncludedFrames { get; set; }

        public DateTimeOffset CreateTime { get; set; }

        public DateTimeOffset LastUpdateTime { get; set; }

        public string? FailureMessage { get; set; }

        public int? Progress { get; set; }

        public string? Summary { get; set; }

        public double SensitiveContentPercent { get; set; }
    }
}
