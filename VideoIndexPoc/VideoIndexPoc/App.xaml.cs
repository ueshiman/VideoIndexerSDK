using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Serilog;
using VideoIndexPoc.VideoIndexerClient;
using VideoIndexPoc.VideoIndexerClient.ApiAccess;
using VideoIndexPoc.VideoIndexerClient.Auth;
using VideoIndexPoc.VideoIndexerClient.FileAccess;
using VideoIndexPoc.VideoIndexerClient.Parser;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexPoc
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {

        public static IHost AppHost { get; private set; }

        public static ILogger<App> Logger { get; private set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            // Serilog の設定
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(
                    path: "logs/app-log.txt",           // 出力先のファイル
                    rollingInterval: RollingInterval.Day, // ログファイルを日単位でローテーション
                    retainedFileCountLimit: 7,          // 保持するファイルの最大数
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            // Microsoft.Extensions.Logging に統合
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(); // Serilog を追加
            });

            Logger = loggerFactory.CreateLogger<App>();
            Logger.LogInformation("アプリケーションが起動しました。");

            //var armAccessToken = AccountTokenProvider.GetArmAccessToken();

            // ホストを構築
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // サービスの登録
                    services.AddSingleton<MainWindow>();
                    services.AddTransient<Windows.ListWindow>();
                    services.AddTransient<Windows.ItemWindow>();

                    services.AddSingleton<IAccountTokenProviderDynamic, AccountTokenProviderDynamic>();
                    services.AddTransient<IVideoIndexerClient, VideoIndexerClient.VideoIndexerClient>();
                    services.AddTransient<IVideoListParser, VideoListParser>();
                    services.AddTransient<IVideoListAccess, VideoListAccess>();
                    services.AddTransient<IVideoItemParser, VideoItemParser>();
                    services.AddTransient<IVideoItemAccess, VideoItemAccess>();
                    services.AddTransient<IVideoDownload, VideoDownload>();
                    services.AddTransient<IUrlAccess, UrlAccess>();
                })
                .Build();
        }

        public MainWindow MainWindow { get; private set; }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            Logger.LogInformation("OnLaunched イベントが呼び出されました。");
            // ホストから MainWindow を取得して起動
            MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            MainWindow.Activate();
            base.OnLaunched(args);
        }
    }
}
