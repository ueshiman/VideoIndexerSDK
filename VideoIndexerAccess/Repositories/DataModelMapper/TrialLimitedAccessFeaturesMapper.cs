using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialLimitedAccessFeaturesMapper : ITrialLimitedAccessFeaturesMapper
    {
        public TrialLimitedAccessFeaturesModel MapFrom(ApiTrialLimitedAccessFeaturesModel model)
        {
            return new TrialLimitedAccessFeaturesModel
            {
                IsFaceIdentificationEnabled = model.isFaceIdentificationEnabled,
                IsCelebrityRecognitionEnabled = model.isCelebrityRecognitionEnabled,
                IsFaceDetectionEnabled = model.isFaceDetectionEnabled,
            };
        }

        public ApiTrialLimitedAccessFeaturesModel MapToApiTrialLimitedAccessFeaturesModel(TrialLimitedAccessFeaturesModel model)
        {
            return new ApiTrialLimitedAccessFeaturesModel
            {
                isFaceIdentificationEnabled = model.IsFaceIdentificationEnabled,
                isCelebrityRecognitionEnabled = model.IsCelebrityRecognitionEnabled,
                isFaceDetectionEnabled = model.IsFaceDetectionEnabled,
            };
        }
    }
}
