using System.Text.Json.Serialization;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiCustomSpeechModelPropertiesModel
    {
        /// <summary>
        /// モデルの廃止予定日
        /// </summary>
        public ApiCustomSpeechModelDeprecationDatesModel? deprecationDates { get; set; }

        /// <summary>
        /// エラー情報（存在する場合）
        /// </summary>
        public string? error { get; set; }
    }
}
