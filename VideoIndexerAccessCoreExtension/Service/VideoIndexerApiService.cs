using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Polly;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Authorization;
using VideoIndexerAccessCore.VideoIndexerClient.Configuration;
using VideoIndexerAccessCore.VideoIndexerClient.FileAccess;
using VideoIndexerAccessCore.VideoIndexerClient.HttpAccess;
using VideoIndexerAccessCore.VideoIndexerClient.Parser;

namespace VideoIndexerAccessCoreExtension.Service
{
    public static class VideoIndexerApiService
    {
        public static IServiceCollection AddVideoIndexerCore(this IServiceCollection services)
        {
            services.AddVideoIndexerApi();
            services.AddVideoIndexerAuthorization();
            services.AddVideoIndexerConfiguration();
            services.AddVideoIndexerFileAccess();
            services.AddVideoIndexerHttpAccess();
            services.AddAccountApi();

            return services;
        }

        public static IServiceCollection AddVideoIndexerApi(this IServiceCollection services)
        {
            services.TryAddTransient<ISecureLogMessageBuilder, SecureLogMessageBuilder>();
            services.TryAddTransient<IAccounApitAccess, AccountApiAccess>();
            services.TryAddTransient<IBrandsApiAccess, BrandsApiAccess>();
            services.TryAddTransient<IClassicLanguageCustomizationApiAccess, ClassicLanguageCustomizationApiAccess>();
            services.TryAddTransient<IProjectMigrationApiAccess, ProjectMigrationApiAccess>();
            services.TryAddTransient<IVideoArtifactApiAccess, VideoArtifactApiAccess>();
            services.TryAddTransient<IVideoDownloadApiAccess, VideoDownloadApiAccess>();
            services.TryAddTransient<IVideoIndexApiAccess, VideoIndexApiAccess>();
            services.TryAddTransient<IVideoItemParser, VideoItemParser>();
            services.TryAddTransient<IVideoListParser, VideoListParser>();
            services.TryAddTransient<IVideoListApiAccess, VideoListAccessApiAccess>();
            services.TryAddTransient<IVideoMigrationApiAccess, VideoMigrationApiAccess>();
            services.TryAddTransient<ICustomLogosApiAccess, CustomLogosApiAccess>();
            services.TryAddTransient<IIndexingApiAccess, IndexingApiAccess>();
            services.TryAddTransient<IJobsApiAccess, JobsApiAccess>();
            services.TryAddTransient<ILanguagesApiAccess, LanguagesApiAccess>();
            services.TryAddTransient<IPersonModelsApiAccess, PersonModelsApiAccess>();
            services.TryAddTransient<IProjectsApiAccess, ProjectsApiAccess>();
            services.TryAddTransient<IPromptContentApiAccess, PromptContentApiAccess>();
            services.TryAddTransient<IRedactionApiAccess, RedactionApiAccess>();
            services.TryAddTransient<ISpeechCustomizationApiAccess, SpeechCustomizationApiAccess>();
            services.TryAddTransient<ITextualSummarizationApiAccess, TextualSummarizationApiAccess>();
            services.TryAddTransient<ITrialAccountAccessTokensApiAccess, TrialAccountAccessTokensApiAccess>();
            services.TryAddTransient<ITrialAccountsApiAccess, TrialAccountsApiAccess>();
            services.TryAddTransient<IVideosApiAccess, VideosApiAccess>();
            services.TryAddTransient<IWidgetsApiAccess, WidgetsApiAccess>();
            
            return services;
        }

        public static IServiceCollection AddAccountApi(this IServiceCollection services)
        {
            services.TryAddTransient<IAccountMigrationStatusApiAccess, AccountMigrationStatusApiAccess>();

            return services;
        }

        public static IServiceCollection AddVideoIndexerAuthorization(this IServiceCollection services)
        {
            services.TryAddTransient<IAuthenticator, Authenticator>();
            services.TryAddTransient<IAuthorizationSecret, AuthorizationSecret>();
            services.TryAddSingleton<IAccountTokenProviderDynamic, AccountTokenProviderDynamic>();

            return services;
        }

        public static IServiceCollection AddVideoIndexerConfiguration(this IServiceCollection services)
        {
            services.TryAddTransient<IApiResourceConfigurations, ApiResourceConfigurations>();

            return services;
        }

        public static IServiceCollection AddVideoIndexerFileAccess(this IServiceCollection services)
        {
            services.TryAddTransient<IUrlAccess, UrlAccess>();

            return services;
        }

        public static IServiceCollection AddVideoIndexerHttpAccess(this IServiceCollection services)
        {
            services.TryAddTransient<IDurableHttpClient, DurableHttpClient>();

            services.AddHttpClient(ApiResourceConfigurations.DefaultHttpClientNameDefault)
                .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                {
                    AllowAutoRedirect = false // Redirectを無効に設定
                })
                .AddResilienceHandler("my-pipeline", builder =>
                {
                    // Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
                    builder.AddRetry(new HttpRetryStrategyOptions
                    {
                        MaxRetryAttempts = 4,
                        Delay = TimeSpan.FromSeconds(2),
                        BackoffType = DelayBackoffType.Exponential
                    });

                    // Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
                    builder.AddTimeout(TimeSpan.FromSeconds(5));
                });
            return services;
        }
    }
}
