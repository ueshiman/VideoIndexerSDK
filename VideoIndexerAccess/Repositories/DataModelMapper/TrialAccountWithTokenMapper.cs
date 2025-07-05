using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialAccountWithTokenMapper : ITrialAccountWithTokenMapper
    {
        public TrialAccountWithTokenModel MapFrom(ApiTrialAccountWithTokenModel model)
        {
            return new TrialAccountWithTokenModel
            {
                Id = model.id,
                Name = model.name,
                Location = model.location,
                AccountType = model.accountType,
                Url = model.url,
                AccessToken = model.accessToken,
                IsInMoveToArm = model.isInMoveToArm,
                IsArmOnly = model.isArmOnly,
                MoveToArmStartedDate = model.moveToArmStartedDate,
            };
        }

        public ApiTrialAccountWithTokenModel MapToApiTrialAccountWithTokenModel(TrialAccountWithTokenModel model)
        {
            return new ApiTrialAccountWithTokenModel
            {
                id = model.Id,
                name = model.Name,
                location = model.Location,
                accountType = model.AccountType,
                url = model.Url,
                accessToken = model.AccessToken,
                isInMoveToArm = model.IsInMoveToArm,
                isArmOnly = model.IsArmOnly,
                moveToArmStartedDate = model.MoveToArmStartedDate
            };
        }
    }
}
