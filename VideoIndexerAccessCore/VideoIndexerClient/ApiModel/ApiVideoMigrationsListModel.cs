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
        public List<ApiVideoMigrationModel> results { get; set; } = new List<ApiVideoMigrationModel>();
        public ApiPagingInfoModel? nextPage { get; set; }
    }
}
