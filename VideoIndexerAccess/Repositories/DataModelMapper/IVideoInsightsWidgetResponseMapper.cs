﻿using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IVideoInsightsWidgetResponseMapper
{
    VideoInsightsWidgetResponseModel MapFrom(ApiVideoInsightsWidgetResponseModel model);
    ApiVideoInsightsWidgetResponseModel MapToApiVideoInsightsWidgetResponseModel(VideoInsightsWidgetResponseModel model);
}