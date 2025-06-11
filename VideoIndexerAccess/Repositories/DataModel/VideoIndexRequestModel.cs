using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoIndexRequestModel
    {
        public string VideoId { get; set; }
        public string? Language { get; set; } = null;
        public bool? ReTranslate { get; set; } = null;
        public bool? IncludeStreamingUrls { get; set; } = null;
        public string? IncludedInsights { get; set; } = null;
        public string? ExcludedInsights { get; set; } = null; 
    }
}
