using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoGroupRequestModel
    {
        /// <summary>
        /// ロゴのリスト
        /// </summary>
        public LogoGroupLinkModel[] Logos { get; set; } = Array.Empty<LogoGroupLinkModel>();

        /// <summary>
        /// ロゴグループの名前
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// ロゴグループの説明
        /// </summary>
        public string Description { get; set; } = "";
    }
}
