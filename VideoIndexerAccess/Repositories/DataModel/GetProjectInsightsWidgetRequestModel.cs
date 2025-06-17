using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class GetProjectInsightsWidgetRequestModel
    {
        public string ProjectId { get; set; }
        public string? WidgetType { get; set; }
    }
}
