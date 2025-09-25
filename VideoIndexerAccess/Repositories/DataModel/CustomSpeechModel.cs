using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{

    /// <summary>
    /// 取得したカスタムスピーチモデルを表します。
    /// </summary>
    public class CustomSpeechModel
    {
        /// <summary>
        /// スピーチモデルの ID（GUID）
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルのプロパティ
        /// </summary>
        public ApiCustomSpeechModelPropertiesModel? Properties { get; set; }

        /// <summary>
        /// スピーチモデルの表示名
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルの説明
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルのロケール（言語）
        /// </summary>
        public string Locale { get; set; } = string.Empty;

        /// <summary>
        /// スピーチモデルに関連付けられたデータセット ID のリスト
        /// </summary>
        public List<string> Datasets { get; set; } = new();

        /// <summary>
        /// スピーチモデルのステータス
        /// </summary>
        public ApiSpeechObjectState Status { get; set; }

        /// <summary>
        /// 最後にアクションが行われた日時
        /// </summary>
        public DateTime? LastActionDateTime { get; set; }

        /// <summary>
        /// 作成日時
        /// </summary>
        public DateTime? CreatedDateTime { get; set; }

        /// <summary>
        /// カスタムプロパティ（任意）
        /// </summary>
        public Dictionary<string, string>? CustomProperties { get; set; }
    }
}
