using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupLinkMapper : ILogoGroupLinkMapper
    {
        public LogoGroupLinkModel MapFrom(ApiLogoGroupLinkModel model)
        {
            return new LogoGroupLinkModel
            {
                LogoId = model.logoId,
            };
        }
        public ApiLogoGroupLinkModel MapToApiLogoGroupLinkModel(LogoGroupLinkModel model)
        {
            return new ApiLogoGroupLinkModel
            {
                logoId = model.LogoId,
            };
        }
    }
}
