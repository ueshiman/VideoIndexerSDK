using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IFilePropertiesMapper
{
    FilePropertiesModel MapFrom(ApiFilePropertiesModel apiModel);
    ApiFilePropertiesModel MapFromApiFilePropertiesModel(FilePropertiesModel model);
}