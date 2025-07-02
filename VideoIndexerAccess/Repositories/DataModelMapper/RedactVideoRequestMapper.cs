using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class RedactVideoRequestMapper : IRedactVideoRequestMapper
    {
        private readonly IFaceRedactionMapper _faceRedactionMapper;

        public RedactVideoRequestMapper(IFaceRedactionMapper faceRedactionMapper)
        {
            _faceRedactionMapper = faceRedactionMapper;
        }

        public RedactVideoRequestModel MapFroModel(ApiRedactVideoRequestModel model) => new() { Faces = model.faces != null ? _faceRedactionMapper.MapFrom(model.faces) : null, };

        public ApiRedactVideoRequestModel MapToApiRedactVideoRequestModel(RedactVideoRequestModel model) => new() { faces = model.Faces != null ? _faceRedactionMapper.MapToApiFaceRedactionModel(model.Faces) : null, };
    }
}
