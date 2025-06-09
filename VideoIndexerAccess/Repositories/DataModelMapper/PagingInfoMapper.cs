using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PagingInfoMapper : IPagingInfoMapper
    {
        public PagingInfoModel? MapFrom(ApiPagingInfoModel? dataModel)
        {
            return dataModel is null ? null : new PagingInfoModel
            {
                PageSize = dataModel.pageSize,
                Skip = dataModel.skip,
                Done = dataModel.done,
                TotalCount = dataModel.totalCount,
            };
        }
    }
}
