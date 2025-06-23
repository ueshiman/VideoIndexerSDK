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

        public ApiPagingInfoModel? MapToApiPagingInfoModel(PagingInfoModel? dataModel)
        {
            return dataModel is null ? null : new ApiPagingInfoModel
            {
                pageSize = dataModel.PageSize,
                skip = dataModel.Skip,
                done = dataModel.Done,
                totalCount = dataModel.TotalCount,
            };
        }
    }
}
