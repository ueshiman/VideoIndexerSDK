using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoContractModel
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public DateTimeOffset LastUpdateTime { get; set; }

        public string? LastUpdatedBy { get; set; }

        public string? CreatedBy { get; set; }

        public string? Name { get; set; }

        public string? WikipediaSearchTerm { get; set; }

        public List<LogoTextVariationModel> TextVariations { get; set; } = new();
    }
}
