using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class DeleteVideoResultMapper : IDeleteVideoResultMapper
    {
        public DeleteVideoResultModel MapFrom(ApiDeleteVideoResultModel model)
        {
            return new DeleteVideoResultModel
            {
                FailedAssets = model.failedAssets?.ToArray(),
            };
        }

        public ApiDeleteVideoResultModel MapToApiDeleteVideoResultModel(DeleteVideoResultModel model)
        {
            return new ApiDeleteVideoResultModel
            {
                failedAssets = model.FailedAssets?.ToArray(),
            };
        }
    }
}
