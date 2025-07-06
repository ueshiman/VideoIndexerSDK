namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoSearchResultModel
    {
        public List<VideoSearchResultItemModel>? Results { get; set; }
        public PagingInfoModel? NextPage { get; set; }
    }
}
