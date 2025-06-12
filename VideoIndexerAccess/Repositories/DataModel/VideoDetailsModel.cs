using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoDetailsModel
    {
        public string? PublishedUrl { get; set; }
        public string? ViewToken { get; set; }
        public string? State { get; set; }
        public string? ProcessingProgress { get; set; }
        public string? FailureMessage { get; set; }
    }
}
