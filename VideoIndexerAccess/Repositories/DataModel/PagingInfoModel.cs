using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PagingInfoModel
    {
        public int PageSize { get; set; }
        public int Skip { get; set; }
        public bool Done { get; set; }
        public int? TotalCount { get; set; }
    }
}
