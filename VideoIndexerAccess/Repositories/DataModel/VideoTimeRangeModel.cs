namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoTimeRangeModel
    {
        public string VideoId { get; set; } = string.Empty;
        public TimeRangeModel Range { get; set; } = new();
    }
}
