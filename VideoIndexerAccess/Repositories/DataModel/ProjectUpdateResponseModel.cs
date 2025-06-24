namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectUpdateResponseModel
    {
        /// <summary>
        /// アカウントID。
        /// </summary>
        public string AccountId { get; set; } = "";

        /// <summary>
        /// プロジェクトのID。
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// プロジェクトの名前。
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// プロジェクトの作成日時。
        /// </summary>
        public string Created { get; set; } = "";

        /// <summary>
        /// 最後に更新された日時。
        /// </summary>
        public string LastModified { get; set; } = "";

        /// <summary>
        /// プロジェクトを更新したユーザーの名前。
        /// </summary>
        public string UserName { get; set; } = "";

        /// <summary>
        /// 動画の総再生時間（秒単位）。
        /// </summary>
        public int DurationInSeconds { get; set; }

        /// <summary>
        /// プロジェクトのサムネイルとなる動画のID。
        /// </summary>
        public string ThumbnailVideoId { get; set; } = "";

        /// <summary>
        /// サムネイル画像のID。
        /// </summary>
        public string ThumbnailId { get; set; } = "";
    }
}
