using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴ作成レスポンスモデル
    /// </summary>
    public class ApiLogoContractModel
    {
        public Guid id { get; set; }

        public DateTimeOffset creationTime { get; set; }

        public DateTimeOffset lastUpdateTime { get; set; }

        public string? lastUpdatedBy { get; set; }

        public string? createdBy { get; set; }

        public string? name { get; set; }

        public string? wikipediaSearchTerm { get; set; }

        public List<ApiLogoTextVariationModel> textVariations { get; set; } = new ();
    }
}
