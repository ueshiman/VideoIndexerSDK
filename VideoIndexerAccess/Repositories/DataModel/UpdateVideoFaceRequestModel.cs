using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UpdateVideoFaceRequestModel
    {
        public string VideoId { get; set; }
        public int FaceId { get; set; }
        public string? NewName { get; set; }
        public string? PersonId { get; set; }
        public bool? CreateNewPerson { get; set; }
    }
}
