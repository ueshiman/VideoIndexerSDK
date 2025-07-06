namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoSearchResultItemModel
    {
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Created { get; set; }
        public string? LastModified { get; set; }
        public string? LastIndexed { get; set; }
        public string? PrivacyMode { get; set; }
        public string? UserName { get; set; }
        public bool IsOwned { get; set; }
        public bool IsBase { get; set; }
        public int DurationInSeconds { get; set; }
        public string? State { get; set; }
        public string? ThumbnailVideoId { get; set; }
        public string? ThumbnailId { get; set; }
        public string? IndexingPreset { get; set; }
        public string? StreamingPreset { get; set; }
        public string? SourceLanguage { get; set; }
    }
}
