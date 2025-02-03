using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexPoc.VideoIndexerClient.Model
{

    public class VideoListJsonModel
    {
        public VideoListItem[] results { get; set; }
        public Nextpage nextPage { get; set; }
    }

    public class Nextpage
    {
        public int pageSize { get; set; }
        public int skip { get; set; }
        public bool done { get; set; }
    }
}
