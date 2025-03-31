using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    /// <summary>
    /// 個々のビデオの詳細情報を格納するクラス。
    /// </summary>
    public class ApiVideoDetailsModel
    {
        public string? publishedUrl { get; set; }
        public string? viewToken { get; set; }
        public string? state { get; set; }
        public string? processingProgress { get; set; }
        public string? failureMessage { get; set; }
    }
}
