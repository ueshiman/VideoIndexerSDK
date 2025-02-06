using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Serilog;
using System;
using VideoIndexerAccessExtension.Service;
using VideoIndexerExperiment2.Windows;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace VideoIndexerExperiment2
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>

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
                    //services.AddLogging(); // ロギングサービスを追加
                    services.AddLogging(configure => configure.AddConsole()); // ロギングサービスを追加

                    services.AddSingleton<MainWindow>();
                    services.AddTransient<ListWindow>();
                    services.AddTransient<ItemWindow>();

                    services.AddVideoIndexerAccess();

                    //services.AddTransient<IAuthenticationTokenizer, AuthenticationTokenizer>();
                    //services.AddTransient<IVideoListRepository, VideoListRepository>();
                    //services.AddTransient<IVideoIndexRepository, VideoIndexRepository>();

                    //services.AddTransient<IAuthenticator, Authenticator>();
                    //services.AddTransient<IDurableHttpClient, DurableHttpClient>();
                    //services.AddTransient<IAuthorizationSecret, AuthorizationSecret>();
                    //services.AddSingleton<IAccountTokenProviderDynamic, AccountTokenProviderDynamic>();
                    //services.AddTransient<IAccounApitAccess, AccountApiAccess>();
                    //services.AddTransient<IApiResourceConfigurations, ApiResourceConfigurations>();
                    ////services.AddTransient<IVideoIndexerClient, VideoIndexerClient>();
                    //services.AddTransient<IVideoListParser, VideoListParser>();
                    //services.AddTransient<IVideoListApiAccess, VideoListAccessApiAccess>();
                    //services.AddTransient<IVideoItemParser, VideoItemParser>();
                    //services.AddTransient<IVideoItemApiAccess, VideoIndexApiAccess>();
                    //services.AddTransient<IVideoDownloadApiAccess, VideoDownloadApiAccess>();
                    //services.AddTransient<IVideoArtifactApiAccess, VideoArtifactApiAccess>();


                    //services.AddTransient<IUrlAccess, UrlAccess>();
                    //services.AddHttpClient(ApiResourceConfigurations.DefaultHttpClientNameDefault)
                    //    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    //    {
                    //        AllowAutoRedirect = false // Redirectを無効に設定
                    //    })
                    //    .AddResilienceHandler("my-pipeline", builder =>
                    //    {
                    //        // Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
                    //        builder.AddRetry(new HttpRetryStrategyOptions
                    //        {
                    //            MaxRetryAttempts = 4,
                    //            Delay = TimeSpan.FromSeconds(2),
                    //            BackoffType = DelayBackoffType.Exponential
                    //        });

                    //        // Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
                    //        builder.AddTimeout(TimeSpan.FromSeconds(5));
                    //    });
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
            try
            {
                MainWindow = AppHost.Services.GetRequiredService<MainWindow>();
                MainWindow.Activate();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "MainWindow の取得に失敗しました。");
            }
            base.OnLaunched(args);
        }

        private Window? m_window;
    }
}
