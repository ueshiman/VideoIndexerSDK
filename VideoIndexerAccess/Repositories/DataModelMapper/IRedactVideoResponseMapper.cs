using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IRedactVideoResponseMapper
{
    RedactVideoResponseModel MapFrom(ApiRedactVideoResponseModel apiResponse);
    ApiRedactVideoResponseModel MapToApiRedactVideoResponseModel(RedactVideoResponseModel responseModel);
}