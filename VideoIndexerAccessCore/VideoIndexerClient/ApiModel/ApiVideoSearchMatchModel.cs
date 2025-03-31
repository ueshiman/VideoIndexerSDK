using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiVideoSearchMatchModel
    {
        public string startTime { get; set; } = "";
        public string type { get; set; } = "";
        public string subType { get; set; } = "";
        public string text { get; set; } = "";
        public string exactText { get; set; } = "";
    }
}
