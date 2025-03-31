using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectRenderOperationResultModel
    {
        public List<ApiVideoTimeRangeModel> videoRanges { get; set; } = new();
    }
}
