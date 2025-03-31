namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// APIの人物リストレスポンスモデル
    /// </summary>
    public class ApiListPersonsResponseModel
    {
        public List<ApiPersonModel> Results { get; set; }
        public int PageSize { get; set; }
        public int Skip { get; set; }
    }
}
