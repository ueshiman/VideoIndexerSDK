using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
