using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// プロジェクト更新レスポンスモデル
    /// </summary>
    /// <summary>
    /// プロジェクト更新レスポンスモデル。
    /// </summary>
    public class ApiProjectUpdateResponseModel
    {
        /// <summary>
        /// アカウントID。
        /// </summary>
        public string accountId { get; set; } = "";

        /// <summary>
        /// プロジェクトのID。
        /// </summary>
        public string id { get; set; } = "";

        /// <summary>
        /// プロジェクトの名前。
        /// </summary>
        public string name { get; set; } = "";

        /// <summary>
        /// プロジェクトの作成日時。
        /// </summary>
        public string created { get; set; } = "";

        /// <summary>
        /// 最後に更新された日時。
        /// </summary>
        public string lastModified { get; set; } = "";

        /// <summary>
        /// プロジェクトを更新したユーザーの名前。
        /// </summary>
        public string userName { get; set; } = "";

        /// <summary>
        /// 動画の総再生時間（秒単位）。
        /// </summary>
        public int durationInSeconds { get; set; }

        /// <summary>
        /// プロジェクトのサムネイルとなる動画のID。
        /// </summary>
        public string thumbnailVideoId { get; set; } = "";

        /// <summary>
        /// サムネイル画像のID。
        /// </summary>
        public string thumbnailId { get; set; } = "";
    }
}
