namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiVideoTimeRangeModel
    {
        public string videoId { get; set; } = string.Empty;
        public ApiTimeRangeModel range { get; set; } = new();
    }
}
