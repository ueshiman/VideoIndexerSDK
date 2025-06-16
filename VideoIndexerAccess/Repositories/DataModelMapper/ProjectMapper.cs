using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectMapper : IProjectMapper
    {
        public ProjectModel MapFrom(ApiProjectModel apiProjectModel)
        {
            return new ProjectModel
            {
                AccountId = apiProjectModel.accountId,
                Id = apiProjectModel.id,
                Name = apiProjectModel.name,
                Created = apiProjectModel.created,
                LastModified = apiProjectModel.lastModified,
                IsSearchable = apiProjectModel.isSearchable
            };
        }
        public ApiProjectModel MapToApiProjectModel(ProjectModel projectModel)
        {
            return new ApiProjectModel
            {
                accountId = projectModel.AccountId,
                id = projectModel.Id,
                name = projectModel.Name,
                created = projectModel.Created,
                lastModified = projectModel.LastModified,
                isSearchable = projectModel.IsSearchable
            };
        }
    }
}
