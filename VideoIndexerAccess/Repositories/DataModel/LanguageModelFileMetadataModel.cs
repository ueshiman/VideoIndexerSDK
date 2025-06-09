namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LanguageModelFileMetadataModel
    {
        /// <summary>ファイルID（GUID）</summary>
        public string? Id { get; set; }

        /// <summary>ファイル名</summary>
        public string? Name { get; set; }

        /// <summary>ファイルが有効かどうか</summary>
        public bool Enable { get; set; }

        /// <summary>作成者</summary>
        public string? Creator { get; set; }

        /// <summary>ファイル作成日時</summary>
        public DateTime? CreationTime { get; set; }
    }
}
