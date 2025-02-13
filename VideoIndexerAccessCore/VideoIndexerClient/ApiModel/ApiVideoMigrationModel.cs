namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ビデオ移行のレスポンスを表すクラス。
    /// </summary>
    public class ApiVideoMigrationModel
    {
        public ApiVideoMigrationState status { get; set; }
        public string? name { get; set; }
        public string? details { get; set; }
        public string? videoId { get; set; }
    }
}
