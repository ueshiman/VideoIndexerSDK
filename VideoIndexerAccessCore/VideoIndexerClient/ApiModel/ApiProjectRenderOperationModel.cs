using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectRenderOperationModel
    {
        public string state { get; set; } = string.Empty;
        public ApiProjectRenderOperationResultModel? result { get; set; }
        public ApiErrorResponseModel? error { get; set; }
    }
}
