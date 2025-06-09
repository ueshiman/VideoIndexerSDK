namespace VideoIndexerAccess.Repositories.DataModel
{
    public class LanguageModelEditModel
    {
        public int Id { get; set; }
        public string VideoId { get; set; } = string.Empty;
        public int LineId { get; set; }
        public string CreateDate { get; set; } = string.Empty;
        public string OriginalValue { get; set; } = string.Empty;
        public string CurrentValue { get; set; } = string.Empty;
        public int LinguisticTrainingDataGroupsId { get; set; }
        public string VideoName { get; set; } = string.Empty;
    }
}
