using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupRequestMapper : ILogoGroupRequestMapper
    {
        private readonly ILogoGroupLinkMapper _logoGroupLinkMapper;

        public LogoGroupRequestMapper(ILogoGroupLinkMapper logoGroupLinkMapper)
        {
            _logoGroupLinkMapper = logoGroupLinkMapper;
        }

        public LogoGroupRequestModel MapFrom(ApiLogoGroupRequestModel model)
        {
            return new LogoGroupRequestModel
            {
                Logos = model.logos.Select(_logoGroupLinkMapper.MapFrom).ToArray(),
                Name = model.name,
                Description = model.description,
            };
        }
        
        public ApiLogoGroupRequestModel MapToApiLogoGroupRequestModel(LogoGroupRequestModel model)
        {
            return new ApiLogoGroupRequestModel
            {
                logos = model.Logos.Select(_logoGroupLinkMapper.MapToApiLogoGroupLinkModel).ToArray(),
                name = model.Name,
                description = model.Description
            };
        }
    }
}
