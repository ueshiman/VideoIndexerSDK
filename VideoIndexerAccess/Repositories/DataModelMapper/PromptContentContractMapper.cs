using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PromptContentContractMapper : IPromptContentContractMapper
    {
        private readonly IPromptContentItemMapper _promptContentItemMapper;

        public PromptContentContractMapper(IPromptContentItemMapper promptContentItemMapper)
        {
            _promptContentItemMapper = promptContentItemMapper;
        }

        public PromptContentContractModel MapFrom(ApiPromptContentContractModel apiModel)
        {
            return new PromptContentContractModel
            {
                Partition = apiModel.partition,
                Name = apiModel.name,
                Sections = apiModel.sections?.Select(_promptContentItemMapper.MapFrom).ToList()
            };
        }

        public ApiPromptContentContractModel MapToApiPromptContentContractModel(PromptContentContractModel model)
        {
            return new ApiPromptContentContractModel
            {
                partition = model.Partition,
                name = model.Name,
                sections = model.Sections?.Select(_promptContentItemMapper.MapToApiApiPromptContentItemModel).ToList()
            };
        }
    }
}
