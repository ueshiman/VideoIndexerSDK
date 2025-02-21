using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴ作成リクエストモデル
    /// </summary>
    public class ApiLogoRequestModel
    {
        public string name { get; set; } = "";
        public string wikipediaSearchTerm { get; set; } = "";
        public ApiTextVariationModel[] textVariations { get; set; } = Array.Empty<ApiTextVariationModel>();
    }
}
