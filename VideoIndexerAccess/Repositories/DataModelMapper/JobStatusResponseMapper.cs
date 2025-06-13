using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class JobStatusResponseMapper : IJobStatusResponseMapper
    {
        public JobStatusResponseModel MapFrom(ApiJobStatusResponseModel model)
        {
            return new JobStatusResponseModel()
            {
                CreationTime = model.creationTime,
                LastUpdateTime = model.lastUpdateTime,
                Progress = model.progress,
                JobType = model.jobType,
                State = model.state,
            };
        }
        public ApiJobStatusResponseModel MapToApiJobStatusResponseModel(JobStatusResponseModel model)
        {
            return new ApiJobStatusResponseModel()
            {
                creationTime = model.CreationTime,
                lastUpdateTime = model.LastUpdateTime,
                progress = model.Progress,
                jobType = model.JobType,
                state = model.State,
            };
        }
    }
}
