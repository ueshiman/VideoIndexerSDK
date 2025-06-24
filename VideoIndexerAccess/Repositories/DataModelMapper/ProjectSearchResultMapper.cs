using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class ProjectSearchResultMapper : IProjectSearchResultMapper
    {
        private readonly IProjectSearchResultItemMapper _projectSearchResultItemMapper;
        private readonly IPagingInfoMapper _pagingInfoMapper;

        public ProjectSearchResultMapper(IProjectSearchResultItemMapper projectSearchResultItemMapper, IPagingInfoMapper pagingInfoMapper)
        {
            _projectSearchResultItemMapper = projectSearchResultItemMapper;
            _pagingInfoMapper = pagingInfoMapper;
        }

        public ProjectSearchResultModel MapFrom(ApiProjectSearchResultModel model)
        {
            return new ProjectSearchResultModel
            {
                Results = model.results?.Select(_projectSearchResultItemMapper.MapFrom).ToList(),
                NextPage = _pagingInfoMapper.MapFrom(model.nextPage),
            };
        }
        public ApiProjectSearchResultModel MapToApiProjectSearchResultModel(ProjectSearchResultModel model)
        {
            return new ApiProjectSearchResultModel
            {
                results = model.Results?.Select(_projectSearchResultItemMapper.MapToApiProjectSearchResultItemModel).ToList(),
                nextPage = _pagingInfoMapper.MapToApiPagingInfoModel(model.NextPage),
            };
        }
    }
}
