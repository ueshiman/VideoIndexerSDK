using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class PatchOperationMapper : IPatchOperationMapper
    {
        public PatchOperationModel MapFrom(ApiPatchOperationModel model)
        {
            return new PatchOperationModel
            {
                Op = model.op,
                Path = model.path,
                Value = model.value,
            };
        }

        public ApiPatchOperationModel MapToApiPatchOperationModel(PatchOperationModel model)
        {
            return new ApiPatchOperationModel
            {
                op = model.Op,
                path = model.Path,
                value = model.Value,
            };
        }
    }
}
