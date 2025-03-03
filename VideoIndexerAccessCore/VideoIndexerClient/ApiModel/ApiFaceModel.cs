using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// API のレスポンスで含まれる SampleFace 情報を表すクラス
    /// </summary>
    public class ApiFaceModel
    {
        public string? Id { get; set; }
        public string? State { get; set; } // 例: "Ok"
        public string? SourceType { get; set; } // 例: "UploadedPicture"
        public string? SourceVideoId { get; set; }
    }
}
