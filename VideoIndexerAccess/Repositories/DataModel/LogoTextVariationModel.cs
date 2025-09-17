namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LogoTextVariationModel
    {
        public string Text { get; set; } = "";
        public bool CaseSensitive { get; set; }
        public DateTimeOffset CreationTime { get; set; }
        public string? CreatedBy { get; set; } = "";
    }
}
