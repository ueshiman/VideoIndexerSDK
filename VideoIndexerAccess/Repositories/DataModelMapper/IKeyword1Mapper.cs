using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IKeyword1Mapper
{
    Keyword1 MapFrom(Keyword1ApiModel model);
}