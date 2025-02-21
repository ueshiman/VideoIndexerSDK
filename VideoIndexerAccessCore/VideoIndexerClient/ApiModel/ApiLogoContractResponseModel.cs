using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴ情報のレスポンスモデル
    /// </summary>
    public class ApiLogoContractResponseModel
    {
        public string id { get; set; } = "";
        public string creationTime { get; set; } = "";
        public string lastUpdateTime { get; set; } = "";
        public string lastUpdatedBy { get; set; } = "";
        public string createdBy { get; set; } = "";
        public string name { get; set; } = "";
        public string wikipediaSearchTerm { get; set; } = "";
        public ApiLogoTextVariationModel[] textVariations { get; set; } = Array.Empty<ApiLogoTextVariationModel>();
    }
}
