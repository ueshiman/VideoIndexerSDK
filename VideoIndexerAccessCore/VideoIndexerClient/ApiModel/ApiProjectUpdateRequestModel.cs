using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// プロジェクト更新リクエストモデル。
    /// </summary>
    public class ApiProjectUpdateRequestModel
    {
        /// <summary>
        /// プロジェクトの名前。
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// プロジェクトに含まれる動画とその再生範囲。
        /// </summary>
        public ApiVideoTimeRangeModel[] videosRanges { get; set; } = Array.Empty<ApiVideoTimeRangeModel>();

        /// <summary>
        /// プロジェクトが検索可能かどうか。
        /// </summary>
        public bool? isSearchable { get; set; }
    }

}
