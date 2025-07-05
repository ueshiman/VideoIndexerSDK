using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class StreamingUrlMapper : IStreamingUrlMapper
    {
        public StreamingUrlModel MapFrom(ApiStreamingUrlModel model)
        {
            return new StreamingUrlModel
            {
                Url = model.url,
                Jwt = model.jwt
            };
        }

        public ApiStreamingUrlModel MapTo(StreamingUrlModel model)
        {
            return new ApiStreamingUrlModel
            {
                url = model.Url,
                jwt = model.Jwt
            };
        }
    }
}
