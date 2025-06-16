namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// , string projectId, string? indexId = null, string? format = null, string? language = null, bool? includeAudioEffects = null, bool? includeSpeakers = null
    /// </summary>
    public class GetProjectCaptionsRequestModel
    {
        public string ProjectId { get; set; }
        public string? IndexId { get; set; }
        public string? Format { get; set; }
        public string? Language { get; set; }
        public bool? IncludeAudioEffects { get; set; }
        public bool? IncludeSpeakers { get; set; }
    }
}
