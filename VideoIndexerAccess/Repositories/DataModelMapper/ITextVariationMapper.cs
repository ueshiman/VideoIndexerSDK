using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITextVariationMapper
{
    TextVariationModel MapFrom(ApiTextVariationModel model);
    ApiTextVariationModel MapToApiTextVariationModel(TextVariationModel model);
}