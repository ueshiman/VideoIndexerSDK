using System.Threading.Tasks;

namespace VideoIndexPoc2.VideoIndexerClient.FileAccess;

public interface IUrlAccess
{
    Task DownloadVideoAsync(string downloadUrl, string outputPath);
}