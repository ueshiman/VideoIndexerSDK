using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiErrorResponseModel
    {
        public string errorType { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }
}
