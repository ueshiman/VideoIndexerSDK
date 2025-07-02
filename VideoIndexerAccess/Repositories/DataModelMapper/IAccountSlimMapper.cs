using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IAccountSlimMapper
{
    AccountSlimModel MapFrom(ApiAccountSlimModel apiAccountSlimModel);
    ApiAccountSlimModel MapToApiAccountSlimModel(AccountSlimModel accountSlimModel);
}