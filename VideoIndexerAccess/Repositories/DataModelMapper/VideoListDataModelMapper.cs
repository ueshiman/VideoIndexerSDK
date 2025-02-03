using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoListDataModelMapper : IVideoListDataModelMapper
    {
        public VideoListDataModel MapFrom(ApiVideoListItemModel dataModel)
        {
            return new VideoListDataModel
            {
                AccountId = dataModel.accountId,
                Id = dataModel.id,
                Partition = dataModel.partition,
                ExternalId = dataModel.externalId,
                Metadata = dataModel.metadata,
                Name = dataModel.name,
                Description = dataModel.description,
                Created = dataModel.created,
                LastModified = dataModel.lastModified,
                LastIndexed = dataModel.lastIndexed,
                PrivacyMode = dataModel.privacyMode,
                UserName = dataModel.userName,
                IsOwned = dataModel.isOwned,
                IsBase = dataModel.isBase,
                HasSourceVideoFile = dataModel.hasSourceVideoFile,
                State = dataModel.state,
                ModerationState = dataModel.moderationState,
                ReviewState = dataModel.reviewState,
                IsSearchable = dataModel.isSearchable,
                ProcessingProgress = dataModel.processingProgress,
                DurationInSeconds = dataModel.durationInSeconds,
                ThumbnailVideoId = dataModel.thumbnailVideoId,
                ThumbnailId = dataModel.thumbnailId,
                SearchMatches = dataModel?.searchMatches?.ToArray() ?? Array.Empty<object>(),
                IndexingPreset = dataModel?.indexingPreset,
                StreamingPreset = dataModel?.streamingPreset,
                SourceLanguage = dataModel?.sourceLanguage,
                SourceLanguages = dataModel?.sourceLanguages?.ToArray() ?? Array.Empty<string>(),
                PersonModelId = dataModel?.personModelId,
                ThumbnailUrl = dataModel?.thumbnailUrl,
            };
        }
    }
}
