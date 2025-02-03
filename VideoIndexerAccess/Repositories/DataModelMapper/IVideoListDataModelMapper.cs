using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoListDataModelMapper
{
    VideoListDataModel MapFrom(ApiVideoListItemModel dataModel);
}