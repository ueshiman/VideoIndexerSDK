using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectSearchResultItemMapper : IProjectSearchResultItemMapper
    {
        private readonly IVideoSearchMatchMapper _videoSearchMatchMapper;

        public ProjectSearchResultItemMapper(IVideoSearchMatchMapper videoSearchMatchMapper)
        {
            _videoSearchMatchMapper = videoSearchMatchMapper;
        }

        public ProjectSearchResultItemModel MapFrom(ApiProjectSearchResultItemModel model)
        {
            return new ProjectSearchResultItemModel
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
                SearchMatches = model.searchMatches?.Select(_videoSearchMatchMapper.MapFrom).ToList(),
            };
        }

        public ApiProjectSearchResultItemModel MapToApiProjectSearchResultItemModel(ProjectSearchResultItemModel model)
        {
            return new ApiProjectSearchResultItemModel
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
                searchMatches = model.SearchMatches?.Select(_videoSearchMatchMapper.MapToApiVideoSearchMatchModel).ToList(),
            };
        }
    }
}
