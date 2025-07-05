namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetVideoStreamingUrlRequestModel
    {
        public required string VideoId { get; set; }
        public bool? UseProxy { get; set; }
        public string? UrlFormat { get; set; }
        public int? TokenLifetimeInMinutes { get; set; }
    }
}
