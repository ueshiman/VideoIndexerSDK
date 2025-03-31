using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    // アカウントの移行ステータスを表すモデルクラス
    public class ApiAccountMigrationStatusModel
    {
        // 移行ステータス
        public int status { get; set; }

        // 移行進捗（パーセンテージ）
        public int progress { get; set; }

        // 移行が残っているビデオの数
        public int videosLeftToMigrate { get; set; }

        // 移行が完了したビデオの数
        public int videosMigrated { get; set; }

        // 移行に失敗したビデオの数
        public int videosFailedToMigrate { get; set; }

        // 移行対象のビデオの総数
        public int totalVideosToMigrate { get; set; }

        // 移行が残っているプロジェクトの数
        public int projectsLeftToMigrate { get; set; }

        // 移行が完了したプロジェクトの数
        public int projectsMigrated { get; set; }

        // 移行に失敗したプロジェクトの数
        public int projectsFailedToMigrate { get; set; }

        // 移行対象のプロジェクトの総数
        public int totalProjectsToMigrate { get; set; }

        // 詳細情報
        public string? details { get; set; }

        // 移行完了日
        public DateTime? migrationCompletedDate { get; set; }
    }
}
