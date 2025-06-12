namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PatchValueModel
    {
        public double? Confidence { get; set; }
        public string? Language { get; set; }
        public string? Text { get; set; }
        public int? SpeakerId { get; set; }
        public PatchInstanceModel? Instances { get; set; }
    }
}
