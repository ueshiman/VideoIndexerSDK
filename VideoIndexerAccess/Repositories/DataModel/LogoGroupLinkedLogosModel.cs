using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoGroupLinkedLogosModel
    {
        public Guid Id { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public string? LastUpdatedBy { get; set; }

        public string? CreatedBy { get; set; }

        public string? Name { get; set; }

        public string? WikipediaSearchTerm { get; set; }

        public List<LogoTextVariationModel>? TextVariations { get; set; }
    }
}
