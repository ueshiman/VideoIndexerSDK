namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface ISecureLogMessageBuilder
{
    string BuildRequestUri(string baseUri, string? accessToken, out string logUrl);
}