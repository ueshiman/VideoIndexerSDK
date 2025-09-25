using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class SpeechDatasetPropertiesMapper : ISpeechDatasetPropertiesMapper
    {
        public SpeechDatasetPropertiesModel MapFrom(ApiSpeechDatasetPropertiesModel apiModel)
        {
            return new SpeechDatasetPropertiesModel
            {
                AcceptedLineCount = apiModel.acceptedLineCount,
                RejectedLineCount = apiModel.rejectedLineCount,
                Duration = apiModel.duration,
                Error = apiModel.error
            };  
        }

        public ApiSpeechDatasetPropertiesModel MapToApiSpeechDatasetPropertiesModel(SpeechDatasetPropertiesModel domainModel)
        {
            return new ApiSpeechDatasetPropertiesModel
            {
                acceptedLineCount = domainModel.AcceptedLineCount,
                rejectedLineCount = domainModel.RejectedLineCount,
                duration = domainModel.Duration,
                error = domainModel.Error
            };
        }
    }
}
