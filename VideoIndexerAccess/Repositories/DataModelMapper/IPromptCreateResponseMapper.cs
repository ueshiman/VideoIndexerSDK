using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPromptCreateResponseMapper
{
    PromptCreateResponseModel MapFrom(ApiPromptCreateResponseModel model);
    ApiPromptCreateResponseModel MapTo(PromptCreateResponseModel model);
}