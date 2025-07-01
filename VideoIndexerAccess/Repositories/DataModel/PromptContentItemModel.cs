namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PromptContentItemModel
    {
        public int Id { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
        public string? Content { get; set; }
        public List<string>? Frames { get; set; }
    }
}
