using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class BrandsMapper
    {
        public BrandModel MapFrom(ApiBrandModel model)
        {
            return new BrandModel
            {
                ReferenceUrl = model.referenceUrl,
                Id = model.id,
                Name = model.name,
                AccountId = model.accountId,
                LastModifierUserName = model.lastModifierUserName,
                Create = model.create,
                LastModified = model.lastModified,
                Enabled = model.enabled,
                Description = model.description,
                Tags = model.tags,
            };
        }
    }
}
