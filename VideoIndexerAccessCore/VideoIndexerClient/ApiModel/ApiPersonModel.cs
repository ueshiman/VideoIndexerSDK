using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// API のレスポンスをマッピングするための Person クラス
    /// </summary>
    public class ApiPersonModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public ApiFaceModel? sampleFace { get; set; } // sampleFace を追加
        public int imageCount { get; set; }
        public double score { get; set; }
        public string? lastModified { get; set; }
        public string? lastModifierName { get; set; }
    }
}
