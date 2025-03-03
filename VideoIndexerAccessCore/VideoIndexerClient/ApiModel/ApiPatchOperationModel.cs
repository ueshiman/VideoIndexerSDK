using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// JSON Patch の各操作を表すクラス。
    /// </summary>
    public class ApiPatchOperationModel
    {
        /// <summary>
        /// JSON Patch の操作の種類 (add, remove, replace など)。
        /// </summary>
        public string op { get; set; } = string.Empty;

        /// <summary>
        /// 操作対象の JSON パス。
        /// </summary>
        public string path { get; set; } = string.Empty;

        /// <summary>
        /// 設定する値 (適用する場合のみ)。
        /// </summary>
        public AipPatchValueModel? value { get; set; }
    }
}
