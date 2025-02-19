using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// プロジェクトのマイグレーション情報を表すレスポンスモデル。
    /// </summary>
    public class ApiProjectMigrationModel
    {
        public ApiProjectMigrationState status;
        public string? name;
        public string? details;
        public string? projectId;
    }
}
