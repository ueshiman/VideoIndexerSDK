using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupContractMapper : ILogoGroupContractMapper
    {
        private readonly ILogoGroupLinkMapper _logoGroupLinkMapper;

        public LogoGroupContractMapper(ILogoGroupLinkMapper logoGroupLinkMapper)
        {
            _logoGroupLinkMapper = logoGroupLinkMapper;
        }

        public LogoGroupContractModel MapFrom(ApiLogoGroupContractModel model)
        {
            return new LogoGroupContractModel
            {
                Id = model.id,
                CreationTime = model.creationTime,
                LastUpdateTime = model.lastUpdateTime,
                LastUpdatedBy = model.lastUpdatedBy,
                CreatedBy = model.createdBy,
                Logos = model.logos.Select(_logoGroupLinkMapper.MapFrom).ToList(),
                Name = model.name,
                Description = model.description,
            };
        }

        public ApiLogoGroupContractModel MapToApiLogoGroupContractModel(LogoGroupContractModel model)
        {
            return new ApiLogoGroupContractModel
            {
                id = model.Id,
                creationTime = model.CreationTime,
                lastUpdateTime = model.LastUpdateTime,
                lastUpdatedBy = model.LastUpdatedBy ?? string.Empty,
                createdBy = model.CreatedBy ?? string.Empty,
                logos = model.Logos?.Select(_logoGroupLinkMapper.MapToApiLogoGroupLinkModel).ToArray() ?? [],
                name = model.Name ?? string.Empty,
                description = model.Description ?? string.Empty
            };
        }
    }
}
