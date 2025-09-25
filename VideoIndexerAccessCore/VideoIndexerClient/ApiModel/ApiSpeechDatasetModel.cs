namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiSpeechDatasetModel
    {
        public Guid id { get; set; }
        public ApiSpeechDatasetPropertiesModel properties { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string locale { get; set; }
        public int kind { get; set; }
        public int status { get; set; }
        public DateTimeOffset lastActionDateTime { get; set; }
        public DateTimeOffset createdDateTime { get; set; }
        public Dictionary<string, string> customProperties { get; set; }
    }
}
