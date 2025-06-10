using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class TextVariationModel
    {
        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { get; set; } = "";
        /// <summary>
        /// 大文字小文字を区別するかどうか
        /// </summary>
        public bool CaseSensitive { get; set; }
    }
}
