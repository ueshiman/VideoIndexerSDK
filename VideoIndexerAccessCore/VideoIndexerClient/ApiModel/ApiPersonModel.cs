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
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public ApiFaceModel? SampleFace { get; set; } // sampleFace を追加
        public int ImageCount { get; set; }
        public double Score { get; set; }
        public string? LastModified { get; set; }
        public string? LastModifierName { get; set; }
    }
}
