namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>  
    /// 各動画の情報を表します  
    /// </summary>  
    public class ApiVideoSearchResultItemModel
    {
        public string? accountId { get; set; }
        public string? id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public string? created { get; set; }
        public string? lastModified { get; set; }
        public string? lastIndexed { get; set; }
        public string? privacyMode { get; set; }
        public string? userName { get; set; }
        public bool isOwned { get; set; }
        public bool isBase { get; set; }
        public int durationInSeconds { get; set; }
        public string? state { get; set; }
        public string? thumbnailVideoId { get; set; }
        public string? thumbnailId { get; set; }
        public string? indexingPreset { get; set; }
        public string? streamingPreset { get; set; }
        public string? sourceLanguage { get; set; }
    }

}
