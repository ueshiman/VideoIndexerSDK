using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoGroupUpdateRequestModel
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public LogoGroupLinkModel[] Logos { get; set; } = Array.Empty<LogoGroupLinkModel>();
    }
}
