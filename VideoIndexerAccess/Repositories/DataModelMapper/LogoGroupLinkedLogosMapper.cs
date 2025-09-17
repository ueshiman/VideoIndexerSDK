using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupLinkedLogosMapper : ILogoGroupLinkedLogosMapper
    {
        private readonly ILogoTextVariationMapper _logoTextVariationMapper;

        public LogoGroupLinkedLogosMapper(ILogoTextVariationMapper logoTextVariationMapper)
        {
            _logoTextVariationMapper = logoTextVariationMapper;
        }

        public LogoGroupLinkedLogosModel MapFrom(ApiLogoGroupLinkedLogosModel apiModel)
        {
            return new LogoGroupLinkedLogosModel
            {
                Id = apiModel.id,
                CreationTime = apiModel.creationTime,
                LastUpdateTime = apiModel.lastUpdateTime,
                LastUpdatedBy = apiModel.lastUpdatedBy,
                CreatedBy = apiModel.createdBy,
                Name = apiModel.name,
                WikipediaSearchTerm = apiModel.wikipediaSearchTerm,
                TextVariations = apiModel.textVariations?.Select(_logoTextVariationMapper.MapFrom)?.ToList()
            };
        }

        public ApiLogoGroupLinkedLogosModel MapToApiLogoGroupLinkedLogosModel(LogoGroupLinkedLogosModel model)
        {
            return new ApiLogoGroupLinkedLogosModel
            {
                id = model.Id,
                creationTime = model.CreationTime,
                lastUpdateTime = model.LastUpdateTime,
                lastUpdatedBy = model.LastUpdatedBy,
                createdBy = model.CreatedBy,
                name = model.Name,
                wikipediaSearchTerm = model.WikipediaSearchTerm,
                textVariations = model.TextVariations?.Select(_logoTextVariationMapper.MapToApiLogoTextVariationModel)?.ToList()
            };
        }
    }
}
