using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoMigrationModel
    {
        public VideoMigrationState Status { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? VideoId { get; set; }
    }
}
