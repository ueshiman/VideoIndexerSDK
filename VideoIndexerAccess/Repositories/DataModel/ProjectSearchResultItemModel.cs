namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectSearchResultItemModel
    {
        public string AccountId { get; set; } = "";
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Created { get; set; } = "";
        public string LastModified { get; set; } = "";
        public string UserName { get; set; } = "";
        public int DurationInSeconds { get; set; }
        public string ThumbnailVideoId { get; set; } = "";
        public string ThumbnailId { get; set; } = "";
        public List<VideoSearchMatchModel>? SearchMatches { get; set; }
    }
}
