using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TextVariationMapper : ITextVariationMapper
    {
        public TextVariationModel MapFrom(ApiTextVariationModel model)
        {
            return new TextVariationModel
            {
                Text = model.text,
                CaseSensitive = model.caseSensitive
            };
        }
        public ApiTextVariationModel MapToApiTextVariationModel(TextVariationModel model)
        {
            return new ApiTextVariationModel
            {
                text = model.Text,
                caseSensitive = model.CaseSensitive
            };
        }
    }
}
