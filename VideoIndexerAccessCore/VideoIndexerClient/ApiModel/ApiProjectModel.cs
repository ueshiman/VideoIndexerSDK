using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectModel
    {
        public string accountId { get; set; } = string.Empty;
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string created { get; set; } = string.Empty;
        public string lastModified { get; set; } = string.Empty;
        public bool isSearchable { get; set; }
    }
}
