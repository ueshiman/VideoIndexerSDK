namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// Person クラス
    /// </summary>
    public class PersonModel
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public FaceModel? SampleFace { get; set; } // sampleFace を追加
        public int ImageCount { get; set; }
        public double Score { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public string? LastModifierName { get; set; }
    }
}
