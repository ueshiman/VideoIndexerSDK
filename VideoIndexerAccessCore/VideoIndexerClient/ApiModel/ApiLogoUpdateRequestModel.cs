using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴ情報の更新リクエストモデル
    /// </summary>
    public class ApiLogoUpdateRequestModel
    {
        public string name { get; set; } = "";
        public string wikipediaSearchTerm { get; set; } = "";
        public ApiLogoTextVariationModel[] textVariations { get; set; } = Array.Empty<ApiLogoTextVariationModel>();
    }

}
