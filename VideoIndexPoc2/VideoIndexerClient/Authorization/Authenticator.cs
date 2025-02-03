using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexPoc2.VideoIndexerClient.Configuration;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization
{
    public class Authenticator : IAuthenticator
    {
        private readonly ILogger<Authenticator> _logger;
        private readonly IAccountTokenProviderDynamic _accountTokenProvider;
        private readonly IApiResourceConfigurations _apiResourceConfigurations;

        public Authenticator(ILogger<Authenticator> logger, IApiResourceConfigurations apiResourceConfigurations, IAccountTokenProviderDynamic accountTokenProvider)
        {
            _logger = logger;
            _apiResourceConfigurations = apiResourceConfigurations;
            _accountTokenProvider = accountTokenProvider;
        }

        public async Task<string> GetAccessTkenAsync()
        {
            try
            {
                var armAccessToken = await _accountTokenProvider.GetArmAccessTokenAsync();
                return await _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating.");
                throw;
            }
        }


        public string GetAccessToken()
        {
            try
            {
                var armAccessToken = _accountTokenProvider.GetArmAccessToken();
                return _accountTokenProvider.GetAccountAccessTokenAsync(armAccessToken).Result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating.");
                throw;
            }
        }
    }
}
