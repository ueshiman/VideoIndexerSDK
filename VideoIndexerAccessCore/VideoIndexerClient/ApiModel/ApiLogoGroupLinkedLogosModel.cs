using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiLogoGroupLinkedLogosModel
    {
        public Guid id { get; set; }

        public DateTime creationTime { get; set; }

        public DateTime lastUpdateTime { get; set; }

        public string? lastUpdatedBy { get; set; }

        public string? createdBy { get; set; }

        public string? name { get; set; }

        public string? wikipediaSearchTerm { get; set; }

        public List<ApiLogoTextVariationModel>? textVariations { get; set; }
    }
}
