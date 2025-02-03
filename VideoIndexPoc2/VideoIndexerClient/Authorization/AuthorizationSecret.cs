using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization
{
    public class AuthorizationSecret : IAuthorizationSecret
    {
        private static readonly string _tenantId = Environment.GetEnvironmentVariable("VIDEOINDEXER_TENANT_ID");
        private static readonly string _clientId = Environment.GetEnvironmentVariable("VIDEOINDEXER_CLIENT_ID");
        private static readonly string _clientSecret = Environment.GetEnvironmentVariable("VIDEOINDEXER_CLIENT_SECRET");

        public string TenantId => _tenantId;
        public string ClientId => _clientId;
        public string ClientSecret => _clientSecret;

    }
}
