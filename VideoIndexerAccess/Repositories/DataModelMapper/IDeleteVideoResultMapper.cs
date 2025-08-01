﻿using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IDeleteVideoResultMapper
{
    DeleteVideoResultModel MapFrom(ApiDeleteVideoResultModel model);
    ApiDeleteVideoResultModel MapToApiDeleteVideoResultModel(DeleteVideoResultModel model);
}