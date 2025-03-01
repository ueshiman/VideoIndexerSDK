using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// 動画インデックスの更新後のレスポンスデータを格納するクラス。
    /// </summary>
    public class ApiVideoIndexResponseModel
    {
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public string? Created { get; set; }
        public bool? IsOwned { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsBase { get; set; }
        public int? DurationInSeconds { get; set; }
        public string? Duration { get; set; }
        public List<ApiVideoDetailsModel>? Videos { get; set; }
    }
}
