namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PromptContentRequestModel
    {
        public required string VideoId { get; set; }
        public string? ModelName { get; set; }
        public string? PromptStyle { get; set; }
    }
}
