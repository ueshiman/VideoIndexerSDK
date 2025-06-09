using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoMigrationsListMapper
{
    VideoMigrationsListModel? MapFrom(ApiVideoMigrationsListModel? dataModel);
}