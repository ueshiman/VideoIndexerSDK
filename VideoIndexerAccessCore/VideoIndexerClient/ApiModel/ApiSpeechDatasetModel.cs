namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiSpeechDatasetModel
    {
        public string id { get; set; }
        public ApiSpeechDatasetPropertiesModel propertiesModel { get; set; }
        public string displayName { get; set; }
        public string description { get; set; }
        public string locale { get; set; }
        public int kind { get; set; }
        public int status { get; set; }
        public string lastActionDateTime { get; set; }
        public string createdDateTime { get; set; }
        public Dictionary<string, string> customProperties { get; set; }
    }
}
