using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class BrandsMapper : IBrandsMapper
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

        public ApiBrandModel MapToApiBrandModel(BrandModel model)
        {
            return new ApiBrandModel
            {
                referenceUrl = model.ReferenceUrl,
                id = model.Id,
                name = model.Name,
                accountId = model.AccountId,
                lastModifierUserName = model.LastModifierUserName,
                create = model.Create,
                lastModified = model.LastModified,
                enabled = model.Enabled,
                description = model.Description,
                tags = model.Tags,
            };
        }
    }
}
