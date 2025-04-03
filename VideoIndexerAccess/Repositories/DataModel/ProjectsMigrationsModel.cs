using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class ProjectsMigrationsModel
    {
        public List<ProjectMigrationModel> Results { get; set; } = new List<ProjectMigrationModel>();
        public PagingInfoModel? NextPage { get; set; }
    }
}
