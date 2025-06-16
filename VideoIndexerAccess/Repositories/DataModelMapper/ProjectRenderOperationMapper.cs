using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectRenderOperationMapper : IProjectRenderOperationMapper
    {
        private readonly IProjectRenderOperationResultMapper _projectRenderOperationResultMapper;
        private readonly IErrorResponseMapper _errorResponseMapper;
        public ProjectRenderOperationMapper(IProjectRenderOperationResultMapper projectRenderOperationResultMapper, IErrorResponseMapper errorResponseMapper)
        {
            _projectRenderOperationResultMapper = projectRenderOperationResultMapper;
            _errorResponseMapper = errorResponseMapper;
        }
        public ProjectRenderOperationModel MapFrom(ApiProjectRenderOperationModel model)
        {
            return new ProjectRenderOperationModel
            {
                State = model.state,
                Result = model.result is null ? null : _projectRenderOperationResultMapper.MapFrom(model.result),
                Error = model.error is null ? null : _errorResponseMapper.MapFrom(model.error),
            };
        }
        public ApiProjectRenderOperationModel MapToApiProjectRenderOperationModel(ProjectRenderOperationModel model)
        {
            return new ApiProjectRenderOperationModel
            {
                state = model.State,
                error = model.Error is null ? null : _errorResponseMapper.MapToApiErrorResponseModel(model.Error),
                result = model.Result is null ? null : _projectRenderOperationResultMapper.MapToApiProjectRenderOperationResultModel(model.Result),
            };
        }
    }
}
