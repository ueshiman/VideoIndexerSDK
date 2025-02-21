using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴ作成レスポンスモデル
    /// </summary>
    public class ApiLogoResponseModel
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string createdBy { get; set; } = "";
        public string wikipediaSearchTerm { get; set; } = "";
    }
}
