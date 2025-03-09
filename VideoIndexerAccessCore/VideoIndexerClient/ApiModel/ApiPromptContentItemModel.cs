using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiPromptContentItemModel
    {
        public int id { get; set; }
        public string? start { get; set; }
        public string? end { get; set; }
        public string? content { get; set; }
        public List<string>? frames { get; set; }
    }
}
