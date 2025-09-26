using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TextualSummarizationContractPageModel
    {
        public int? size { get; set; }
        public int? pageNumber { get; set; }
        public bool? done { get; set; }
        public List<TextualSummarizationJobContractModel>? items { get; set; }
    }
}
