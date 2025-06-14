namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectRenderOperationModel
    {
        public string State { get; set; } = string.Empty;
        public ProjectRenderOperationResultModel? Result { get; set; }
        public ErrorResponseModel? Error { get; set; }
    }
}
