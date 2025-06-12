namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoIndexResponseModel
    {
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Created { get; set; }
        public bool? IsOwned { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsBase { get; set; }
        public int? DurationInSeconds { get; set; }
        public string? Duration { get; set; }
        public List<VideoDetailsModel>? Videos { get; set; }
    }
}
