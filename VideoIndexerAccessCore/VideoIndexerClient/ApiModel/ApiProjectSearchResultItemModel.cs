using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class ApiProjectSearchResultItemModel
    {
        public string accountId { get; set; } = "";
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string created { get; set; } = "";
        public string lastModified { get; set; } = "";
        public string userName { get; set; } = "";
        public int durationInSeconds { get; set; }
        public string thumbnailVideoId { get; set; } = "";
        public string thumbnailId { get; set; } = "";
        public List<ApiVideoSearchMatchModel>? searchMatches { get; set; }
    }
}
