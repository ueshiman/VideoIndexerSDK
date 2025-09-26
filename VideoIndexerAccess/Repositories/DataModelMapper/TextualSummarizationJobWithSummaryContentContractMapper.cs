using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TextualSummarizationJobWithSummaryContentContractMapper : ITextualSummarizationJobWithSummaryContentContractMapper
    {
        public TextualSummarizationJobWithSummaryContentContractModel MapFrom(ApiAOAITextualSummarizationJobWithSummaryContentContractModel model)
        {
            return new TextualSummarizationJobWithSummaryContentContractModel
            {
                Id = model.id,
                DeploymentName = model.deploymentName,
                AddToEndOfSummaryInstructions = model.addToEndOfSummaryInstructions,
                Disclaimer = model.disclaimer,
                AccountId = model.accountId,
                VideoId = model.videoId,
                State = model.state,
                ModelName = model.modelName,
                SummaryStyle = model.summaryStyle,
                SummaryLength = model.summaryLength,
                IncludedFrames = model.includedFrames,
                CreateTime = model.createTime,
                LastUpdateTime = model.lastUpdateTime,
                FailureMessage = model.failureMessage,
                Progress = model.progress,
                Summary = model.summary,
                SensitiveContentPercent = model.sensitiveContentPercent
            };
        }

        public ApiAOAITextualSummarizationJobWithSummaryContentContractModel MapTo(TextualSummarizationJobWithSummaryContentContractModel model)
        {
            return new ApiAOAITextualSummarizationJobWithSummaryContentContractModel
            {
                id = model.Id,
                deploymentName = model.DeploymentName,
                addToEndOfSummaryInstructions = model.AddToEndOfSummaryInstructions,
                disclaimer = model.Disclaimer,
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
                summary = model.Summary,
                sensitiveContentPercent = model.SensitiveContentPercent
            };
        }
    }
}
