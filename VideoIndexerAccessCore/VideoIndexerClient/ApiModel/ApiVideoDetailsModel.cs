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
        public string? PublishedUrl { get; set; }
        public string? ViewToken { get; set; }
        public string? State { get; set; }
        public string? ProcessingProgress { get; set; }
        public string? FailureMessage { get; set; }
    }
}
