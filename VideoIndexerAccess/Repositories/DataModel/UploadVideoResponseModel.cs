using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UploadVideoResponseModel
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? State { get; set; }
        public string? PrivacyMode { get; set; }
    }
}
