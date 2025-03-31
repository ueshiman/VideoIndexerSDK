using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴグループ作成リクエストモデル
    /// </summary>
    public class ApiLogoGroupRequestModel
    {
        /// <summary>
        /// ロゴのリスト
        /// </summary>
        public ApiLogoGroupLinkModel[] logos { get; set; } = Array.Empty<ApiLogoGroupLinkModel>();

        /// <summary>
        /// ロゴグループの名前
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// ロゴグループの説明
        /// </summary>
        public string description { get; set; } = "";
    }
}
