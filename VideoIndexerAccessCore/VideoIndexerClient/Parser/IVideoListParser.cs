using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.Parser;

public interface IVideoListParser
{
    IEnumerable<ApiVideoListItemModel> ParseVideoList(string jsonResponse);
}