using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IProjectUpdateResponseMapper
{
    ProjectUpdateResponseModel MapFrom(ApiProjectUpdateResponseModel model);
    ApiProjectUpdateResponseModel MapToApiProjectUpdateResponseModel(ProjectUpdateResponseModel model);
}