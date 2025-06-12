using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PatchOperationModel
    {
        /// <summary>
        /// JSON Patch の操作の種類 (add, remove, replace など)。
        /// </summary>
        public string Op { get; set; } = string.Empty;

        /// <summary>
        /// 操作対象の JSON パス。
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// 設定する値 (適用する場合のみ)。
        /// </summary>
        public AipPatchValueModel? Value { get; set; }
    }
}
