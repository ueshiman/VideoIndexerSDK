using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetVideoStreamingUrlRequestModel
    {
        public required string VideoId { get; set; }
        public bool? UseProxy { get; set; }
        public string? UrlFormat { get; set; }
        public int? TokenLifetimeInMinutes { get; set; }
    }
}
