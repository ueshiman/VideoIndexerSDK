namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectModel
    {
        public string AccountId { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Created { get; set; } = string.Empty;
        public string LastModified { get; set; } = string.Empty;
        public bool IsSearchable { get; set; }
    }
}
