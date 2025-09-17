namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// ロゴ情報の更新リクエストモデル
    /// </summary>
    public class LogoUpdateRequestModel
    {
        public string Name { get; set; } = "";
        public string WikipediaSearchTerm { get; set; } = "";
        public LogoTextVariationModel[] TextVariations { get; set; } = Array.Empty<LogoTextVariationModel>();
    }
}
