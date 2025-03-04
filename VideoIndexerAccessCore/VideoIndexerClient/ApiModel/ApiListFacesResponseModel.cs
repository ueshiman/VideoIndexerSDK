using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// API のレスポンス全体を表すモデル
    /// </summary>
    public class ApiListFacesResponseModel
    {
        public List<ApiFaceModel> Results { get; set; } = new List<ApiFaceModel>();
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public int TotalCount { get; set; }
    }
}
