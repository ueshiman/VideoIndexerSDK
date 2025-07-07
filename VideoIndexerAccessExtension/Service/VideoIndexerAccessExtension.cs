using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VideoIndexerAccess.Repositories.AuthorizAccess;
using VideoIndexerAccess.Repositories.DataModelMapper;
using VideoIndexerAccess.Repositories.VideoItemRepository;
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
        services.TryAddTransient<IAccountMigrationStatusMapper, AccountMigrationStatusMapper>();
        services.TryAddTransient<IAccountSlimMapper, AccountSlimMapper>();
        services.TryAddTransient<IAppearanceMapper, AppearanceMapper>();
        services.TryAddTransient<IAudioEffectsMapper, AudioEffectsMapper>();
        services.TryAddTransient<IBlockMapper, BlockMapper>();
        services.TryAddTransient<IBrandMapper, BrandMapper>();
        services.TryAddTransient<IBrandModelMapper, BrandModelMapper>();
        services.TryAddTransient<IBrandModelSettingsMapper, BrandModelSettingsMapper>();
        services.TryAddTransient<IBrandsMapper, BrandsMapper>();
        services.TryAddTransient<ICustomLanguageMapper, CustomLanguageMapper>();
        services.TryAddTransient<ICustomLanguageModelTrainingDataFileMapper, CustomLanguageModelTrainingDataFileMapper>();
        services.TryAddTransient<ICustomLanguageRequestMapper, CustomLanguageRequestMapper>();
        services.TryAddTransient<IDeleteVideoResultMapper, DeleteVideoResultMapper>();
        services.TryAddTransient<IDurationMapper, DurationMapper>();
        services.TryAddTransient<IEmotionsMapper, EmotionsMapper>();
        services.TryAddTransient<IErrorResponseMapper, ErrorResponseMapper>();
        services.TryAddTransient<IFaceFilterMapper, FaceFilterMapper>();
        services.TryAddTransient<IFaceMapper, FaceMapper>();
        services.TryAddTransient<IFaceRedactionMapper, FaceRedactionMapper>();
        services.TryAddTransient<IFramePatternsMapper, FramePatternsMapper>();
        services.TryAddTransient<IInsightsMapper, InsightsMapper>();
        services.TryAddTransient<IInstanceMapper, InstanceMapper>();
        services.TryAddTransient<IItemVideoMapper, ItemVideoMapper>();
        services.TryAddTransient<IJobStatusResponseMapper, JobStatusResponseMapper>();
        services.TryAddTransient<IKeyFrameMapper, KeyFrameMapper>();
        services.TryAddTransient<IKeyword1Mapper, Keyword1Mapper>();
        services.TryAddTransient<IKeyWordMapper, KeyWordMapper>();
        services.TryAddTransient<ILabel1Mapper, Label1Mapper>();
        services.TryAddTransient<ILabelMapper, LabelMapper>();
        services.TryAddTransient<ILanguageModelDataMapper, LanguageModelDataMapper>();
        services.TryAddTransient<ILanguageModelEditMapper, LanguageModelEditMapper>();
        services.TryAddTransient<ILanguageModelFileDataMapper, LanguageModelFileDataMapper>();
        services.TryAddTransient<ILanguageModelFileMetadataMapper, LanguageModelFileMetadataMapper>();
        services.TryAddTransient<ILogoGroupContractMapper, LogoGroupContractMapper>();
        services.TryAddTransient<ILogoGroupLinkMapper, LogoGroupLinkMapper>();
        services.TryAddTransient<ILogoGroupRequestMapper, LogoGroupRequestMapper>();
        services.TryAddTransient<ILogoGroupResponseMapper, LogoGroupResponseMapper>();
        services.TryAddTransient<ILogoRequestMapper, LogoRequestMapper>();
        services.TryAddTransient<ILogoResponseMapper, LogoResponseMapper>();
        services.TryAddTransient<ILogoTextVariationMapper, LogoTextVariationMapper>();
        services.TryAddTransient<INamedLocationMapper, NamedLocationMapper>();
        services.TryAddTransient<INamedPeopleMapper, NamedPeopleMapper>();
        services.TryAddTransient<IOcrMapper, OcrMapper>();
        services.TryAddTransient<IPagingInfoMapper, PagingInfoMapper>();
        services.TryAddTransient<IPatchOperationMapper, PatchOperationMapper>();
        services.TryAddTransient<IProjectMapper, ProjectMapper>();
        services.TryAddTransient<IProjectMigrationMapper, ProjectMigrationMapper>();
        services.TryAddTransient<IProjectMigrationStateMapper, ProjectMigrationStateMapper>();
        services.TryAddTransient<IProjectRenderOperationMapper, ProjectRenderOperationMapper>();
        services.TryAddTransient<IProjectRenderOperationResultMapper, ProjectRenderOperationResultMapper>();
        services.TryAddTransient<IProjectRenderResponseMapper, ProjectRenderResponseMapper>();
        services.TryAddTransient<IProjectRenderResultMapper, ProjectRenderResultMapper>();
        services.TryAddTransient<IProjectSearchResultItemMapper, ProjectSearchResultItemMapper>();
        services.TryAddTransient<IProjectSearchResultMapper, ProjectSearchResultMapper>();
        services.TryAddTransient<IProjectsMigrationsMapper, ProjectsMigrationsMapper>();
        services.TryAddTransient<IProjectUpdateRequestMapper, ProjectUpdateRequestMapper>();
        services.TryAddTransient<IProjectUpdateResponseMapper, ProjectUpdateResponseMapper>();
        services.TryAddTransient<IPromptContentContractMapper, PromptContentContractMapper>();
        services.TryAddTransient<IPromptContentItemMapper, PromptContentItemMapper>();
        services.TryAddTransient<IPromptCreateResponseMapper, PromptCreateResponseMapper>();
        services.TryAddTransient<IRangeOfVideoMapper, RangeOfVideoMapper>();
        services.TryAddTransient<IRedactVideoRequestMapper, RedactVideoRequestMapper>();
        services.TryAddTransient<IRedactVideoResponseMapper, RedactVideoResponseMapper>();
        services.TryAddTransient<ISceneMapper, SceneMapper>();
        services.TryAddTransient<ISentiment1Mapper, Sentiment1Mapper>();
        services.TryAddTransient<ISentimentsMapper, SentimentsMapper>();
        services.TryAddTransient<IShotMapper, ShotMapper>();
        services.TryAddTransient<ISocialMapper, SocialMapper>();
        services.TryAddTransient<ISpeakerlongestmonolog1Mapper, Speakerlongestmonolog1Mapper>();
        services.TryAddTransient<ISpeakerLongestMonologMapper, SpeakerLongestMonologMapper>();
        services.TryAddTransient<ISpeakerMapper, SpeakerMapper>();
        services.TryAddTransient<ISpeakernumberoffragments1Mapper, Speakernumberoffragments1Mapper>();
        services.TryAddTransient<ISpeakerNumberOfFragmentsMapper, SpeakerNumberOfFragmentsMapper>();
        services.TryAddTransient<ISpeakertalktolistenratio1Mapper, Speakertalktolistenratio1Mapper>();
        services.TryAddTransient<ISpeakerTalkToListenRatioMapper, SpeakerTalkToListenRatioMapper>();
        services.TryAddTransient<ISpeakerwordcount1Mapper, Speakerwordcount1Mapper>();
        services.TryAddTransient<ISpeakerWordCountMapper, SpeakerWordCountMapper>();
        services.TryAddTransient<IStatistics1Mapper, Statistics1Mapper>();
        services.TryAddTransient<IStatisticsMapper, StatisticsMapper>();
        services.TryAddTransient<IStreamingUrlMapper, StreamingUrlMapper>();
        services.TryAddTransient<ISummarizedInsightsMapper, SummarizedInsightsMapper>();
        services.TryAddTransient<ITextualcontentmoderationMapper, TextualcontentmoderationMapper>();
        services.TryAddTransient<ITextVariationMapper, TextVariationMapper>();
        services.TryAddTransient<ITimeRangeMapper, TimeRangeMapper>();
        services.TryAddTransient<ITopic1Mapper, Topic1Mapper>();
        services.TryAddTransient<ITopicMapper, TopicMapper>();
        services.TryAddTransient<ITranscriptMapper, TranscriptMapper>();
        services.TryAddTransient<ITrialAccountMapper, TrialAccountMapper>();
        services.TryAddTransient<ITrialAccountQuotaUsageMapper, TrialAccountQuotaUsageMapper>();
        services.TryAddTransient<ITrialAccountStatisticsMapper, TrialAccountStatisticsMapper>();
        services.TryAddTransient<ITrialAccountWithTokenMapper, TrialAccountWithTokenMapper>();
        services.TryAddTransient<ITrialLimitedAccessFeaturesMapper, TrialLimitedAccessFeaturesMapper>();
        services.TryAddTransient<IUploadVideoResponseMapper, UploadVideoResponseMapper>();
        services.TryAddTransient<IVideoDetailsMapper, VideoDetailsMapper>();
        services.TryAddTransient<IVideoIndexResponseMapper, VideoIndexResponseMapper>();
        services.TryAddTransient<IVideoInsightsWidgetResponseMapper, VideoInsightsWidgetResponseMapper>();
        services.TryAddTransient<IVideoItemDataMapper, VideoItemDataMapper>();
        services.TryAddTransient<IVideoMigrationMapper, VideoMigrationMapper>();
        services.TryAddTransient<IVideoMigrationsListMapper, VideoMigrationsListMapper>();
        services.TryAddTransient<IVideoMigrationStateMapper, VideoMigrationStateMapper>();
        services.TryAddTransient<IVideoPlayerWidgetResponseMapper, VideoPlayerWidgetResponseMapper>();
        services.TryAddTransient<IVideoSearchMatchMapper, VideoSearchMatchMapper>();
        services.TryAddTransient<IVideoSearchResultItemMapper, VideoSearchResultItemMapper>();
        services.TryAddTransient<IVideoSearchResultMapper, VideoSearchResultMapper>();
        services.TryAddTransient<IVideosRangeMapper, VideosRangeMapper>();
        services.TryAddTransient<IVideoListDataModelMapper, VideoListDataModelMapper>();
        services.TryAddTransient<IVideoStrangeMapper, VideoStrangeMapper>();
        services.TryAddTransient<IVideoThumbnailFormatTypeMapper, VideoThumbnailFormatTypeMapper>();
        services.TryAddTransient<IVideoTimeRangeMapper, VideoTimeRangeMapper>();

        return services;
    }

    public static IServiceCollection AddVideoIndexerVideoRepository(this IServiceCollection services)
    {
        services.TryAddTransient<IAccountRepository, AccountRepository>();
        services.TryAddTransient<IAccountsRepository, AccountsRepository>();
        services.TryAddTransient<IBrandsRepository, BrandsRepository>();
        services.TryAddTransient<IClassicLanguageCustomizationRepository, ClassicLanguageCustomizationRepository>();
        services.TryAddTransient<ICustomLanguageRequestMapper, CustomLanguageRequestMapper>();
        services.TryAddTransient<ICreateLogoRepository, CreateLogoRepository>();
        services.TryAddTransient<IIndexingRepository, IndexingRepository>();
        services.TryAddTransient<IJobsRepository, JobsRepository>();
        services.TryAddTransient<IProjectsRepository, ProjectsRepository>();
        services.TryAddTransient<IPromptContentRepository, PromptContentRepository>();
        services.TryAddTransient<IRedactVideoRepository, RedactVideoRepository>();
        services.TryAddTransient<ITrialAccountAccessTokensRepository, TrialAccountAccessTokensRepository>();
        services.TryAddTransient<ITrialAccountsRepository, TrialAccountsRepository>();

        services.TryAddTransient<IVideoDataRepository, VideoDataRepository>();
        services.TryAddTransient<IVideoIndexRepository, VideoIndexRepository>();
        services.TryAddTransient<IVideoListRepository, VideoListRepository>();

        services.TryAddTransient<IVideosRepository, VideosRepository>();

        services.TryAddTransient<IWidgetRepository, WidgetRepository>();

        return services;
    }
}