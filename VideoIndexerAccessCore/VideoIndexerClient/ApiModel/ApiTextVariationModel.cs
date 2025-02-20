using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// テキストバリエーションモデル
    /// </summary>
    public class ApiTextVariationModel
    {
        public string text { get; set; } = "";
        public bool caseSensitive { get; set; }
    }
}
