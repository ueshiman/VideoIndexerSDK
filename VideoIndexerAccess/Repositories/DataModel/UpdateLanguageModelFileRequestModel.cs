using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UpdateLanguageModelFileRequestModel
    {
        public string ModelId { get; set; }
        public string FileId { get; set; }
        public string? FileName { get; set; }
        public bool? Enable { get; set; }
    }
}
