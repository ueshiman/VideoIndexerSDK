using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoContractMapper : ILogoContractMapper
    {
        private readonly ILogoTextVariationMapper _logoTextVariationMapper;

        public LogoContractMapper(ILogoTextVariationMapper logoTextVariationMapper)
        {
            _logoTextVariationMapper = logoTextVariationMapper;
        }

        public LogoContractModel MapFrom(ApiLogoContractModel model)
        {
            return new LogoContractModel
            {
                Id = model.id,
                CreationTime = model.creationTime,
                LastUpdateTime = model.lastUpdateTime,
                LastUpdatedBy = model.lastUpdatedBy,
                CreatedBy = model.createdBy,
                Name = model.name,
                WikipediaSearchTerm = model.wikipediaSearchTerm,
                TextVariations = model.textVariations?.Select(textVariation => _logoTextVariationMapper.MapFrom(textVariation)).ToList() ?? new List<LogoTextVariationModel>()
            };
        }

        public ApiLogoContractModel MapToApiLogoContractModel(LogoContractModel model)
        {
            return new ApiLogoContractModel
            {
                id = model.Id,
                creationTime = model.CreationTime,
                lastUpdateTime = model.LastUpdateTime,
                lastUpdatedBy = model.LastUpdatedBy,
                createdBy = model.CreatedBy,
                name = model.Name,
                wikipediaSearchTerm = model.WikipediaSearchTerm,
                textVariations = model.TextVariations?.Select(textVariation => _logoTextVariationMapper.MapToApiLogoTextVariationModel(textVariation)).ToList() ?? new List<ApiLogoTextVariationModel>()
            };
        }
    }
}
