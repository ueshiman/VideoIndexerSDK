using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.Parser
{
    public interface IVideoItemParser
    {
        VideoItemApiModel ParseVideoItem(string jsonResponse);
    }
}