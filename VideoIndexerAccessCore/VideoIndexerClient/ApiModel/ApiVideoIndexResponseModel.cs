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
        public string? accountId { get; set; }
        public string? id { get; set; }
        public string? name { get; set; }
        public string? userName { get; set; }
        public string? created { get; set; }
        public bool? isOwned { get; set; }
        public bool? isEditable { get; set; }
        public bool? isBase { get; set; }
        public int? durationInSeconds { get; set; }
        public string? duration { get; set; }
        public List<ApiVideoDetailsModel>? videos { get; set; }
    }
}
