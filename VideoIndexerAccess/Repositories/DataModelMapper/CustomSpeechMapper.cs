using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class CustomSpeechMapper : ICustomSpeechMapper
    {
        public CustomSpeechModel MapFrom(ApiCustomSpeechModel apiModel)
        {
            return new CustomSpeechModel()
            {
                Id = apiModel.id,
                Properties = apiModel.properties,
                DisplayName = apiModel.displayName,
                Description = apiModel.description,
                Locale = apiModel.locale,
                Datasets = apiModel.datasets,
                Status = apiModel.status,
                LastActionDateTime = apiModel.lastActionDateTime,
                CreatedDateTime = apiModel.createdDateTime,
                CustomProperties = apiModel.customProperties
            };
        }

        public ApiCustomSpeechModel MapToApiCustomSpeechModel(CustomSpeechModel model)
        {
            return new ApiCustomSpeechModel()
            {
                id = model.Id,
                properties = model.Properties,
                displayName = model.DisplayName,
                description = model.Description,
                locale = model.Locale,
                datasets = model.Datasets,
                status = model.Status,
                lastActionDateTime = model.LastActionDateTime,
                createdDateTime = model.CreatedDateTime,
                customProperties = model.CustomProperties
            };
        }
    }
}

