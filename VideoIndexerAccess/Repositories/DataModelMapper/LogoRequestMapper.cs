using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoRequestMapper : ILogoRequestMapper
    {
        private readonly ITextVariationMapper _textVariationMapper;

        public LogoRequestMapper(ITextVariationMapper textVariationMapper)
        {
            _textVariationMapper = textVariationMapper;
        }

        public LogoRequestModel MapFrom(ApiLogoRequestModel apiModel)
        {
            return new LogoRequestModel
            {
                Name = apiModel.name,
                WikipediaSearchTerm = apiModel.wikipediaSearchTerm,
                TextVariations = apiModel.textVariations.Select(_textVariationMapper.MapFrom).ToArray()
            };
        }


        public ApiLogoRequestModel MapToApiLogoRequestModel(LogoRequestModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Model cannot be null");
            }

            return new ApiLogoRequestModel
            {
                name = model.Name,
                wikipediaSearchTerm = model.WikipediaSearchTerm,
                textVariations = model.TextVariations.Select(_textVariationMapper.MapToApiTextVariationModel).ToArray()
            };
        }
    }
}
