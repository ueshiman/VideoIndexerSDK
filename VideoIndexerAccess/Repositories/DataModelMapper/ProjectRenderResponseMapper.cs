using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectRenderResponseMapper : IProjectRenderResponseMapper
    {
        private readonly IProjectRenderResultMapper _projectRenderResultMapper;
        private readonly IErrorResponseMapper _errorResponseMapper;

        public ProjectRenderResponseMapper(IProjectRenderResultMapper projectRenderResultMapper, IErrorResponseMapper errorResponseMapper)
        {
            _projectRenderResultMapper = projectRenderResultMapper;
            _errorResponseMapper = errorResponseMapper;
        }

        public ProjectRenderResponseModel MapFrom(ApiProjectRenderResponseModel model)
        {
            return new ProjectRenderResponseModel
            {
                State = model.state,
                Result = _projectRenderResultMapper.MapFrom(model.result),
                Error = _errorResponseMapper.MapFrom(model.error),
            };
        }

        public ApiProjectRenderResponseModel MapToApiProjectRenderResponseModel(ProjectRenderResponseModel model)
        {
            return new ApiProjectRenderResponseModel
            {
                state = model.State,
                result = model.Result is null ? null : _projectRenderResultMapper.MapToApiProjectRenderResultModel(model.Result),
                error = _errorResponseMapper.MapToApiErrorResponseModel(model.Error),
            };
        }
    }
}
