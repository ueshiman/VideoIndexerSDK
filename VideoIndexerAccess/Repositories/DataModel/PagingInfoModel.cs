namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PagingInfoModel
    {
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public bool Done { get; set; }
        public int? TotalCount { get; set; }
    }
}
