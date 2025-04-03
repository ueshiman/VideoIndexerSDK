using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoMigrationStateMapper : IVideoMigrationStateMapper
    {
        public VideoMigrationState? MapFrom(ApiVideoMigrationState? state)
        {
            return state switch
            {
                ApiVideoMigrationState.NotStarted => VideoMigrationState.NotStarted,
                ApiVideoMigrationState.Pending => VideoMigrationState.Pending,
                ApiVideoMigrationState.InProgress => VideoMigrationState.InProgress,
                ApiVideoMigrationState.Success => VideoMigrationState.Success,
                ApiVideoMigrationState.Failed => VideoMigrationState.Failed,
                ApiVideoMigrationState.NotApplicable => VideoMigrationState.NotApplicable,
                ApiVideoMigrationState.PendingUserAction => VideoMigrationState.PendingUserAction,
                _ => null,
                //_ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
            };
        }
    }
}
