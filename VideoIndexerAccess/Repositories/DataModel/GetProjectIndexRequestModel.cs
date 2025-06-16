namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetProjectIndexRequestModel
    {
        public string ProjectId { get; set; }
        public string? Language { get; set; }
        public bool? ReTranslate { get; set; }
        public string? IncludedInsights { get; set; }
        public string? ExcludedInsights { get; set; }
        public bool? IncludeSummarizedInsights { get; set; }
    }
}
