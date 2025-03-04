namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// API のレスポンスをマッピングするための ApiCustomPersonModel クラス
    /// </summary>
    public class ApiCustomPersonModel
    {
        /// <summary>
        /// Person Model の ID (GUID 形式)
        /// </summary>
        public string? id { get; set; }

        /// <summary>
        /// Person Model の名前
        /// </summary>
        public string? name { get; set; }

        /// <summary>
        /// デフォルトの Person Model であるかどうか
        /// </summary>
        public bool isDefault { get; set; }

        /// <summary>
        /// Person Model に登録されている Person の数
        /// </summary>
        public int personsCount { get; set; }

        /// <summary>
        /// Person の識別スコアのしきい値
        /// </summary>
        public decimal personIdentificationThreshold { get; set; }
    }
}
