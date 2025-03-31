using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// APIレスポンスを格納するクラス。Insights Widget のURLやエラー情報を保持します。
    /// </summary>
    public class ApiVideoInsightsWidgetResponseModel
    {
        /// <summary>
        /// ウィジェット表示用のURL（リダイレクト先など）
        /// </summary>
        public string? widgetUrl { get; set; }

        /// <summary>
        /// エラー時のタイプ（例：GENERAL, USER_NOT_ALLOWEDなど）
        /// </summary>
        public string? errorType { get; set; }

        /// <summary>
        /// エラーに関する詳細メッセージ
        /// </summary>
        public string? message { get; set; }

        /// <summary>
        /// サーバーからのリクエストID（ログ調査などに利用）
        /// </summary>
        public string? requestId { get; set; }

        // 必要に応じてさらにプロパティ追加可能
    }
}
