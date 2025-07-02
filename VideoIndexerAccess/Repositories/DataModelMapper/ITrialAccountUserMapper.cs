using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialAccountUserMapper
{
    TrialAccountUserModel MapFrom(ApiTrialAccountUserModel model);
    ApiTrialAccountUserModel MapToApiTrialAccountUserModel(TrialAccountUserModel model);
}