using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialAccountWithTokenMapper
{
    TrialAccountWithTokenModel MapFrom(ApiTrialAccountWithTokenModel model);
    ApiTrialAccountWithTokenModel MapToApiTrialAccountWithTokenModel(TrialAccountWithTokenModel model);
}