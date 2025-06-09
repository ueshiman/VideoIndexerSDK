using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LanguageModelFileMetadataMapper : ILanguageModelFileMetadataMapper
    {
        public LanguageModelFileMetadataModel MapFrom(ApiLanguageModelFileMetadataModel model)
        {
            return new LanguageModelFileMetadataModel
            {
                Id = model.id,
                Name = model.name,
                Enable = model.enable,
                Creator = model.creator,
                CreationTime = model.creationTime,
            };
        }

        public ApiLanguageModelFileMetadataModel MapToApiLanguageModelFileMetadataModel(LanguageModelFileMetadataModel model)
        {
            return new ApiLanguageModelFileMetadataModel
            {
                id = model.Id,
                name = model.Name,
                enable = model.Enable,
                creator = model.Creator,
                creationTime = model.CreationTime,
            };
        }
    }
}
