namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// プロジェクト一覧取得リクエストモデル。
    /// </summary>
    public class ProjectsRequestModel
    {
        /// <summary>
        /// 指定された日付以降に作成されたプロジェクトをフィルタリング。
        /// </summary>
        public string? CreatedAfter { get; set; }

        /// <summary>
        /// 指定された日付以前に作成されたプロジェクトをフィルタリング。
        /// </summary>
        public string? CreatedBefore { get; set; }

        /// <summary>
        /// 取得するページサイズ。
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// スキップするレコード数。
        /// </summary>
        public int? Skip { get; set; }
    }
}
