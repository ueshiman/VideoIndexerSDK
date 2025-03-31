namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// Video Player Widget API のレスポンスモデル
    /// </summary>
    public class ApiVideoPlayerWidgetResponseModel
    {
        /// <summary>
        /// プレイヤーウィジェットの表示URL（リダイレクト先）
        /// </summary>
        public string? playerWidgetUrl { get; set; }

        /// <summary>
        /// エラーの種類（発生した場合）
        /// </summary>
        public string? errorType { get; set; }

        /// <summary>
        /// エラーメッセージ（発生した場合）
        /// </summary>
        public string? message { get; set; }
    }

}
