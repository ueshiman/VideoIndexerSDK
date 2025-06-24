using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class BuildProjectResponseModel
    {
        public BuildProjectResponseModelStatus Status{ get; set; }
        public ProjectModel? Project { get; set; }
        public ProjectRenderOperationModel? RenderOperation { get; set; }
    }


    public enum BuildProjectResponseModelStatus
    {
        Success,
        Failure,
        Started,
        InProgress
    }
}
