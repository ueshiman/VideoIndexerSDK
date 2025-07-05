using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoThumbnailFormatTypeMapper : IVideoThumbnailFormatTypeMapper
    {
        public VideoThumbnailFormatType MapFrom(string formatType)
        {
            return formatType switch
            {
                "Jpeg" => VideoThumbnailFormatType.Jpeg,
                "Base64" => VideoThumbnailFormatType.Base64,
                _ => throw new ArgumentException($"Unknown video thumbnail format type: {formatType}"),
            };
        }

        public string MapToString(VideoThumbnailFormatType formatType)
        {
            return formatType switch
            {
                VideoThumbnailFormatType.Jpeg => "Jpeg",
                VideoThumbnailFormatType.Base64 => "Base64",
                _ => throw new ArgumentException($"Unknown video thumbnail format type: {formatType}"),
            };
        }
    }
}
