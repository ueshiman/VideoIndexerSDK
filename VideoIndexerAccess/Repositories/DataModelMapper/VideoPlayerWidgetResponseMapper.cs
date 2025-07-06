using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoPlayerWidgetResponseMapper : IVideoPlayerWidgetResponseMapper
    {
        public VideoPlayerWidgetResponseModel MapFrom(ApiVideoPlayerWidgetResponseModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new VideoPlayerWidgetResponseModel
            {
                PlayerWidgetUrl = model.playerWidgetUrl,
                ErrorType = model.errorType,
                Message = model.message
            };
        }

        public ApiVideoPlayerWidgetResponseModel MapToApiVideoPlayerWidgetResponseModel(VideoPlayerWidgetResponseModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new ApiVideoPlayerWidgetResponseModel
            {
                playerWidgetUrl = model.PlayerWidgetUrl,
                errorType = model.ErrorType,
                message = model.Message
            };
        }
    }
}
