using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.VideoIndexerClient.Parser
{
    public interface IVideoItemParser
    {
        VideoItem ParseVideoItem(string jsonResponse);
    }
}