using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoIndexResponseMapper : IVideoIndexResponseMapper
    {
        private readonly IVideoDetailsMapper _videoDetailsMapper;

        public VideoIndexResponseMapper(IVideoDetailsMapper videoDetailsMapper)
        {
            _videoDetailsMapper = videoDetailsMapper;
        }

        public VideoIndexResponseModel MapFrom(ApiVideoIndexResponseModel apiResponse)
        {
            return new VideoIndexResponseModel
            {
                AccountId = apiResponse.accountId,
                Id = apiResponse.id,
                Name = apiResponse.name,
                UserName = apiResponse.userName,
                Created = apiResponse.created,
                IsOwned = apiResponse.isOwned,
                IsEditable = apiResponse.isEditable,
                IsBase = apiResponse.isBase,
                DurationInSeconds = apiResponse.durationInSeconds,
                Duration = apiResponse.duration,
                Videos = apiResponse.videos?.Select(_videoDetailsMapper.MapFrom).ToList()
            };
        }
        public ApiVideoIndexResponseModel MapToApiVideoIndexResponseModel(VideoIndexResponseModel responseModel)
        {
            return new ApiVideoIndexResponseModel
            {
                accountId = responseModel.AccountId,
                id = responseModel.Id,
                name = responseModel.Name,
                userName = responseModel.UserName,
                created = responseModel.Created,
                isOwned = responseModel.IsOwned,
                isEditable = responseModel.IsEditable,
                isBase = responseModel.IsBase,
                durationInSeconds = responseModel.DurationInSeconds,
                duration = responseModel.Duration,
                videos = responseModel.Videos?.Select(_videoDetailsMapper.MapToApiVideoDetailsModel).ToList()
            };
        }

    }
}
