using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexPoc2.Repositories.AuthorizAccesss;
using VideoInexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoInexerAccessCore.VideoIndexerClient.Model;

namespace VideoIndexPoc2.Repositories.VideoItemRepository
{
    public class VideoIndexRepository : IVideoIndexRepository
    {
        private readonly ILogger<VideoListRepository> _logger;
        private readonly IAuthenticationTokenizer _authenticationTokenizer;
        private readonly IAccounApitAccess _accountAccess;
        private readonly IVideoItemApiAccess _videoItemApiAccess;


        public VideoIndexRepository(ILogger<VideoListRepository> logger, IAuthenticationTokenizer authenticationTokenizer, IAccounApitAccess accountAccess, IVideoItemApiAccess videoItemApiAccess)
        {
            _logger = logger;
            _authenticationTokenizer = authenticationTokenizer;
            _accountAccess = accountAccess;
            _videoItemApiAccess = videoItemApiAccess;
        }

        public async Task<string> GetVideoItemJsonAsync(string location, string accountId, string videoId)
        {
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            return await _videoItemApiAccess.GetVideoItemJsonAsync(location, accountId, videoId, accessToken);
        }

        public async Task<VideoItem> GetVideoItemAsync(string location, string accountId, string videoId)
        {
            var accessToken = await _authenticationTokenizer.GetAccessToken();
            return await _videoItemApiAccess.GetVideoItemAsync(location, accountId, videoId, accessToken);
        }

    }
}
