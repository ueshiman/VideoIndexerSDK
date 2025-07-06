using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoPlayerWidgetResponseModel
    {
        /// <summary>
        /// プレイヤーウィジェットの表示URL（リダイレクト先）
        /// </summary>
        public string? PlayerWidgetUrl { get; set; }

        /// <summary>
        /// エラーの種類（発生した場合）
        /// </summary>
        public string? ErrorType { get; set; }

        /// <summary>
        /// エラーメッセージ（発生した場合）
        /// </summary>
        public string? Message { get; set; }
    }
}
