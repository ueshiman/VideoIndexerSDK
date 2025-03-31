namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{ public class ApiTextualSummarizationContractPageModel
    {
        public int? size { get; set; }
        public int? pageNumber { get; set; }
        public bool? done { get; set; }
        public List<ApiAOAITextualSummarizationJobContractModel>? items { get; set; }
    }
}
