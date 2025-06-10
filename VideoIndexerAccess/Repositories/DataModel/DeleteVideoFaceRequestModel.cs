using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class DeleteVideoFaceRequestModel
    {
        public string VideoId { get; set; } = string.Empty;
        public int FaceId { get; set; }
    }
}
