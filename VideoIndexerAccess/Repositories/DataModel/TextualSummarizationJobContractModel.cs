namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// ビデオ要約のレスポンスモデル。
    /// </summary>
    public class TextualSummarizationJobContractModel
    {
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
        public string? DeploymentName { get; set; }
        public string? Disclaimer { get; set; }
    }
}
