using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoMigrationMapper
{
    VideoMigrationModel? MapFrom(ApiVideoMigrationModel? model);
}