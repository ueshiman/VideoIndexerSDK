using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface ITextVariationMapper
{
    TextVariationModel MapFrom(ApiTextVariationModel apiTextVariationModel);
    ApiTextVariationModel ToApiTextVariationModel(TextVariationModel textVariationModel);
}