using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomPersonModel
    {
        /// <summary>
        /// Person Model の ID (GUID 形式)
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Person Model の名前
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// デフォルトの Person Model であるかどうか
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Person Model に登録されている Person の数
        /// </summary>
        public int PersonsCount { get; set; }

        /// <summary>
        /// Person の識別スコアのしきい値
        /// </summary>
        public decimal PersonIdentificationThreshold { get; set; }
    }
}
