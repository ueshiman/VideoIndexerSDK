namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetVideoCaptionsRequestModel
    {
        public required string VideoId { get; set; }
        public string? IndexId { get; set; } = null;
        public string? Format { get; set; } = null;
        public string? Language { get; set; } = null;
        public bool? IncludeAudioEffects { get; set; } = null;
        public bool? IncludeSpeakers { get; set; } = null;
    }
}
