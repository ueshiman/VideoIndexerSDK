using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoMigrationMapper : IVideoMigrationMapper
    {
        private readonly IVideoMigrationStateMapper _videoMigrationStateMapper;

        public VideoMigrationMapper(IVideoMigrationStateMapper videoMigrationStateMapper)
        {
            _videoMigrationStateMapper = videoMigrationStateMapper;
        }

        public VideoMigrationModel? MapFrom(ApiVideoMigrationModel? model)
        {
            return model is null ? null : new VideoMigrationModel
            {
                Status = _videoMigrationStateMapper.MapFrom(model?.status) ?? throw new NullReferenceException(),
                Name = model?.name,
                Details = model?.details,
                VideoId = model?.videoId,
            };
        }
    }
}
