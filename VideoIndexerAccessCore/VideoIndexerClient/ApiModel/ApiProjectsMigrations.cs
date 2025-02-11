namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

    public class ApiProjectsMigrations
    {
        public List<ApiProjectMigrationModel> Results { get; set; } = new List<ApiProjectMigrationModel>();
        public ApiPagingInfoModel? NextPage { get; set; }
    }

