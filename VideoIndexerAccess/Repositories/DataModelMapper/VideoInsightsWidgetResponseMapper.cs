using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoInsightsWidgetResponseMapper : IVideoInsightsWidgetResponseMapper
    {
        public VideoInsightsWidgetResponseModel MapFrom(ApiVideoInsightsWidgetResponseModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new VideoInsightsWidgetResponseModel
            {
                WidgetUrl = model.widgetUrl,
                ErrorType = model.errorType,
                Message = model.message,
                RequestId = model.requestId
            };
        }

        public ApiVideoInsightsWidgetResponseModel MapToApiVideoInsightsWidgetResponseModel(VideoInsightsWidgetResponseModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));
            return new ApiVideoInsightsWidgetResponseModel
            {
                widgetUrl = model.WidgetUrl,
                errorType = model.ErrorType,
                message = model.Message,
                requestId = model.RequestId
            };
        }
    }
}
