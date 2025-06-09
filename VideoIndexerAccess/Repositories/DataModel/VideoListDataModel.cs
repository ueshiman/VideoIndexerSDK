namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoListDataModel
    {
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? Partition { get; set; }
        public object? ExternalId { get; set; }
        public object? Metadata { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? LastIndexed { get; set; }
        public string? PrivacyMode { get; set; }
        public string? UserName { get; set; }
        public bool? IsOwned { get; set; }
        public bool? IsBase { get; set; }
        public bool? HasSourceVideoFile { get; set; }
        public string? State { get; set; }
        public string? ModerationState { get; set; }
        public string? ReviewState { get; set; }
        public bool? IsSearchable { get; set; }
        public string? ProcessingProgress { get; set; }
        public int? DurationInSeconds { get; set; }
        public string? ThumbnailVideoId { get; set; }
        public string? ThumbnailId { get; set; }
        public object[]? SearchMatches { get; set; }
        public string? IndexingPreset { get; set; }
        public string? StreamingPreset { get; set; }
        public string? SourceLanguage { get; set; }
        public string[]? SourceLanguages { get; set; }
        public string? PersonModelId { get; set; }
        public string? ThumbnailUrl { get; set; }
    }
}
