using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoUpdateRequestMapper : ILogoUpdateRequestMapper
    {
        private readonly ILogoTextVariationMapper _logoTextVariationMapper;

        public LogoUpdateRequestMapper(ILogoTextVariationMapper logoTextVariationMapper)
        {
            _logoTextVariationMapper = logoTextVariationMapper;
        }

        public LogoUpdateRequestModel MapFrom(ApiLogoUpdateRequestModel apiModel)
        {
            if (apiModel == null) throw new ArgumentNullException(nameof(apiModel));

            return new LogoUpdateRequestModel
            {
                Name = apiModel.name,
                WikipediaSearchTerm = apiModel.wikipediaSearchTerm,
                TextVariations = apiModel.textVariations.Select(_logoTextVariationMapper.MapFrom).ToArray()
            };
        }

        public ApiLogoUpdateRequestModel MapToApiLogoUpdateRequestModel(LogoUpdateRequestModel domainModel)
        {
            if (domainModel == null) throw new ArgumentNullException(nameof(domainModel));

            return new ApiLogoUpdateRequestModel
            {
                name = domainModel.Name,
                wikipediaSearchTerm = domainModel.WikipediaSearchTerm,
                textVariations = domainModel.TextVariations.Select(_logoTextVariationMapper.MapToApiLogoTextVariationModel).ToArray()
            };
        }
    }
}
