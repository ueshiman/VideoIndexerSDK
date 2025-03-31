namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiAccountSlimModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? location { get; set; }
        public string? accountType { get; set; }
        public string? url { get; set; }
        public string? accessToken { get; set; }
        public bool? isInMoveToArm { get; set; }
        public bool? isArmOnly { get; set; }
        public DateTime? moveToArmStartedDate { get; set; }
    }
}
