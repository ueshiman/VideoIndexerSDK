namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// JSON Patch の値をカプセル化するクラス。
    /// </summary>
    public class AipPatchValueModel
    {
        public double? Confidence { get; set; }
        public string? Language { get; set; }
        public string? Text { get; set; }
        public int? SpeakerId { get; set; }
        public ApiPatchInstanceModel? Instances { get; set; }
    }
}
