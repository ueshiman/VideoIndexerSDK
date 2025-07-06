using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoInsightsWidgetResponseModel
    {
        /// <summary>
        /// ウィジェット表示用のURL（リダイレクト先など）
        /// </summary>
        public string? WidgetUrl { get; set; }

        /// <summary>
        /// エラー時のタイプ（例：GENERAL, USER_NOT_ALLOWEDなど）
        /// </summary>
        public string? ErrorType { get; set; }

        /// <summary>
        /// エラーに関する詳細メッセージ
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// サーバーからのリクエストID（ログ調査などに利用）
        /// </summary>
        public string? RequestId { get; set; }
    }
}