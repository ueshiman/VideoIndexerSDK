namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiFaceRedactionModel
    {
        public string blurringKind { get; set; } = "HighBlur";
        public ApiFaceFilterModel FilterModel { get; set; } = new ApiFaceFilterModel();
    }
}
