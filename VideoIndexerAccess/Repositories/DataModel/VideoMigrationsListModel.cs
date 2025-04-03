using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoMigrationsListModel
    {
        public List<VideoMigrationModel?> Results { get; set; } = new List<VideoMigrationModel?>();
        public PagingInfoModel? NextPage { get; set; }
    }
}
