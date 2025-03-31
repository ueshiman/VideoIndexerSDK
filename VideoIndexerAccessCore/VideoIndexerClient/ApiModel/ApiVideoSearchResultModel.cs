using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// 動画一覧の結果を表すレスポンスモデル
    /// </summary>
    public class ApiVideoSearchResultModel
    {
        public List<ApiVideoSearchResultItemModel>? results { get; set; }
        public ApiPagingInfoModel? nextPage { get; set; }
    }
}
