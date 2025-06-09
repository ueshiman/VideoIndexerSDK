using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class LogoResponseMapper : ILogoResponseMapper
    {
        public LogoResponseModel Map(ApiLogoResponseModel apiModel)
        {
            if (apiModel == null)
            {
                throw new ArgumentNullException(nameof(apiModel), "API model cannot be null");
            }
            return new LogoResponseModel
            {
                Id = apiModel.id,
                Name = apiModel.name,
                CreatedBy = apiModel.createdBy,
                WikipediaSearchTerm = apiModel.wikipediaSearchTerm
            };
        }
        public ApiLogoResponseModel MapToApiLogoResponseModel(LogoResponseModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Model cannot be null");
            }
            return new ApiLogoResponseModel
            {
                id = model.Id,
                name = model.Name,
                createdBy = model.CreatedBy,
                wikipediaSearchTerm = model.WikipediaSearchTerm
            };
        }
    }
}
