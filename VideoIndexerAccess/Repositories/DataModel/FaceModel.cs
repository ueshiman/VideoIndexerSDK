namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// API のレスポンスで含まれる SampleFace 情報を表すクラス
    /// </summary>
    public class FaceModel
    {
        public Guid? Id { get; set; }
        public string? State { get; set; } // 例: "Ok"
        public string? SourceType { get; set; } // 例: "UploadedPicture"
        public string? SourceVideoId { get; set; }
    }
}
