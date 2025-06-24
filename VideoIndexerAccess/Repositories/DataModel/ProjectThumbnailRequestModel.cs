namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectThumbnailRequestModel
    {
        public required string ProjectId { get; set; }
        public required string ThumbnailId { get; set; }
        public string? Format { get; set; }
    }
}
