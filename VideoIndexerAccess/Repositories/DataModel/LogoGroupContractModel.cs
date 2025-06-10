using System;
using System.Collections.Generic;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// LogoGroupContract JSONスキーマに対応するモデル
    /// </summary>
    public class LogoGroupContractModel
    {
        /// <summary>
        /// グループID（GUID）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 作成日時（ISO 8601形式）
        /// </summary>
        public DateTimeOffset CreationTime { get; set; }

        /// <summary>
        /// 最終更新日時（ISO 8601形式）
        /// </summary>
        public DateTimeOffset LastUpdateTime { get; set; }

        /// <summary>
        /// 最終更新者（null許容）
        /// </summary>
        public string? LastUpdatedBy { get; set; }

        /// <summary>
        /// 作成者（null許容）
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// ロゴリンクのリスト（null許容）
        /// </summary>
        public List<LogoGroupLinkModel>? Logos { get; set; }

        /// <summary>
        /// グループ名（null許容）
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// グループの説明（null許容）
        /// </summary>
        public string? Description { get; set; }
    }
}