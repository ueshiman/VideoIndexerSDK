namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiFaceFilterModel
    {
        public List<int> ids { get; set; } = new List<int>();
        public string scope { get; set; } = "Exclude";
    }
}
