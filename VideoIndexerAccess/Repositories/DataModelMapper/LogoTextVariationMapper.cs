using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoTextVariationMapper : ILogoTextVariationMapper
    {
        public LogoTextVariationModel MapFrom(ApiLogoTextVariationModel model)
        {
            return new LogoTextVariationModel
            {
                Text = model.text,
                CaseSensitive = model.caseSensitive,
                CreationTime = model.creationTime,
                CreatedBy = model.createdBy ?? string.Empty
            };
        }
        public ApiLogoTextVariationModel MapToApiTextVariationModel(LogoTextVariationModel model)
        {
            return new ApiLogoTextVariationModel
            {
                text = model.Text,
                caseSensitive = model.CaseSensitive,
                creationTime = model.CreationTime,
                createdBy = model.CreatedBy,
            };
        }
    }
}
