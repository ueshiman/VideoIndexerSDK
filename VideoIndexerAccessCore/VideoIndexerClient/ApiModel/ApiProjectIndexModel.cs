namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectIndexModel
    {
        public string accountId { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string? name { get; set; }
        public string? userName { get; set; }
        public DateTime created { get; set; }
        public bool isOwned { get; set; }
        public bool isEditable { get; set; }
        public bool isBase { get; set; }
        public int durationInSeconds { get; set; }
        public string? duration { get; set; }
        public object? summarizedInsights { get; set; }
        public List<ApiVideoIndexModel>? videos { get; set; }
        public List<ApiVideoTimeRangeModel>? videosRanges { get; set; }
        public ApiRenderedProjectInfoModel? renderedProjectInfo { get; set; }
    }
}
