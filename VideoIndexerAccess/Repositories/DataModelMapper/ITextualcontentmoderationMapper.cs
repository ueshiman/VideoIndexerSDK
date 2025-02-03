using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITextualcontentmoderationMapper
{
    Textualcontentmoderation MapFrom(TextualcontentModerationApiModel model);
}