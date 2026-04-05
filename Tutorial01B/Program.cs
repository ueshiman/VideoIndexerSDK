using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tutorial01B.Extensions;
using Tutorial01B.Services;

// -------------------------------------------------------
// 設定の構築
// appsettings.json と promptssettings.json を両方読み込みます。
// -------------------------------------------------------
IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile("promptssettings.json", optional: false, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();

// -------------------------------------------------------
// DI コンテナの構築
// -------------------------------------------------------
var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder.AddConsole();
    builder.SetMinimumLevel(LogLevel.Information);
});

services.AddMultiAgentSystem(configuration);

await using var serviceProvider = services.BuildServiceProvider();

// -------------------------------------------------------
// アプリケーションの実行
// -------------------------------------------------------
var chatService = serviceProvider.GetRequiredService<IChatService>();
await chatService.RunSampleAsync();
