using System.Collections.Generic;
using VideoIndexPoc.VideoIndexerClient.Model;

namespace VideoIndexPoc.VideoIndexerClient.Parser;

public interface IVideoListParser
{
    IEnumerable<VideoListItem> ParseVideoList(string jsonResponse);
}