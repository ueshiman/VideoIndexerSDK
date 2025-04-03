using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoMigrationStateMapper
{
    VideoMigrationState? MapFrom(ApiVideoMigrationState? state);
}