using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ISummarizedInsightsMapper
{
    Summarizedinsights? MapFrom(SummarizedinsightsApiModel? model);
}