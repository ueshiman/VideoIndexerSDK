using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoSearchMatchMapper : IVideoSearchMatchMapper
    {
        public VideoSearchMatchModel MapFrom(VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiVideoSearchMatchModel model)
        {
            return new VideoSearchMatchModel
            {
                StartTime = model.startTime,
                Type = model.type,
                SubType = model.subType,
                Text = model.text,
                ExactText = model.exactText,
            };
        }

        public VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiVideoSearchMatchModel MapToApiVideoSearchMatchModel(VideoSearchMatchModel model)
        {
            return new VideoIndexerAccessCore.VideoIndexerClient.ApiModel.ApiVideoSearchMatchModel
            {
                startTime = model.StartTime,
                type = model.Type,
                subType = model.SubType,
                text = model.Text,
                exactText = model.ExactText,
            };
        }
    }
}
