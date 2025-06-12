using System;
using System.Collections.Generic;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class UpdateVideoIndexRequestModel
    {
        public string VideoId { get; set; }
        public List<PatchOperationModel> PatchOperations { get; set; }
        public string? Language { get; set; }
    }

}
