using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialAccountUserMapper : ITrialAccountUserMapper
    {
        public TrialAccountUserModel MapFrom(ApiTrialAccountUserModel model)
        {
            return new TrialAccountUserModel
            {
                Id = model.id,
                Name = model.name,
                Email = model.email,
            };
        }

        public ApiTrialAccountUserModel MapToApiTrialAccountUserModel(TrialAccountUserModel model)
        {
            return new ApiTrialAccountUserModel
            {
                id = model.Id,
                name = model.Name,
                email = model.Email,
            };
        }
    }
}
