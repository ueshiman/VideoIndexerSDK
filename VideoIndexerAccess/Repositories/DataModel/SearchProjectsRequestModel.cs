namespace VideoIndexerAccess.Repositories.DataModel
{
    public class SearchProjectsRequestModel
    {
        /// <summary>
        /// フリーテキスト検索クエリ
        /// </summary>
        public string? Query { get; set; }

        /// <summary>
        /// ソース言語
        /// </summary>
        public string? SourceLanguage { get; set; }

        /// <summary>
        /// 取得するプロジェクトの最大件数
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// スキップするプロジェクトの件数 (ページネーション用)
        /// </summary>
        public int? Skip { get; set; }
    }
}
