using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴグループのレスポンスモデル
    /// </summary>
    public class ApiLogoGroupContractModel
    {
        public string id { get; set; } = "";
        public DateTimeOffset creationTime { get; set; }
        public DateTimeOffset lastUpdateTime { get; set; }
        public string lastUpdatedBy { get; set; } = "";
        public string createdBy { get; set; } = "";
        public ApiLogoGroupLinkModel[] logos { get; set; } = [];
        public string name { get; set; } = "";
        public string description { get; set; } = "";
    }
}
