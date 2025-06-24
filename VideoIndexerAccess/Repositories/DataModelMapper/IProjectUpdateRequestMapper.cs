using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectUpdateRequestMapper
{
    ProjectUpdateRequestModel MapFrom(ApiProjectUpdateRequestModel model, string projectId);
    ApiProjectUpdateRequestModel MapToApiProjectUpdateRequestModel(ProjectUpdateRequestModel model);
}