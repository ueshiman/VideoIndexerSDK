using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiVideoTimeRangeModel
    {
        public string videoId { get; set; } = string.Empty;
        public ApiTimeRangeModel range { get; set; } = new();
    }
}
