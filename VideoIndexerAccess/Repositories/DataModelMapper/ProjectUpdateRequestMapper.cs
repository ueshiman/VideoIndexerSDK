using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectUpdateRequestMapper : IProjectUpdateRequestMapper
    {
        private readonly IVideoTimeRangeMapper _videoTimeRangeMapper;

        public ProjectUpdateRequestMapper(IVideoTimeRangeMapper videoTimeRangeMapper)
        {
            _videoTimeRangeMapper = videoTimeRangeMapper;
        }

        public ProjectUpdateRequestModel MapFrom(ApiProjectUpdateRequestModel model, string projectId)
        {
            return new ProjectUpdateRequestModel
            {
                ProjectId = projectId,
                Name = model.name,
                VideosRanges = model.videosRanges.Select(_videoTimeRangeMapper.MapFrom).ToArray(),
                IsSearchable = model.isSearchable,
            };
        }

        public ApiProjectUpdateRequestModel MapToApiProjectUpdateRequestModel(ProjectUpdateRequestModel model)
        {
            return new ApiProjectUpdateRequestModel
            {
                name = model.Name,
                videosRanges = model.VideosRanges.Select(_videoTimeRangeMapper.MapToApiVideoTimeRangeModel).ToArray(),
                isSearchable = model.IsSearchable,
            };
        }
    }
}
