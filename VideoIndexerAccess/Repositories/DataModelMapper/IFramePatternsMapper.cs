﻿using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IFramePatternsMapper
{
    FramePatterns MapFrom(FramePatternsApiModel model);
}