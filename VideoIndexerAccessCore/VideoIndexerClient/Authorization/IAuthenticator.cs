namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization;

public interface IAuthenticator
{
    Task<string> GetAccessTkenAsync();
}