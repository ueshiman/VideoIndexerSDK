namespace VideoIndexerAccess.Repositories.DataModel
{
    public class PromptContentContractModel
    {
        public string? Partition { get; set; }
        public string? Name { get; set; }
        public List<PromptContentItemModel>? Sections { get; set; }
    }
}
