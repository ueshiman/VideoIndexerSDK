namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class SecureLogMessageBuilder : ISecureLogMessageBuilder
    {
        public string BuildRequestUri(string baseUri, string? accessToken, out string logUrl)
        {
            logUrl = baseUri;
            if (!string.IsNullOrEmpty(accessToken))
            {
                baseUri += $"?accessToken={accessToken}";
                logUrl += $"?accessToken=***";
            }
            return baseUri;
        }
    }
}
