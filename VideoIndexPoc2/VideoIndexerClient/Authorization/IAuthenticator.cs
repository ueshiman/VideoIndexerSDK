using System.Threading.Tasks;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization;

public interface IAuthenticator
{
    Task<string> GetAccessTkenAsync();
}