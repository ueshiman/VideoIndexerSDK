namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectMigrationModel
    {
        public ProjectMigrationState Status { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? ProjectId { get; set; }
    }
}
