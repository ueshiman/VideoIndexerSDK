using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectSearchResultModel
    {
        public List<ApiProjectSearchResultItemModel>? results { get; set; }
        public ApiPagingInfoModel? nextPage { get; set; }
    }
}
