using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class TrialAccountStatisticsMapper : ITrialAccountStatisticsMapper
    {
        public TrialAccountStatisticsModel MapFrom(ApiTrialAccountStatisticsModel model)
        {
            return new TrialAccountStatisticsModel
            {
                VideosCount = model.videosCount,
                LingusiticModelsCount = model.lingusiticModelsCount,
                PersonModlesCount = model.personModlesCount,
                BrandsCount = model.brandsCount,
            };
        }
        public ApiTrialAccountStatisticsModel MapToApiTrialAccountStatisticsModel(TrialAccountStatisticsModel model)
        {
            return new ApiTrialAccountStatisticsModel
            {
                videosCount = model.VideosCount,
                lingusiticModelsCount = model.LingusiticModelsCount,
                personModlesCount = model.PersonModlesCount,
                brandsCount = model.BrandsCount,
            };
        }
    }
}
