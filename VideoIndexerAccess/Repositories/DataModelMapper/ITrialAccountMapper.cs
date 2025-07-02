using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialAccountMapper
{
    TrialAccountModel MapFrom(ApiTrialAccountModel model);
}