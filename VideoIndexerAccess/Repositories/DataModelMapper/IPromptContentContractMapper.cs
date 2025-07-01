using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPromptContentContractMapper
{
    PromptContentContractModel MapFrom(ApiPromptContentContractModel apiModel);
    ApiPromptContentContractModel MapToApiPromptContentContractModel(PromptContentContractModel model);
}