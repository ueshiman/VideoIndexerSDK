using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoGroupResponseMapper : ILogoGroupResponseMapper
    {
        public LogoGroupResponseModel MapFrom(ApiLogoGroupResponseModel model)
        {
            return new LogoGroupResponseModel
            {
                Id = model.id,
                Name = model.name,
                Description = model.description,
                CreatedBy = model.createdBy
            };
        }

        public ApiLogoGroupResponseModel MapToApiLogoGroupResponseModel(LogoGroupResponseModel model)
        {
            return new ApiLogoGroupResponseModel
            {
                id = model.Id,
                name = model.Name,
                description = model.Description,
                createdBy = model.CreatedBy
            };
        }

    }
}
