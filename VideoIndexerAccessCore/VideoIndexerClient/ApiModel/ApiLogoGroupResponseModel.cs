using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴグループ作成レスポンスモデル
    /// </summary>
    public class ApiLogoGroupResponseModel
    {
        /// <summary>
        /// グループID
        /// </summary>
        public string id { get; set; } = "";

        /// <summary>
        /// グループ名
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// グループの説明
        /// </summary>
        public string description { get; set; } = "";

        /// <summary>
        /// 作成者
        /// </summary>
        public string createdBy { get; set; } = "";
    }

}
