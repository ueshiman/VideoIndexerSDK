namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiSpeechDatasetFileModel
    {
        public string datasetId { get; set; }
        public string fileId { get; set; }
        public string name { get; set; }
        public string contentUrl { get; set; }
        public int kind { get; set; }
        public string createdDateTime { get; set; }
        public ApiFilePropertiesModel propertiesModel { get; set; }
    }
}
