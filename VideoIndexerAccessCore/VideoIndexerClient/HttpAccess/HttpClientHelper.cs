using System.Collections.Specialized;
using System.Web;

namespace VideoIndexerAccessCore.VideoIndexerClient.HttpAccess
{
    public static class HttpClientHelper
    {
        public static HttpClient CreateHttpClient()
        {
            var handler = new SocketsHttpHandler
            {
                AllowAutoRedirect = false,
            };
            var httpClient = new HttpClient(handler);
            return httpClient;
        }
        
        public static string CreateQueryString(this IDictionary<string, string> parameters)
        {
            NameValueCollection queryParameters = HttpUtility.ParseQueryString(string.Empty);
            foreach (var parameter in parameters)
            {
                queryParameters[parameter.Key] = parameter.Value;
            }

            return queryParameters.ToString() ?? String.Empty;
        }

        public static void VerifyStatus(this HttpResponseMessage response, System.Net.HttpStatusCode excpectedStatusCode)
        {
            if (response.StatusCode != excpectedStatusCode)
            {
                throw new Exception(response.ToString());
            }
        }
    }
}
