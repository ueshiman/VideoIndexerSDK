using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴグループ情報の更新リクエストモデル
    /// </summary>
    public class ApiLogoGroupUpdateRequestModel
    {
        public string name { get; set; } = "";
        public string description { get; set; } = "";
        public ApiLogoGroupLinkModel[] logos { get; set; } = Array.Empty<ApiLogoGroupLinkModel>();
    }
}
