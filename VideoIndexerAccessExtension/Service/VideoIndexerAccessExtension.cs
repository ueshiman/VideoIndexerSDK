using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccess.Repositories.VideoItemRepository;
using VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;
using VideoIndexerAccessCoreExtension.Service;

namespace VideoIndexerAccessExtension.Service;

public static class VideoIndexerAccessExtension
{
    public static IServiceCollection AddVideoIndexerAccess(this IServiceCollection services)
    {
        services.AddVideoIndexerAuthorizeAccess();
        services.AddVideoIndexerDataModelMapper();
        services.AddVideoIndexerVideoRepository();
        services.AddVideoIndexerCore();

        return services;
    }

    public static IServiceCollection AddVideoIndexerAuthorizeAccess(this IServiceCollection services)
    {
        services.TryAddTransient<IAuthenticationTokenizer, AuthenticationTokenizer>();

        return services;
    }

    public static IServiceCollection AddVideoIndexerDataModelMapper(this IServiceCollection services)
    {
        services.TryAddTransient<IAppearanceMapper, AppearanceMapper>();
        services.TryAddTransient<IAudioEffectsMapper, AudioEffectsMapper>();
        services.TryAddTransient<IBrandMapper, BrandMapper>();
        services.TryAddTransient<IDurationMapper, DurationMapper>();
        services.TryAddTransient<IEmotionsMapper, EmotionsMapper>();
        services.TryAddTransient<IFaceMapper, FaceMapper>();
        services.TryAddTransient<IFramePatternsMapper, FramePatternsMapper>();
        services.TryAddTransient<IInsightsMapper, InsightsMapper>();
        services.TryAddTransient<IInstanceMapper, InstanceMapper>();
        services.TryAddTransient<IItemVideoMapper, ItemVideoMapper>();
        services.TryAddTransient<IKeyWordMapper, KeyWordMapper>();
        services.TryAddTransient<ILabelMapper, LabelMapper>();
        services.TryAddTransient<INamedLocationMapper, NamedLocationMapper>();
        services.TryAddTransient<INamedPeopleMapper, NamedPeopleMapper>();
        services.TryAddTransient<IRangeOfVideoMapper, RangeOfVideoMapper>();
        services.TryAddTransient<ISentimentsMapper, SentimentsMapper>();
        services.TryAddTransient<ISocialMapper, SocialMapper>();
        services.TryAddTransient<ISpeakerLongestMonologMapper, SpeakerLongestMonologMapper>();
        services.TryAddTransient<ISpeakerMapper, SpeakerMapper>();
        services.TryAddTransient<ISpeakerNumberOfFragmentsMapper, SpeakerNumberOfFragmentsMapper>();
        services.TryAddTransient<ISpeakerTalkToListenRatioMapper, SpeakerTalkToListenRatioMapper>();
        services.TryAddTransient<ISpeakerWordCountMapper, SpeakerWordCountMapper>();
        services.TryAddTransient<IStatisticsMapper, StatisticsMapper>();
        services.TryAddTransient<ISummarizedInsightsMapper, SummarizedInsightsMapper>();
        services.TryAddTransient<ITopicMapper, TopicMapper>();
        services.TryAddTransient<ITopic1Mapper, Topic1Mapper>();
        services.TryAddTransient<IVideosRangeMapper, VideosRangeMapper>();
        services.TryAddTransient<IVideoStrangeMapper, VideoStrangeMapper>();
        services.TryAddTransient<IVideoItemDataMapper, VideoItemDataMapper>();
        services.TryAddTransient<IVideoListDataModelMapper, VideoListDataModelMapper>();
        services.TryAddTransient<IBlockMapper, BlockMapper>();
        services.TryAddTransient<IKeyword1Mapper, Keyword1Mapper>();
        services.TryAddTransient<ILabel1Mapper, Label1Mapper>();
        services.TryAddTransient<IOcrMapper, OcrMapper>();
        services.TryAddTransient<ITranscriptMapper, TranscriptMapper>();
        services.TryAddTransient<ISceneMapper, SceneMapper>();
        services.TryAddTransient<IKeyFrameMapper, KeyFrameMapper>();
        services.TryAddTransient<IShotMapper, ShotMapper>();
        services.TryAddTransient<ISentiment1Mapper, Sentiment1Mapper>();
        services.TryAddTransient<ITextualcontentmoderationMapper, TextualcontentmoderationMapper>();
        services.TryAddTransient<ISpeakertalktolistenratio1Mapper, Speakertalktolistenratio1Mapper>();
        services.TryAddTransient<ISpeakerlongestmonolog1Mapper, Speakerlongestmonolog1Mapper>();
        services.TryAddTransient<ISpeakernumberoffragments1Mapper, Speakernumberoffragments1Mapper>();
        services.TryAddTransient<ISpeakerwordcount1Mapper, Speakerwordcount1Mapper>();
        services.TryAddTransient<IStatistics1Mapper, Statistics1Mapper>();
        services.TryAddTransient<IProjectMigrationApiAccess,ProjectMigrationApiAccess>();
        services.TryAddTransient<IVideoArtifactApiAccess, VideoArtifactApiAccess>();
        services.TryAddTransient<IVideoDownloadApiAccess, VideoDownloadApiAccess>();
        services.TryAddTransient<IVideoIndexApiAccess, VideoIndexApiAccess>();
        services.TryAddTransient<IClassicLanguageCustomizationApiAccess, ClassicLanguageCustomizationApiAccess>();

        return services;
    }

    public static IServiceCollection AddVideoIndexerVideoRepository(this IServiceCollection services)
    {
        services.TryAddTransient<IVideoDataRepository, VideoDataRepository>();
        services.TryAddTransient<IVideoIndexRepository, VideoIndexRepository>();
        services.TryAddTransient<IVideoListRepository, VideoListRepository>();
        services.TryAddTransient<IAccountRepository, AccountRepository>();

        return services;
    }
}