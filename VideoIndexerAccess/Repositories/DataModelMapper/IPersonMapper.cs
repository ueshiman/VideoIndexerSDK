using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface IPersonMapper
{
    PersonModel MapFrom(ApiPersonModel model);
    ApiPersonModel MapToApiPersonModel(PersonModel model);
}