using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VideoIndexerAccessExtension.Service;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // サービスの登録
        services.AddLogging(configure => configure.AddConsole());
        services.AddTransient<IVideoService, VideoService>();
        // 他のサービスの登録
        services.AddVideoIndexerAccess();
    })
    .Build();

await host.RunAsync();

public interface IVideoService
{
    void ProcessVideos();
}

public class VideoService : IVideoService
{
    private readonly ILogger<VideoService> _logger;

    public VideoService(ILogger<VideoService> logger)
    {
        _logger = logger;
    }

    public void ProcessVideos()
    {
        _logger.LogInformation("Processing videos...");
        // ビデオ処理のロジック
    }
}

