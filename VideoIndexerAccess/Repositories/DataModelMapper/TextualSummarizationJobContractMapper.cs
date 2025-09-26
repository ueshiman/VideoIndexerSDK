using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TextualSummarizationJobContractMapper : ITextualSummarizationJobContractMapper
    {
        public TextualSummarizationJobContractModel MapFrom(ApiAOAITextualSummarizationJobContractModel apiModel)
        {
            return new TextualSummarizationJobContractModel
            {
                Id = apiModel.id,
                AccountId = apiModel.accountId,
                VideoId = apiModel.videoId,
                State = apiModel.state,
                ModelName = apiModel.modelName,
                SummaryStyle = apiModel.summaryStyle,
                SummaryLength = apiModel.summaryLength,
                IncludedFrames = apiModel.includedFrames,
                CreateTime = apiModel.createTime,
                LastUpdateTime = apiModel.lastUpdateTime,
                FailureMessage = apiModel.failureMessage,
                Progress = apiModel.progress,
                DeploymentName = apiModel.deploymentName,
                Disclaimer = apiModel.disclaimer
            };
        }

        public ApiAOAITextualSummarizationJobContractModel MapTo(TextualSummarizationJobContractModel model)
        {
            return new ApiAOAITextualSummarizationJobContractModel
            {
                id = model.Id,
                accountId = model.AccountId,
                videoId = model.VideoId,
                state = model.State,
                modelName = model.ModelName,
                summaryStyle = model.SummaryStyle,
                summaryLength = model.SummaryLength,
                includedFrames = model.IncludedFrames,
                createTime = model.CreateTime,
                lastUpdateTime = model.LastUpdateTime,
                failureMessage = model.FailureMessage,
                progress = model.Progress,
                deploymentName = model.DeploymentName,
                disclaimer = model.Disclaimer
            };
        }
    }
}
