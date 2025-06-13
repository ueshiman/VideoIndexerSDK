using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IJobStatusResponseMapper
{
    JobStatusResponseModel MapFrom(ApiJobStatusResponseModel model);
    ApiJobStatusResponseModel MapToApiJobStatusResponseModel(JobStatusResponseModel model);
}