using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IFaceFilterMapper
{
    FaceFilterModel MapFrom(ApiFaceFilterModel apiFaceFilterModel);
    ApiFaceFilterModel MapToApiFaceFilterModel(FaceFilterModel faceFilterModel);
}