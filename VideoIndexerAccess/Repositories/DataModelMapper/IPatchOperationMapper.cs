using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPatchOperationMapper
{
    PatchOperationModel MapFrom(ApiPatchOperationModel model);
    ApiPatchOperationModel MapToApiPatchOperationModel(PatchOperationModel model);
}