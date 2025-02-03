using System.Threading.Tasks;

namespace VideoIndexPoc.VideoIndexerClient.FileAccess;

public interface IUrlAccess
{
    Task DownloadVideoAsync(string downloadUrl, string outputPath);
}