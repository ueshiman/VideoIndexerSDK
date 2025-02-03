using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient.Parser
{
    public interface IVideoItemParser
    {
        VideoItem ParseVideoItem(string jsonResponse);
    }
}