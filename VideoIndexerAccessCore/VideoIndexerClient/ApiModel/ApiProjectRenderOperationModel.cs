namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectRenderOperationModel
    {
        public string state { get; set; } = string.Empty;
        public ApiProjectRenderOperationResultModel? result { get; set; }
        public ApiErrorResponseModel? error { get; set; }
    }
}
