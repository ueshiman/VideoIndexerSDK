using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectUpdateRequestModel
    {
        public required string ProjectId { get; set; }

        /// <summary>
        /// プロジェクトの名前。
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// プロジェクトに含まれる動画とその再生範囲。
        /// </summary>
        public VideoTimeRangeModel[] VideosRanges { get; set; } = [];

        /// <summary>
        /// プロジェクトが検索可能かどうか。
        /// </summary>
        public bool? IsSearchable { get; set; }
    }
}
