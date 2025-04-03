using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoMigrationsListMapper : IVideoMigrationsListMapper
    {
        private readonly IVideoMigrationMapper _videoMigrationMapper;
        private readonly IPagingInfoMapper _pagingInfoMapper;

        public VideoMigrationsListMapper(IVideoMigrationMapper videoMigrationMapper, IPagingInfoMapper pagingInfoMapper)
        {
            _videoMigrationMapper = videoMigrationMapper;
            _pagingInfoMapper = pagingInfoMapper;
        }

        public VideoMigrationsListModel? MapFrom(ApiVideoMigrationsListModel? dataModel)
        {
            return dataModel is null ? null :  new VideoMigrationsListModel
            {
                Results = dataModel.results.Select(_videoMigrationMapper.MapFrom).ToList(),
                NextPage = _pagingInfoMapper.MapFrom(dataModel.nextPage),
            };
        }
    }
}
