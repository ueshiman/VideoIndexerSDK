namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectRenderResponseModel
    {
        public string State { get; set; } = "";
        public ProjectRenderResultModel? Result { get; set; }
        public ErrorResponseModel? Error { get; set; }
    }
}
