using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// ロゴのテキストバリエーションモデル
    /// </summary>
    public class ApiLogoTextVariationModel
    {
        public string text { get; set; } = "";
        public bool caseSensitive { get; set; }
        public DateTimeOffset creationTime { get; set; }
        public string? createdBy { get; set; }
    }
}
