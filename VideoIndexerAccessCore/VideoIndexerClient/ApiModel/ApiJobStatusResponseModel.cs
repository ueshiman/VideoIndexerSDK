﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ジョブのステータス情報を保持するモデルクラス
    /// </summary>
    public class ApiJobStatusResponseModel
    {
        /// <summary>
        /// ジョブの作成日時
        /// </summary>
        public string? CreationTime { get; set; }

        /// <summary>
        /// ジョブの最終更新日時
        /// </summary>
        public string? LastUpdateTime { get; set; }

        /// <summary>
        /// ジョブの進行状況（0-100%）
        /// </summary>
        public int? Progress { get; set; }

        /// <summary>
        /// ジョブの種類
        /// </summary>
        public string? JobType { get; set; }

        /// <summary>
        /// ジョブの現在の状態
        /// </summary>
        public int? State { get; set; }
    }

}
