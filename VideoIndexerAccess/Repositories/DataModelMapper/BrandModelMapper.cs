using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class BrandModelMapper : IBrandModelMapper
    {
        public ApiBrandModel MapToApiBrandModel(BrandModel model)
        {
            return new ApiBrandModel
            {
                accountId = model.AccountId,
                create = model.Create,
                description = model.Description,
                enabled = model.Enabled,
                id = model.Id,
                lastModified = model.LastModified,
                lastModifierUserName = model.LastModifierUserName,
                name = model.Name,
                referenceUrl = model.ReferenceUrl,
                tags = model.Tags,
            };
        }

        public BrandModel MapFrom(ApiBrandModel model)
        {
            return new BrandModel
            {
                AccountId = model.accountId,
                Create = model.create,
                Description = model.description,
                Enabled = model.enabled,
                Id = model.id,
                LastModified = model.lastModified,
                LastModifierUserName = model.lastModifierUserName,
                Name = model.name,
                ReferenceUrl = model.referenceUrl,
                Tags = model.tags,
            };
        }
    }
}
