using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IFaceRedactionMapper
{
    FaceRedactionModel MapFrom(ApiFaceRedactionModel model);
    ApiFaceRedactionModel MapToApiFaceRedactionModel(FaceRedactionModel model);
}