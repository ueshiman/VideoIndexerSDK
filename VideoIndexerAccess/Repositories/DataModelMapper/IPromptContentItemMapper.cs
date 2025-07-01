using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPromptContentItemMapper
{
    PromptContentItemModel MapFrom(ApiPromptContentItemModel model);
    ApiPromptContentItemModel MapToApiApiPromptContentItemModel(PromptContentItemModel model);
}