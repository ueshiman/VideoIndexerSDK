using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IFaceModelMapper
{
    FaceModel MapFrom(ApiFaceModel apiModel);
    ApiFaceModel MapToApiFaceModel(FaceModel model);
}