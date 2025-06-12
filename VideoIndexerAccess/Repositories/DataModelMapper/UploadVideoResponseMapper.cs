using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class UploadVideoResponseMapper : IUploadVideoResponseMapper
    {
        public UploadVideoResponseModel MapFrom(ApiUploadVideoResponseModel model)
        {
            return new UploadVideoResponseModel
            {
                Id = model.id,
                Name = model.name,
                State = model.state,
                PrivacyMode = model.privacyMode
            };
        }

        public ApiUploadVideoResponseModel MapToApiUploadVideoResponseModel(UploadVideoResponseModel model)
        {
            return new ApiUploadVideoResponseModel
            {
                id = model.Id,
                name = model.Name,
                state = model.State,
                privacyMode = model.PrivacyMode
            };
        }
    }
}
