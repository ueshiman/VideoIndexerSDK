namespace VideoIndexerAccess.Repositories.DataModel
{
    public class RenderProjectRequestModel
    {
        public required string ProjectId { get; set; }
        public bool SendCompletionEmail { get; set; } = false;
    }
}
