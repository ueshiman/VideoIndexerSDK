using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class FaceFilterMapper : IFaceFilterMapper
    {
        public FaceFilterModel MapFrom(ApiFaceFilterModel apiFaceFilterModel)
        {
            return new FaceFilterModel
            {
                Ids = apiFaceFilterModel.ids.ToList() ?? new List<int>(),
                Scope = apiFaceFilterModel.scope
            };
        }
        public ApiFaceFilterModel MapToApiFaceFilterModel(FaceFilterModel faceFilterModel)
        {
            return new ApiFaceFilterModel
            {
                ids = faceFilterModel.Ids.ToList() ?? new List<int>(),
                scope = faceFilterModel.Scope
            };
        }
    }
}
