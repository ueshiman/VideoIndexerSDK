using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoThumbnailFormatTypeMapper
{
    VideoThumbnailFormatType MapFrom(string formatType);
    string MapToString(VideoThumbnailFormatType formatType);
}