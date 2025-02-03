namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{

    public class ApiVideoListJsonModel
    {
        public ApiVideoListItemModel[]? results { get; set; }
        public Nextpage? nextPage { get; set; }
    }

    public class Nextpage
    {
        public int? pageSize { get; set; }
        public int? skip { get; set; }
        public bool? done { get; set; }
    }
}
