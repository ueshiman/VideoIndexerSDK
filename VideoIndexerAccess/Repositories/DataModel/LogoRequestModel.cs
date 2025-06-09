using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoRequestModel
    {
        public string Name { get; set; } = "";
        public string WikipediaSearchTerm { get; set; } = "";
        public TextVariationModel[] TextVariations { get; set; } = [];
    }
}
