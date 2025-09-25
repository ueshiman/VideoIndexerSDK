using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class FaceModelMapper : IFaceModelMapper
    {
        public FaceModel MapFrom(ApiFaceModel apiModel)
        {
            return new FaceModel
            {
                Id = apiModel.id,
                State = apiModel.state,
                SourceType = apiModel.sourceType,
                SourceVideoId = apiModel.sourceVideoId
            };
        }

        public ApiFaceModel MapToApiFaceModel(FaceModel model)
        {
            return new ApiFaceModel
            {
                id = model.Id,
                state = model.State,
                sourceType = model.SourceType,
                sourceVideoId = model.SourceVideoId
            };
        }
    }
}
