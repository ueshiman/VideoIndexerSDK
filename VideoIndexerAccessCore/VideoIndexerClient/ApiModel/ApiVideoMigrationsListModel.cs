using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ビデオ移行のリストレスポンスを表すクラス。
    /// </summary>
    public class ApiVideoMigrationsListModel
    {
        public List<ApiVideoMigrationModel> results { get; set; }
        public ApiPagingInfoModel nextPage { get; set; }
    }
}
