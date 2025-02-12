namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public partial class ApiPagingInfoModel
{
    public int pageSize { get; set; }
    public int skip { get; set; }
    public bool done { get; set; }
    public int? totalCount { get; set; }

}