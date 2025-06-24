namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectSearchResultModel
    {
        public List<ApiProjectSearchResultItemModel>? results { get; set; }
        public ApiPagingInfoModel? nextPage { get; set; }
    }
}
