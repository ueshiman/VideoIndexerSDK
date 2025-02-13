using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiBrandModel
    {
        public string? referenceUrl { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public string? accountId { get; set; }
        public string? lastModifierUserName { get; set; }
        public DateTime? create { get; set; }
        public DateTime? lastModified { get; set; }
        public bool? enabled { get; set; }
        public string? description { get; set; }
        public string[]? tags { get; set; }
    }
}
