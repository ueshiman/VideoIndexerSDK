namespace VideoIndexerAccess.Repositories.DataModel
{
    public class CreateProjectRequestModel
    {
        public string ProjectName { get; set; }
        public List<VideoTimeRangeModel> VideoRanges { get; set; }
    }
}
