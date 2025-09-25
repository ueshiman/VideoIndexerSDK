using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class CustomPersonMapper : ICustomPersonMapper
    {
        public CustomPersonModel MapFrom(ApiCustomPersonModel apiModel)
        {
            return new CustomPersonModel
            {
                Id = apiModel.id,
                Name = apiModel.name,
                IsDefault = apiModel.isDefault,
                PersonsCount = apiModel.personsCount,
                PersonIdentificationThreshold = apiModel.personIdentificationThreshold
            };
        }

        public ApiCustomPersonModel MapToApiCustomPersonModel(CustomPersonModel model)
        {
            return new ApiCustomPersonModel
            {
                id = model.Id,
                name = model.Name,
                isDefault = model.IsDefault,
                personsCount = model.PersonsCount,
                personIdentificationThreshold = model.PersonIdentificationThreshold
            };
        }
    }
}
