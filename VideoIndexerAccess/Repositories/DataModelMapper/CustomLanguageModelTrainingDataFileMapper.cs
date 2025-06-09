using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class CustomLanguageModelTrainingDataFileMapper : ICustomLanguageModelTrainingDataFileMapper
    {
        public ApiCustomLanguageModelTrainingDataFileModel MapToApiCustomLanguageModelTrainingDataFileModel(CustomLanguageModelTrainingDataFileModel model)
        {
            return new ApiCustomLanguageModelTrainingDataFileModel
            {
                id = model.Id,
                name = model.Name,
                enable = model.Enable,
                creator = model.Creator,
                creationTime = model.CreationTime
            };
        }
        public CustomLanguageModelTrainingDataFileModel MapFrom(ApiCustomLanguageModelTrainingDataFileModel apiModel)
        {
            return new CustomLanguageModelTrainingDataFileModel
            {
                Id = apiModel.id,
                Name = apiModel.name,
                Enable = apiModel.enable,
                Creator = apiModel.creator,
                CreationTime = apiModel.creationTime
            };
        }
    }
}
