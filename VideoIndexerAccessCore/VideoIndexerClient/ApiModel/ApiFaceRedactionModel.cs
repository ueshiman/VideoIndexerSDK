namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiFaceRedactionModel
    {
        public string blurringKind { get; set; } = "HighBlur";
        public ApiFaceFilterModel filter { get; set; } = new ApiFaceFilterModel();
    }
}
