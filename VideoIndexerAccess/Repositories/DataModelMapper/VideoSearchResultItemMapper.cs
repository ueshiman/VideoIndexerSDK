using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoSearchResultItemMapper : IVideoSearchResultItemMapper
    {
        public VideoSearchResultItemModel MapFrom(ApiVideoSearchResultItemModel model)
        {
            return new VideoSearchResultItemModel()
            {
                AccountId = model.accountId,
                Id = model.id,
                Name = model.name,
                Description = model.description,
                Created = model.created,
                LastModified = model.lastModified,
                LastIndexed = model.lastIndexed,
                PrivacyMode = model.privacyMode,
                UserName = model.userName,
                IsOwned = model.isOwned,
                IsBase = model.isBase,
                DurationInSeconds = model.durationInSeconds,
                State = model.state,
                ThumbnailVideoId = model.thumbnailVideoId,
                ThumbnailId = model.thumbnailId,
                IndexingPreset = model.indexingPreset,
                StreamingPreset = model.streamingPreset,
                SourceLanguage = model.sourceLanguage
            };
        }

        public ApiVideoSearchResultItemModel MapToApiVideoSearchResultItemModel(VideoSearchResultItemModel model)
        {
            return new ApiVideoSearchResultItemModel()
            {
                accountId = model.AccountId,
                id = model.Id,
                name = model.Name,
                description = model.Description,
                created = model.Created,
                lastModified = model.LastModified,
                lastIndexed = model.LastIndexed,
                privacyMode = model.PrivacyMode,
                userName = model.UserName,
                isOwned = model.IsOwned,
                isBase = model.IsBase,
                durationInSeconds = model.DurationInSeconds,
                state = model.State,
                thumbnailVideoId = model.ThumbnailVideoId,
                thumbnailId = model.ThumbnailId,
                indexingPreset = model.IndexingPreset,
                streamingPreset = model.StreamingPreset,
                sourceLanguage = model.SourceLanguage
            };
        }
    }
}
