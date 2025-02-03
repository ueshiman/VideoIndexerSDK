using System.Collections.Generic;
using VideoIndexPoc2.VideoIndexerClient.Model;

namespace VideoIndexPoc2.VideoIndexerClient.Parser;

public interface IVideoListParser
{
    IEnumerable<VideoListItem> ParseVideoList(string jsonResponse);
}