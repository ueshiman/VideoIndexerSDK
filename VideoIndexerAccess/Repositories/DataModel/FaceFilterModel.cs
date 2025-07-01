namespace VideoIndexerAccess.Repositories.DataModel
{
    public class FaceFilterModel
    {
        public List<int> Ids { get; set; } = new List<int>();
        public string? Scope { get; set; }
    }
}
