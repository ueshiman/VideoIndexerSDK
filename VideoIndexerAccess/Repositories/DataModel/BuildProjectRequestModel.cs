using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class BuildProjectRequestModel
    {
        public int DelayMilliSecond { get; set; } = 5000;
        public required CreateProjectRequestModel CreateProjectRequest { get; set; }
    }
}
