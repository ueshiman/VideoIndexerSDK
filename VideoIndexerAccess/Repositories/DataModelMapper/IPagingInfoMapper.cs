using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPagingInfoMapper
{
    PagingInfoModel? MapFrom(ApiPagingInfoModel? dataModel);
    ApiPagingInfoModel? MapToApiPagingInfoModel(PagingInfoModel? dataModel);
}