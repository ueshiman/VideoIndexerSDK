using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IRedactVideoRequestMapper
{
    RedactVideoRequestModel MapFroModel(ApiRedactVideoRequestModel model);
    ApiRedactVideoRequestModel MapToApiRedactVideoRequestModel(RedactVideoRequestModel model);
}