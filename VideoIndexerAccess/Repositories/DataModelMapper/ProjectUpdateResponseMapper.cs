using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectUpdateResponseMapper : IProjectUpdateResponseMapper
    {
        public ProjectUpdateResponseModel MapFrom(ApiProjectUpdateResponseModel model)
        {
            return new ProjectUpdateResponseModel
            {
                AccountId = model.accountId ?? string.Empty,
                Id = model.id ?? string.Empty,
                Name = model.name ?? string.Empty,
                Created = model.created ?? string.Empty,
                LastModified = model.lastModified ?? string.Empty,
                UserName = model.userName ?? string.Empty,
                DurationInSeconds = model.durationInSeconds,
                ThumbnailVideoId = model.thumbnailVideoId ?? string.Empty,
                ThumbnailId = model.thumbnailId ?? string.Empty,
            };
        }

        public ApiProjectUpdateResponseModel MapToApiProjectUpdateResponseModel(ProjectUpdateResponseModel model)
        {
            return new ApiProjectUpdateResponseModel
            {
                accountId = model.AccountId,
                id = model.Id,
                name = model.Name,
                created = model.Created,
                lastModified = model.LastModified,
                userName = model.UserName,
                durationInSeconds = model.DurationInSeconds,
                thumbnailVideoId = model.ThumbnailVideoId,
                thumbnailId = model.ThumbnailId,
            };
        }
    }
}
