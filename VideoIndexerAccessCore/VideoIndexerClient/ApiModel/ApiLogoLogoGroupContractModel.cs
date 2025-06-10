using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiLogoLogoGroupContractModel
    {
        /// <summary>
        /// グループID
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// グループ名
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// グループの説明
        /// </summary>
        public string Description { get; set; } = "";

        /// <summary>
        /// 作成者
        /// </summary>
        public string CreatedBy { get; set; } = "";
    }
}
