using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LanguageModelFileDataMapper : ILanguageModelFileDataMapper
    {
        public LanguageModelFileDataModel MapFrom(ApiLanguageModelFileDataModel model)
        {
            return new LanguageModelFileDataModel
            {
                Id = model.id,
                Name = model.name,
                Enable = model.enable,
                Creator = model.creator,
                CreationTime = model.creationTime,
                Content = model.content
            };
        }
        public ApiLanguageModelFileDataModel MapToApiLanguageModelFileDataModel(LanguageModelFileDataModel model)
        {
            return new ApiLanguageModelFileDataModel
            {
                id = model.Id,
                name = model.Name,
                enable = model.Enable,
                creator = model.Creator,
                creationTime = model.CreationTime,
                content = model.Content
            };
        }
    }
}
