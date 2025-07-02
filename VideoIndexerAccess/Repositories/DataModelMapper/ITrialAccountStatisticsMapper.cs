using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITrialAccountStatisticsMapper
{
    TrialAccountStatisticsModel MapFrom(ApiTrialAccountStatisticsModel model);
    ApiTrialAccountStatisticsModel MapToApiTrialAccountStatisticsModel(TrialAccountStatisticsModel model);
}