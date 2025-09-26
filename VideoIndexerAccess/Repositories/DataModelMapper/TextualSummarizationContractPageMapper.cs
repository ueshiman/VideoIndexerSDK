using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TextualSummarizationContractPageMapper : ITextualSummarizationContractPageMapper
    {
        private readonly ITextualSummarizationJobContractMapper _textualSummarizationJobContractMapper;

        public TextualSummarizationContractPageMapper(ITextualSummarizationJobContractMapper textualSummarizationJobContractMapper)
        {
            _textualSummarizationJobContractMapper = textualSummarizationJobContractMapper;
        }

        public TextualSummarizationContractPageModel MapFrom(ApiTextualSummarizationContractPageModel model)
        {
            return new TextualSummarizationContractPageModel
            {
                size = model.size,
                pageNumber = model.pageNumber,
                done = model.done,
                items = model.items?.Select(_textualSummarizationJobContractMapper.MapFrom).ToList()
            };
        }
        public ApiTextualSummarizationContractPageModel MapTo(TextualSummarizationContractPageModel model)
        {
            return new ApiTextualSummarizationContractPageModel
            {
                size = model.size,
                pageNumber = model.pageNumber,
                done = model.done,
                items = model.items?.Select(_textualSummarizationJobContractMapper.MapTo).ToList()
            };
        }
    }
}
