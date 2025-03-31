using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectRenderResponseModel
    {
        public string state { get; set; } = "";
        public ApiProjectRenderResultModel? result { get; set; }
        public ApiErrorResponseModel? error { get; set; }
    }
}
