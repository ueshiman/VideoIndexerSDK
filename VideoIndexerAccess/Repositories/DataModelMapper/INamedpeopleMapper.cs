using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper;

public interface INamedPeopleMapper
{
    NamedPeople MapFrom(NamedPeopleApiModel model);
}