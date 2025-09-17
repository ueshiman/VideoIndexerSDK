using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupUpdateRequestMapper : ILogoGroupUpdateRequestMapper
    {
        private readonly ILogoGroupLinkMapper _logoGroupLinkMapper;

        public LogoGroupUpdateRequestMapper(ILogoGroupLinkMapper logoGroupLinkMapper)
        {
            _logoGroupLinkMapper = logoGroupLinkMapper;
        }

        public LogoGroupUpdateRequestModel MapFrom(ApiLogoGroupUpdateRequestModel apiModel)
        {
            return new LogoGroupUpdateRequestModel
            {
                Name = apiModel.name,
                Description = apiModel.description,
                Logos = apiModel.logos.Select(_logoGroupLinkMapper.MapFrom).ToArray()
            };
        }

        public ApiLogoGroupUpdateRequestModel MapToApiApiLogoGroupUpdateRequestModel(LogoGroupUpdateRequestModel domainModel)
        {
            return new ApiLogoGroupUpdateRequestModel
            {
                name = domainModel.Name,
                description = domainModel.Description,
                logos = domainModel.Logos.Select(_logoGroupLinkMapper.MapToApiLogoGroupLinkModel).ToArray()
            };
        }
    }
}
