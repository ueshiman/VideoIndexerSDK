using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITextualSummarizationContractPageMapper
{
    TextualSummarizationContractPageModel MapFrom(ApiTextualSummarizationContractPageModel model);
    ApiTextualSummarizationContractPageModel MapTo(TextualSummarizationContractPageModel model);
}