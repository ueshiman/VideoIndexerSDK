namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiPromptContentContractModel
    {
        public string? partition { get; set; }
        public string? name { get; set; }
        public List<ApiPromptContentItemModel>? sections { get; set; }
    }
}
