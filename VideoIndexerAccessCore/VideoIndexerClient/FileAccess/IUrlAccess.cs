namespace VideoIndexerAccessCore.VideoIndexerClient.FileAccess;

public interface IUrlAccess
{
    Task DownloadVideoAsync(string downloadUrl, string outputPath);
}