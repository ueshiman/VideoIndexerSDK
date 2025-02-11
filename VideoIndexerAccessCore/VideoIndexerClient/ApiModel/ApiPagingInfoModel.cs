namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public partial class ApiPagingInfoModel
{
    public int PageSize { get; set; }
    public int Skip { get; set; }
    public bool Done { get; set; }
    public int TotalCount { get; set; }
    
}