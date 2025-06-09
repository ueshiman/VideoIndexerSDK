using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LanguageModelRequestModel
    {
        public string ModelId { get; set; }
        public string? ModelName { get; set; }
        public bool? Enable { get; set; }
        public Dictionary<string, string>? FileUrls { get; set; }
        public Dictionary<string, string>? FilePaths { get; set; }
    }
}
