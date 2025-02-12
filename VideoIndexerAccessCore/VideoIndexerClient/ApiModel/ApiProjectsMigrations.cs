namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

    public class ApiProjectsMigrations
    {
        public List<ApiProjectMigrationModel> results { get; set; } = new List<ApiProjectMigrationModel>();
        public ApiPagingInfoModel? nextPage { get; set; }
    }

