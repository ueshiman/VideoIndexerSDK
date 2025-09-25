using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class SupportedLanguageMapper : ISupportedLanguageMapper
    {
        public SupportedLanguageModel MapFrom(ApiSupportedLanguageModel model)
        {
            return new SupportedLanguageModel
            {
                Name = model.name,
                LanguageCode = model.languageCode,
                IsRightToLeft = model.isRightToLeft,
                IsSourceLanguage = model.isSourceLanguage,
                IsAutoDetect = model.isAutoDetect,
                IsSupportedForLanguageDataset = model.isSupportedForLanguageDataset,
                IsSupportedForPronunciationDataset = model.isSupportedForPronunciationDataset,
                IsSupportedForCustomModels = model.isSupportedForCustomModels,
                IsSupportedForTranslation = model.isSupportedForTranslation
            };
        }

        public ApiSupportedLanguageModel MapToApiSupportedLanguageModel(SupportedLanguageModel model)
        {
            return new ApiSupportedLanguageModel
            {
                name = model.Name,
                languageCode = model.LanguageCode,
                isRightToLeft = model.IsRightToLeft,
                isSourceLanguage = model.IsSourceLanguage,
                isAutoDetect = model.IsAutoDetect,
                isSupportedForLanguageDataset = model.IsSupportedForLanguageDataset,
                isSupportedForPronunciationDataset = model.IsSupportedForPronunciationDataset,
                isSupportedForCustomModels = model.IsSupportedForCustomModels,
                isSupportedForTranslation = model.IsSupportedForTranslation
            };
        }
    }
}
