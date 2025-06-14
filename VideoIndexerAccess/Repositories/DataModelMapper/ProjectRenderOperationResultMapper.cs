using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectRenderOperationResultMapper : IProjectRenderOperationResultMapper
    {
        private readonly IVideoTimeRangeMapper _videoTimeRangeMapper;

        public ProjectRenderOperationResultMapper(IVideoTimeRangeMapper videoTimeRangeMapper)
        {
            _videoTimeRangeMapper = videoTimeRangeMapper;
        }

        public ProjectRenderOperationResultModel MapFrom(ApiProjectRenderOperationResultModel model)
        {
            return new ProjectRenderOperationResultModel
            {
                videoRanges = model.videoRanges.Select(x => _videoTimeRangeMapper.MapFrom(x)).ToList()
            };
        }

        public ApiProjectRenderOperationResultModel MapToApiProjectRenderOperationResultModel(ProjectRenderOperationResultModel model)
        {
            return new ApiProjectRenderOperationResultModel
            {
                videoRanges = model.videoRanges.Select(x => _videoTimeRangeMapper.MapToApiVideoTimeRangeModel(x)).ToList()
            };
        }
    }
}
