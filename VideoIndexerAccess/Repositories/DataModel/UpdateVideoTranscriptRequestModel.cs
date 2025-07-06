namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UpdateVideoTranscriptRequestModel
    {
        public string VideoId { get; set; }
        public string VttContent { get; set; }
        public string? Language { get; set; }
        public bool? SetAsSourceLanguage { get; set; }
        public string? CallbackUrl { get; set; }
        public bool? SendSuccessEmail { get; set; }
    }
}
