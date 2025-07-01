using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PromptContentItemMapper : IPromptContentItemMapper
    {
        public PromptContentItemModel MapFrom(ApiPromptContentItemModel model)
        {
            return new PromptContentItemModel
            {
                Id = model.id, // Assuming Id is part of ApiPromptContentItemModel
                Start = model.start,
                End = model.end,
                Content = model.content,
                Frames = model.frames?.ToList(),
            };
        }

        public ApiPromptContentItemModel MapToApiApiPromptContentItemModel(PromptContentItemModel model)
        {
            return new ApiPromptContentItemModel
            {
                id = model.Id, // Assuming Id is part of ApiPromptContentItemModel
                start = model.Start,
                end = model.End,
                content = model.Content,
                frames = model.Frames?.ToList(),
            };
        }
    }
}
