using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectMigrationModel
    {
        public ProjectMigrationState Status { get; set; }
        public string? Name { get; set; }
        public string? Details { get; set; }
        public string? ProjectId { get; set; }
    }
}
