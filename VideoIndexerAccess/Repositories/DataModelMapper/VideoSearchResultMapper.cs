using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoSearchResultMapper: IVideoSearchResultMapper
    {
        private readonly IVideoSearchResultItemMapper _videoSearchResultItemMapper;
        private readonly IPagingInfoMapper _pagingInfoMapper;

        public VideoSearchResultMapper(IVideoSearchResultItemMapper videoSearchResultItemMapper, IPagingInfoMapper pagingInfoMapper)
        {
            _videoSearchResultItemMapper = videoSearchResultItemMapper;
            _pagingInfoMapper = pagingInfoMapper;
        }

        public VideoSearchResultModel MapFrom(ApiVideoSearchResultModel model)
        {
            return new VideoSearchResultModel()
            {
                Results = model.results?.Select(item => _videoSearchResultItemMapper.MapFrom(item)).ToList(),
                NextPage = _pagingInfoMapper.MapFrom(model.nextPage) 
            };
        }

        public ApiVideoSearchResultModel MapToApiVideoSearchResultModel(VideoSearchResultModel model)
        {
            return new ApiVideoSearchResultModel()
            {
                results = model.Results?.Select(item => _videoSearchResultItemMapper.MapToApiVideoSearchResultItemModel(item)).ToList(),
                nextPage = _pagingInfoMapper.MapToApiPagingInfoModel(model.NextPage)
            };
        }
    }
}
