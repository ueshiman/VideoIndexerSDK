namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization
{
    public class AuthorizationSecret : IAuthorizationSecret
    {
        private static readonly string _tenantId = Environment.GetEnvironmentVariable("VIDEOINDEXER_TENANT_ID") ?? String.Empty;
        private static readonly string _clientId = Environment.GetEnvironmentVariable("VIDEOINDEXER_CLIENT_ID") ?? String.Empty;
        private static readonly string _clientSecret = Environment.GetEnvironmentVariable("VIDEOINDEXER_CLIENT_SECRET") ?? String.Empty;

        public string TenantId => _tenantId;
        public string ClientId => _clientId;
        public string ClientSecret => _clientSecret;

    }
}
