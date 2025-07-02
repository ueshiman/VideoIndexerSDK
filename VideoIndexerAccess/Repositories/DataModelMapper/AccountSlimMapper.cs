using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class AccountSlimMapper : IAccountSlimMapper
    {
        public AccountSlimModel MapFrom(ApiAccountSlimModel apiAccountSlimModel)
        {
            return new AccountSlimModel
            {
                Id = apiAccountSlimModel.id,
                Name = apiAccountSlimModel.name,
                Location = apiAccountSlimModel.location,
                AccountType = apiAccountSlimModel.accountType,
                Url = apiAccountSlimModel.url,
                AccessToken = apiAccountSlimModel.accessToken,
                IsInMoveToArm = apiAccountSlimModel.isInMoveToArm,
                IsArmOnly = apiAccountSlimModel.isArmOnly,
                MoveToArmStartedDate = apiAccountSlimModel.moveToArmStartedDate
            };
        }

        public ApiAccountSlimModel MapToApiAccountSlimModel(AccountSlimModel accountSlimModel)
        {
            return new ApiAccountSlimModel
            {
                id = accountSlimModel.Id,
                name = accountSlimModel.Name,
                location = accountSlimModel.Location,
                accountType = accountSlimModel.AccountType,
                url = accountSlimModel.Url,
                accessToken = accountSlimModel.AccessToken,
                isInMoveToArm = accountSlimModel.IsInMoveToArm,
                isArmOnly = accountSlimModel.IsArmOnly,
                moveToArmStartedDate = accountSlimModel.MoveToArmStartedDate
            };
        }
    }
}
