using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoDetailsMapper : IVideoDetailsMapper
    {
        public VideoDetailsModel MapFrom(ApiVideoDetailsModel apiVideoDetailsModel)
        {
            return new VideoDetailsModel
            {
                PublishedUrl = apiVideoDetailsModel.publishedUrl,
                ViewToken = apiVideoDetailsModel.viewToken,
                State = apiVideoDetailsModel.state,
                ProcessingProgress = apiVideoDetailsModel.processingProgress,
                FailureMessage = apiVideoDetailsModel.failureMessage
            };
        }

        public ApiVideoDetailsModel MapToApiVideoDetailsModel(VideoDetailsModel videoDetailsModel)
        {
            return new ApiVideoDetailsModel
            {
                publishedUrl = videoDetailsModel.PublishedUrl,
                viewToken = videoDetailsModel.ViewToken,
                state = videoDetailsModel.State,
                processingProgress = videoDetailsModel.ProcessingProgress,
                failureMessage = videoDetailsModel.FailureMessage
            };
        }
    }
}
