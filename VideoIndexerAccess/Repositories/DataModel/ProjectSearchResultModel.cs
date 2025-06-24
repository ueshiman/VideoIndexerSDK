using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectSearchResultModel
    {
        public List<ProjectSearchResultItemModel>? Results { get; set; }
        public PagingInfoModel? NextPage { get; set; }
    }
}
