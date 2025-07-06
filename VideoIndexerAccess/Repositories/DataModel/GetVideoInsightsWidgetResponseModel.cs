using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetVideoInsightsWidgetResponseModel
    {
        public required string VideoId { get; set; }
        public string? WidgetType { get; set; }
        public bool? AllowEdit { get; set; }
    }
}
