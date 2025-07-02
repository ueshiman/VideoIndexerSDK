using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class FaceRedactionMapper : IFaceRedactionMapper
    {
        private readonly IFaceFilterMapper _faceFilterMapper;

        public FaceRedactionMapper(IFaceFilterMapper faceFilterMapper)
        {
            _faceFilterMapper = faceFilterMapper;
        }

        public FaceRedactionModel MapFrom(ApiFaceRedactionModel model)
        {
            return new FaceRedactionModel
            {
                BlurringKind = model.blurringKind,
                Filter = _faceFilterMapper.MapFrom(model.filter),
            };
        }

        public ApiFaceRedactionModel MapToApiFaceRedactionModel(FaceRedactionModel model)
        {
            return new ApiFaceRedactionModel
            {
                blurringKind = model.BlurringKind,
                filter = _faceFilterMapper.MapToApiFaceFilterModel(model.Filter),
            };
        }
    }
}