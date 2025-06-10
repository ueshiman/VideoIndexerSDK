using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;
using ApiLogoContractModel = VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiLogoContractModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoResponseMapper : ILogoResponseMapper
    {
        private readonly ILogoTextVariationMapper _logoTextVariationMapper;

        public LogoResponseMapper(ILogoTextVariationMapper logoTextVariationMapper)
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
                TextVariations = model.textVariations.Select(_logoTextVariationMapper.MapFrom).ToList()

            };
        }
        public ApiLogoContractModel MapToApiLogoResponseModel(LogoContractModel model)
        {
            return new ApiLogoContractModel
            {
                id = model.Id,
                name = model.Name,
                createdBy = model.CreatedBy,
                wikipediaSearchTerm = model.WikipediaSearchTerm
            };
        }
    }
}
