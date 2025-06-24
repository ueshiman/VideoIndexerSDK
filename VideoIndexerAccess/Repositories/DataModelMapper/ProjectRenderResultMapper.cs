using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectRenderResultMapper : IProjectRenderResultMapper
    {
        private readonly IVideoTimeRangeMapper _videoTimeRangeMapper;

        public ProjectRenderResultMapper(IVideoTimeRangeMapper videoTimeRangeMapper)
        {
            _videoTimeRangeMapper = videoTimeRangeMapper;
        }

        public ProjectRenderResultModel? MapFrom(ApiProjectRenderResultModel? model) => model is null ? null :  new () { VideoRanges = model?.videoRanges?.Select(_videoTimeRangeMapper.MapFrom).ToArray(), };
        public ApiProjectRenderResultModel? MapToApiProjectRenderResultModel(ProjectRenderResultModel? model) => model  is null ? null : new () { videoRanges = model.VideoRanges?.Select(_videoTimeRangeMapper.MapToApiVideoTimeRangeModel).ToArray(), };
    }
}
