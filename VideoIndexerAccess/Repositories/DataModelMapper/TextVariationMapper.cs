using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TextVariationMapper : ITextVariationMapper
    {
        public TextVariationModel MapFrom(ApiTextVariationModel apiTextVariationModel)
        {
            return new TextVariationModel
            {
                Text = apiTextVariationModel.text ?? string.Empty,
                CaseSensitive = apiTextVariationModel.caseSensitive
            };
        }

        public ApiTextVariationModel ToApiTextVariationModel(TextVariationModel textVariationModel)
        {
            return new ApiTextVariationModel
            {
                text = textVariationModel.Text,
                caseSensitive = textVariationModel.CaseSensitive
            };
        }
    }
}
