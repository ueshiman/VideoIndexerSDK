namespace VideoIndexerAccess.Repositories.DataModel
{
    // アカウントの移行ステータスを表すモデルクラス
    public class AccountMigrationStatusModel
    {
        // 移行ステータス
        public int Status { get; set; }

        // 移行進捗（パーセンテージ）
        public int Progress { get; set; }

        // 移行が残っているビデオの数
        public int VideosLeftToMigrate { get; set; }

        // 移行が完了したビデオの数
        public int VideosMigrated { get; set; }

        // 移行に失敗したビデオの数
        public int VideosFailedToMigrate { get; set; }

        // 移行対象のビデオの総数
        public int TotalVideosToMigrate { get; set; }

        // 移行が残っているプロジェクトの数
        public int ProjectsLeftToMigrate { get; set; }

        // 移行が完了したプロジェクトの数
        public int ProjectsMigrated { get; set; }

        // 移行に失敗したプロジェクトの数
        public int ProjectsFailedToMigrate { get; set; }

        // 移行対象のプロジェクトの総数
        public int TotalProjectsToMigrate { get; set; }

        // 詳細情報
        public string? Details { get; set; }

        // 移行完了日
        public DateTime? MigrationCompletedDate { get; set; }
    }
}
