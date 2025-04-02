using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using VideoIndexerAccess.Repositories.DataModel;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModelMapper
{
    public class VideoItemDataMapper : IVideoItemDataMapper
    {
        private readonly IVideoStrangeMapper _videoStrangeMapper;
        private readonly ISummarizedInsightsMapper _summarizedInsightsMapper;
        private readonly ISocialMapper _socialMapper;
        private readonly IItemVideoMapper _itemVideoMapper;

        public VideoItemDataMapper(IVideoStrangeMapper videoStrangeMapper, ISummarizedInsightsMapper summarizedInsightsMapper, ISocialMapper socialMapper, IItemVideoMapper itemVideoMapper)
        {
            _videoStrangeMapper = videoStrangeMapper;
            _summarizedInsightsMapper = summarizedInsightsMapper;
            _socialMapper = socialMapper;
            _itemVideoMapper = itemVideoMapper;
        }

        public VideoItemDataModel MapFrom(VideoItemApiModel model)
        {
            return new VideoItemDataModel
            {
                Id = model.id,
                AccountId = model.accountId,
                Partition = model.partition,
                Name = model.name,
                Description = model.description,
                PrivacyMode = model.privacyMode,
                State = model.state,
                UserName = model.userName,
                Created = model.created,
                IsOwned = model.isOwned,
                IsEditable = model.isEditable,
                IsBase = model.isBase,
                DurationInSeconds = model.durationInSeconds,
                Duration = model.duration,
                Social = _socialMapper.MapFrom(model.social),
                SummarizedInsights = _summarizedInsightsMapper.MapFrom(model.summarizedInsights),
                VideosRanges = model.videosRanges?.Select(_videoStrangeMapper.MapFrom).ToArray(),
                Videos = model.videos?.Select(_itemVideoMapper.MapFrom).ToArray(),
            };
        }
    }

    public class ItemVideoMapper : IItemVideoMapper
    {
        private readonly IInsightsMapper _insightsMapper;

        public ItemVideoMapper(IInsightsMapper insightsMapper)
        {
            _insightsMapper = insightsMapper;
        }

        public ItemVideo MapFrom(ItemVideoApiModel model)
        {
            return new ItemVideo
            {
                AccountId = model.accountId,
                Id = model.id,
                State = model.state,
                ModerationState = model.moderationState,
                ReviewState = model.reviewState,
                PrivacyMode = model.privacyMode,
                ProcessingProgress = model.processingProgress,
                FailureMessage = model.failureMessage,
                ExternalId = model.externalId,
                ExternalUrl = model.externalUrl,
                Metadata = model.metadata,
                Insights = _insightsMapper.MapFrom(model.insights),
                ThumbnailId = model.thumbnailId,
                Width = model.width,
                Height = model.height,
                DetectSourceLanguage = model.detectSourceLanguage,
                LanguageAutoDetectMode = model.languageAutoDetectMode,
                SourceLanguage = model.sourceLanguage,
                Language = model.language,
                SourceLanguages = model.sourceLanguages?.Select(language => language).ToArray(),
                IndexingPreset = model.indexingPreset,
                StreamingPreset = model.streamingPreset,
                LinguisticModelId = model.linguisticModelId,
                PersonModelId = model.personModelId,
                LogoGroupId = model.logoGroupId,
                IsAdult = model.isAdult,
                ExcludedAIs = model.excludedAIs?.ToArray(),
                IsSearchable = model.isSearchable,
                PublishedUrl = model.publishedUrl,
                PublishedProxyUrl = model.publishedProxyUrl,
                ViewToken = model.viewToken,
                Languages = model.languages?.Select(language => language).ToArray(),
            };
        }
    }

    public class InsightsMapper : IInsightsMapper
    {
        private readonly IBlockMapper _blockMapper;
        private readonly IKeyword1Mapper _keyword1Mapper;
        private readonly ILabel1Mapper _labelMapper;
        private readonly INamedLocationMapper _namedLocationMapper;
        private readonly IOcrMapper _ocrMapper;
        private readonly ITranscriptMapper _transcriptMapper;
        private readonly ITopic1Mapper _topic1Mapper;
        private readonly ISceneMapper _sceneMapper;
        private readonly IShotMapper _shotMapper;
        private readonly INamedPeopleMapper _namedPeopleMapper;
        private readonly ISentiment1Mapper _sentiment1Mapper;
        private readonly ISpeakerMapper _speakerMapper;
        private readonly ITextualcontentmoderationMapper _textualContentModerationMapper;
        private readonly IStatistics1Mapper _statistics1Mapper;

        public InsightsMapper(IBlockMapper blockMapper, IKeyword1Mapper keyword1Mapper, ILabel1Mapper labelMapper, INamedLocationMapper location, IOcrMapper ocrMapper, ITranscriptMapper transcriptMapper, ITopic1Mapper topic1Mapper, ISceneMapper sceneMapper, IShotMapper shotMapper, INamedPeopleMapper namedPeopleMapper, ISentiment1Mapper sentiment1Mapper, ISpeakerMapper speakerMapper, ITextualcontentmoderationMapper textualContentModerationMapper, IStatistics1Mapper statistics1Mapper)
        {
            _blockMapper = blockMapper;
            _keyword1Mapper = keyword1Mapper;
            _labelMapper = labelMapper;
            _namedLocationMapper = location;
            _ocrMapper = ocrMapper;
            _transcriptMapper = transcriptMapper;
            _topic1Mapper = topic1Mapper;
            _sceneMapper = sceneMapper;
            _shotMapper = shotMapper;
            _namedPeopleMapper = namedPeopleMapper;
            _sentiment1Mapper = sentiment1Mapper;
            _speakerMapper = speakerMapper;
            _textualContentModerationMapper = textualContentModerationMapper;
            _statistics1Mapper = statistics1Mapper;
        }

        public Insights? MapFrom(InsightsApiModel? model) => model is null ? null : new Insights
        {
            Version = model.version,
            Duration = model.duration,
            SourceLanguage = model.sourceLanguage,
            SourceLanguages = model.sourceLanguages?.Select(language => language).ToArray(),
            Language = model.language,
            Languages = model.languages?.ToArray(),
            Transcripts = model.transcript?.Select(_transcriptMapper.MapFrom).ToArray(),
            Ocr = model.ocr?.Select(_ocrMapper.MapFrom).ToArray(),
            Keywords = model.keywords?.Select(_keyword1Mapper.MapFrom).ToArray(),
            Topics = model.topics?.Select(_topic1Mapper.MapFrom).ToArray(),
            Labels = model.labels?.Select(_labelMapper.MapFrom).ToArray(),
            Scenes = model.scenes?.Select(_sceneMapper.MapFrom).ToArray(),
            Shots = model.shots?.Select(_shotMapper.MapFrom).ToArray(),
            NamedLocations = model.namedLocations?.Select(_namedLocationMapper.MapFrom).Where(location => location is not null).Select(location => location!).ToArray(),
            NamedPeople = model.namedPeople?.Select(_namedPeopleMapper.MapFrom).ToArray(),
            Sentiments = model.sentiments?.Select(_sentiment1Mapper.MapFrom).ToArray(),
            Blocks = model.blocks?.Select(_blockMapper.MapFrom).ToArray(),
            Speakers = model.speakers?.Select(_speakerMapper.MapFrom).ToArray(),
            TextualContentModeration =  model.textualContentModeration is null ? null : _textualContentModerationMapper.MapFrom(model.textualContentModeration),
            Statistics = model.statistics is null ? null : _statistics1Mapper.MapFrom(model.statistics),
        };
    }

    public class Statistics1Mapper : IStatistics1Mapper
    {
        private readonly ISpeakertalktolistenratio1Mapper _speakerTalkToListenRatio1Mapper;
        private readonly ISpeakerlongestmonolog1Mapper _speakerLongestMonolog1Mapper;
        private readonly ISpeakernumberoffragments1Mapper _speakerNumberOfFragments1Mapper;
        private readonly ISpeakerwordcount1Mapper _speakerWordCount1Mapper;

        public Statistics1Mapper(ISpeakertalktolistenratio1Mapper speakerTalkToListenRatio1Mapper, ISpeakerlongestmonolog1Mapper speakerLongestMonolog1Mapper, ISpeakernumberoffragments1Mapper speakerNumberOfFragments1Mapper, ISpeakerwordcount1Mapper speakerWordCount1Mapper)
        {
            _speakerTalkToListenRatio1Mapper = speakerTalkToListenRatio1Mapper;
            _speakerLongestMonolog1Mapper = speakerLongestMonolog1Mapper;
            _speakerNumberOfFragments1Mapper = speakerNumberOfFragments1Mapper;
            _speakerWordCount1Mapper = speakerWordCount1Mapper;
        }

        public Statistics1 MapFrom(Statistics1ApiModel model)
        {
            return new Statistics1()
            {
                CorrespondenceCount = model.correspondenceCount,
                SpeakerTalkToListenRatio = model.speakerTalkToListenRatio is null ? null : _speakerTalkToListenRatio1Mapper.MapFrom(model.speakerTalkToListenRatio),
                SpeakerLongestMonolog =  model.speakerLongestMonolog is null ? null : _speakerLongestMonolog1Mapper.MapFrom(model.speakerLongestMonolog),
                SpeakerNumberOfFragments = model.speakerNumberOfFragments is null ? null : _speakerNumberOfFragments1Mapper.MapFrom(model.speakerNumberOfFragments),
                SpeakerWordCount = model.speakerWordCount is null ? null : _speakerWordCount1Mapper.MapFrom(model.speakerWordCount),
            };
        }
    }

    public class Speakerwordcount1Mapper : ISpeakerwordcount1Mapper
    {
        public Speakerwordcount1 MapFrom(Speakerwordcount1ApiModel model)
        {
            return new Speakerwordcount1()
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class Speakernumberoffragments1Mapper : ISpeakernumberoffragments1Mapper
    {
        public Speakernumberoffragments1 MapFrom(Speakernumberoffragments1ApiModel model)
        {
            return new Speakernumberoffragments1()
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class Speakerlongestmonolog1Mapper : ISpeakerlongestmonolog1Mapper
    {
        public Speakerlongestmonolog1 MapFrom(Speakerlongestmonolog1ApiModel model)
        {
            return new Speakerlongestmonolog1()
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class Speakertalktolistenratio1Mapper : ISpeakertalktolistenratio1Mapper
    {
        public Speakertalktolistenratio1 MapFrom(Speakertalktolistenratio1ApiModel model)
        {
            return new Speakertalktolistenratio1()
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class TextualcontentmoderationMapper : ITextualcontentmoderationMapper
    {
        public Textualcontentmoderation MapFrom(TextualcontentModerationApiModel model)
        {
            return new Textualcontentmoderation()
            {
                Id = model.id,
                BannedWordsCount = model.bannedWordsCount,
                BannedWordsRatio = model.bannedWordsRatio,
                Instances = model.instances?.ToArray(),
            };
        }
    }

    public class Sentiment1Mapper : ISentiment1Mapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public Sentiment1Mapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Sentiment1 MapFrom(Sentiment1ApiModel model)
        {
            return new Sentiment1()
            {
                Id = model.id,
                AverageScore = model.averageScore,
                SentimentType = model.sentimentType,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class ShotMapper : IShotMapper
    {
        private readonly IKeyFrameMapper _keyFrameMapper;

        public ShotMapper(IKeyFrameMapper keyFrameMapper)
        {
            _keyFrameMapper = keyFrameMapper;
        }

        public Shot MapFrom(ShotApiModel model)
        {
            return new Shot()
            {
                Id = model.id,
                Tags = model.tags?.ToArray(),
                KeyFrames = model.keyFrames?.Select(_keyFrameMapper.MapFrom).ToArray(),
            };
        }
    }

    public class KeyFrameMapper : IKeyFrameMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public KeyFrameMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Keyframe MapFrom(KeyframeApiModel model)
        {
            return new Keyframe()
            {
                Id = model.id,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray()
            };
        }
    }

    public class SceneMapper : ISceneMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public SceneMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Scene MapFrom(SceneApiModel model)
        {
            return new Scene()
            {
                Id = model.id,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class Topic1Mapper : ITopic1Mapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public Topic1Mapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Topic1 MapFrom(Topic1ApiModel model)
        {
            return new Topic1()
            {
                Id = model.id,
                Name = model.name,
                ReferenceId = model.referenceId,
                ReferenceType = model.referenceType,
                IptcName = model.iptcName,
                Confidence = model.confidence,
                IabName = model.iabName,
                Language = model.language,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class TranscriptMapper : ITranscriptMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public TranscriptMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Transcript MapFrom(TranscriptApiModel model)
        {
            return new Transcript()
            {
                Id = model.id,
                Text = model.text,
                Confidence = model.confidence,
                SpeakerId = model.speakerId,
                Language = model.language,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class OcrMapper : IOcrMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public OcrMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Ocr MapFrom(OcrApiModel model)
        {
            return new Ocr()
            {
                Id = model.id,
                Text = model.text,
                Confidence = model.confidence,
                Left = model.left,
                Top = model.top,
                Width = model.width,
                Height = model.height,
                Angle = model.angle,
                Language = model.language,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class Label1Mapper : ILabel1Mapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public Label1Mapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Label1 MapFrom(Label1ApiModel model)
        {
            return new Label1()
            {
                Id = model.id,
                Name = model.name,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
                Language = model.language,
                ReferenceId = model.referenceId,
            };
        }
    }

    public class Keyword1Mapper : IKeyword1Mapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public Keyword1Mapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Keyword1 MapFrom(Keyword1ApiModel model)
        {
            return new Keyword1()
            {
                Confidence = model.confidence,
                Id = model.id,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
                Language = model.language,
                Text = model.text,
            };
        }
    }


    public class BlockMapper : IBlockMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public BlockMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Block MapFrom(BlockApiModel model)
        {
            return new Block()
            {
                Id = model.id,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class SocialMapper : ISocialMapper
    {
        public Social? MapFrom(SocialApiModel? model)
        {
            return model is null ? null : new Social
            {
                LikedByUser = model.likedByUser,
                Likes = model.likes,
                Views = model.views,
            };
        }
    }

    public class StatisticsMapper : IStatisticsMapper
    {
        private readonly ISpeakerTalkToListenRatioMapper _speakerTalkToListenRatioMapper;
        private readonly ISpeakerLongestMonologMapper _speakerLongestMonologMapper;
        private readonly ISpeakerNumberOfFragmentsMapper _speakerNumberOfFragmentsMapper;
        private readonly ISpeakerWordCountMapper _speakerWordCountMapper;

        public StatisticsMapper(ISpeakerTalkToListenRatioMapper speakerTalkToListenRatioMapper, ISpeakerLongestMonologMapper speakerLongestMonologMapper, ISpeakerNumberOfFragmentsMapper speakerNumberOfFragmentsMapper, ISpeakerWordCountMapper speakerWordCountMapper)
        {
            _speakerTalkToListenRatioMapper = speakerTalkToListenRatioMapper;
            _speakerLongestMonologMapper = speakerLongestMonologMapper;
            _speakerNumberOfFragmentsMapper = speakerNumberOfFragmentsMapper;
            _speakerWordCountMapper = speakerWordCountMapper;
        }

        public Statistics? MapFrom(StatisticsApiModel? model)
        {
            return model is null ? null :  new Statistics
            {
                CorrespondenceCount = model.correspondenceCount,
                SpeakerTalkToListenRatio = _speakerTalkToListenRatioMapper.MapFrom(model.speakerTalkToListenRatio),
                SpeakerLongestMonolog = _speakerLongestMonologMapper.MapFrom(model.speakerLongestMonolog),
                SpeakerNumberOfFragments = _speakerNumberOfFragmentsMapper.MapFrom(model.speakerNumberOfFragments),
                SpeakerWordCount = _speakerWordCountMapper.MapFrom(model!.speakerWordCount),
            };
        }
    }

    public class SpeakerWordCountMapper : ISpeakerWordCountMapper
    {
        public Speakerwordcount? MapFrom(SpeakerwordcountApiModel? model)
        {
            return model is null ? null : new Speakerwordcount
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class SpeakerNumberOfFragmentsMapper : ISpeakerNumberOfFragmentsMapper
    {
        public Speakernumberoffragments? MapFrom(SpeakernumberoffragmentsApiModel? model)
        {
            return model is null ? null : new Speakernumberoffragments
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }
    }

    public class SpeakerLongestMonologMapper : ISpeakerLongestMonologMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public SpeakerLongestMonologMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Speakerlongestmonolog? MapFrom(SpeakerlongestmonologApiModel? model)
        {
            return model is null ? null : new Speakerlongestmonolog
            {
                _1 = model._1,
                _2 = model._2,
                _3 = model._3,
            };
        }

    }

    public class SpeakerTalkToListenRatioMapper : ISpeakerTalkToListenRatioMapper
    {
        public Speakertalktolistenratio? MapFrom(SpeakertalktolistenratioApiModel? model)
        {
            return model is null ? null : new Speakertalktolistenratio
            {
                _1 = model?._1,
                _2 = model?._2,
                _3 = model?._3,
            };
        }
    }

    public class SummarizedInsightsMapper : ISummarizedInsightsMapper
    {
        private readonly IAudioEffectsMapper _audioEffectsMapper;
        private readonly IDurationMapper _durationMapper;
        private readonly IBrandMapper _brandMapper;
        private readonly IFaceMapper _faceMapper;
        private readonly IKeyWordMapper _keyWordMapper;
        private readonly ISentimentsMapper _sentimentsMapper;
        private readonly INamedLocationMapper _namedLocationMapper;
        private readonly INamedPeopleMapper _namedPeopleMapper;
        private readonly ITopicMapper _topicMapper;
        private readonly IEmotionsMapper _emotionsMapper;
        private readonly IFramePatternsMapper _framePatternsMapper;
        private readonly IStatisticsMapper _statisticsMapper;
        private readonly ILabelMapper _labelMapper;

        public SummarizedInsightsMapper(IAudioEffectsMapper audioEffectsMapper, IDurationMapper durationMapper, IBrandMapper brandMapper, IFaceMapper faceMapper, IKeyWordMapper keyWordMapper, ISentimentsMapper sentimentsMapper, INamedLocationMapper namedLocationMapper, INamedPeopleMapper namedPeopleMapper, ITopicMapper topicMapper, IEmotionsMapper emotionsMapper, IFramePatternsMapper framePatternsMapper, IStatisticsMapper statisticsMapper, ILabelMapper labelMapper)
        {
            _audioEffectsMapper = audioEffectsMapper;
            _durationMapper = durationMapper;
            _brandMapper = brandMapper;
            _faceMapper = faceMapper;
            _keyWordMapper = keyWordMapper;
            _sentimentsMapper = sentimentsMapper;
            _namedLocationMapper = namedLocationMapper;
            _namedPeopleMapper = namedPeopleMapper;
            _topicMapper = topicMapper;
            _emotionsMapper = emotionsMapper;
            _framePatternsMapper = framePatternsMapper;
            _statisticsMapper = statisticsMapper;
            _labelMapper = labelMapper;
        }

        public Summarizedinsights? MapFrom(SummarizedinsightsApiModel? model)
        {

            return model is null ? null : new Summarizedinsights
            {
                Name = model.name,
                Duration = _durationMapper.MapFrom(model.duration),
                ThumbnailVideoId = model.thumbnailVideoId,
                ThumbnailId = model.thumbnailId,
                AudioEffects = model.audioEffects?.Select(_audioEffectsMapper.MapFrom).ToArray(),
                Labels = model.labels?.Select(_labelMapper.MapFrom).ToArray(),
                Brands = model.brands?.Select(_brandMapper.MapFrom).ToArray(),
                Faces = model.faces?.Select(_faceMapper.MapFrom).ToArray(),
                Keywords = model.keywords?.Select(_keyWordMapper.MapFrom).ToArray(),
                Sentiments = model.sentiments?.Select(_sentimentsMapper.MapFrom).ToArray(),
                NamedLocations = model.namedLocations?.Select(_namedLocationMapper.MapFrom).Where(location => location is not null).ToArray(),
                NamedPeople = model.namedPeople?.Select(_namedPeopleMapper.MapFrom).ToArray(),
                Topics = model.topics?.Select(_topicMapper.MapFrom).ToArray(),
                Emotions = model.emotions?.Select(_emotionsMapper.MapFrom).ToArray(),
                FramePatterns = model.framePatterns?.Select(_framePatternsMapper.MapFrom).ToArray(),
                Statistics = _statisticsMapper.MapFrom(model.statistics),
                Id = model.id,
                PrivacyMode = model.privacyMode,
            };
        }
    }

    public class LabelMapper : ILabelMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public LabelMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public Label MapFrom(LabelApiModel model)
        {
            return new Label
            {
                Id = model.id,
                Name = model.name,
                Appearances = model.appearances?.Where(appearance=> appearance is not null).Select(_appearanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class TopicMapper : ITopicMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public TopicMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public Topic MapFrom(TopicApiModel model)
        {
            return new Topic
            {
                Id = model.id,
                Name = model.name,
                Confidence = model.confidence,
                IabName = model.iabName,
                ReferenceUrl = model.referenceUrl,
                Appearances = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
                IptcName = model.iptcName,
            };
        }
    }

    public class SentimentsMapper : ISentimentsMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public SentimentsMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public Sentiment MapFrom(SentimentApiModel model)
        {
            return new Sentiment
            {
                Appearances  = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
                SeenDurationRatio = model.seenDurationRatio,
                SentimentKey = model.sentimentKey,
            };
        }
    }

    public class KeyWordMapper : IKeyWordMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public KeyWordMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public Keyword MapFrom(KeywordApiModel model)
        {
            return new Keyword
            {
                IsTranscript = model.isTranscript,
                Id = model.id,
                Name = model.name,
                Appearances = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
            };
        }
    }

    public class FaceMapper : IFaceMapper
    {
        public Face MapFrom(FaceApiModel model)
        {
            return new Face
            {
                AppearanceCount = model.appearanceCount,
                Name = model.name,
            };
        }
    }

    public class DurationMapper : IDurationMapper
    {
        public Duration? MapFrom(DurationApiModel? model)
        {
            return model is null ? null : new Duration
            {
                Time = model.time,
                Seconds = model.seconds,
            };
        }
    }

    public class AudioEffectsMapper : IAudioEffectsMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public AudioEffectsMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public AudioEffects MapFrom(AudioEffectsApiModel model)
        {
            return new AudioEffects
            {
                Appearances = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
                AudioEffectKey = model.audioEffectKey,
                SeenDuration = model.seenDuration,
                SeenDurationRatio = model.seenDurationRatio,
            };
        }
    }

    public class VideoStrangeMapper : IVideoStrangeMapper
    {
        private readonly IRangeOfVideoMapper _rangeOfVideoMapper;

        public VideoStrangeMapper(IRangeOfVideoMapper rangeOfVideoMapper)
        {
            _rangeOfVideoMapper = rangeOfVideoMapper;
        }

        public Videosrange MapFrom(VideosrangeApiModel? model)
        {
            return new Videosrange
            {
                VideoId = model?.videoId,
                Range = _rangeOfVideoMapper.MapFrom(model?.range),
            };
        }
    }

    public class RangeOfVideoMapper : IRangeOfVideoMapper
    {
        public RangeOfVideo MapFrom(RangeApiModel? model)
        {
            return new RangeOfVideo
            {
                Start = model?.start,
                End = model?.end,
            };
        }
    }

    public class NamedLocationMapper : INamedLocationMapper
    {
        private readonly IInstanceMapper _instanceMapper;
        private readonly IAppearanceMapper _appearanceMapper;

        public NamedLocationMapper(IInstanceMapper instanceMapper, IAppearanceMapper appearanceMapper)
        {
            _instanceMapper = instanceMapper;
            _appearanceMapper = appearanceMapper;
        }

        public NamedLocation? MapFrom(NamedLocationApiModel? model)
        {
            return model is null ? null : new NamedLocation
            {
                Id = model.id,
                Name = model.name,
                ReferenceId = model.referenceId,
                ReferenceUrl = model.referenceUrl,
                Description = model.description,
                Tags = model.tags?.Where(tag=>tag is not null).Select(tag => tag).ToArray(),
                Confidence = model.confidence,
                IsCustom = model.isCustom,
                Instances = model.instances is null || model.instances.Length == 0 ? [] : model.instances.Select(_instanceMapper.MapFrom).ToArray(),
                Appearances = model.appearances?.Where(appearance => appearance is not null).Select(_appearanceMapper.MapFrom).ToArray(),
                SeenDuration = model.seenDuration,
            };
        }
    }

    public class AppearanceMapper : IAppearanceMapper
    {
        public Appearance MapFrom(AppearanceApiModel model)
        {
            return new Appearance
            {
                StartTime = model.startTime,
                EndTime = model.endTime,
                StartSeconds = model.startSeconds,
                EndSeconds = model.endSeconds,
                Confidence = model.confidence,
            };
        }
    }

    public class NamedPeopleMapper : INamedPeopleMapper
    {
        private readonly IInstanceMapper _instanceMapper;
        private readonly IAppearanceMapper _appearanceMapper;

        public NamedPeopleMapper(IInstanceMapper instanceMapper, IAppearanceMapper appearanceMapper)
        {
            _instanceMapper = instanceMapper;
            _appearanceMapper = appearanceMapper;
        }

        public NamedPeople MapFrom(NamedPeopleApiModel model)
        {
            return new NamedPeople
            {
                Id = model.id,
                Name = model.name,
                ReferenceId = model.referenceId,
                ReferenceUrl = model.referenceUrl,
                Description = model.description,
                Tags = model.tags?.Where(tag=>tag is not null).Select(tag => tag).ToArray(),
                Confidence = model.confidence,
                IsCustom = model.isCustom,
                Instances = model.instances?.Where(instance=> instance is not null).Select(_instanceMapper.MapFrom).ToArray(),
                Appearances = model.appearances?.Where(appearance=> appearance is not null).Select(_appearanceMapper.MapFrom).ToArray(),
                SeenDuration = model.seenDuration,
            };
        }
    }

    public class SpeakerMapper : ISpeakerMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public SpeakerMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Speaker MapFrom(SpeakerApiModel model)
        {
            return new Speaker
            {
                Id = model.id,
                Name = model.name ?? String.Empty,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray() ?? [],
            };
        }
    }

public class VideosRangeMapper : IVideosRangeMapper
    {
        private readonly IRangeOfVideoMapper _rangeOfVideoMapper;

        public VideosRangeMapper(IRangeOfVideoMapper rangeOfVideoMapper)
        {
            _rangeOfVideoMapper = rangeOfVideoMapper;
        }

        public Videosrange MapFrom(VideosrangeApiModel model)
        {
            return new Videosrange
            {
                VideoId = model.videoId,
                Range = _rangeOfVideoMapper.MapFrom(model.range),
            };
        }
    }

    //public class RangeOfVideoMapper : IRangeOfVideoMapper
    //{
    //    public RangeOfVideo MapFrom(RangeApiModel model)
    //    {
    //        return new RangeOfVideo
    //        {
    //            start = model.start,
    //            end = model.end,
    //        };
    //    }
    //}

    //public class AudioEffectsMapper : IAudioEffectsMapper
    //{
    //    private readonly IAppearanceMapper _appearanceMapper;

    //    public AudioEffectsMapper(IAppearanceMapper appearanceMapper)
    //    {
    //        _appearanceMapper = appearanceMapper;
    //    }

    //    public AudioEffects MapFrom(AudioEffectsApiModel model)
    //    {
    //        return new AudioEffects
    //        {
    //            appearances = model.appearances.Select(_appearanceMapper.MapFrom).ToArray(),
    //            audioEffectKey = model.audioEffectKey,
    //            seenDuration = model.seenDuration,
    //            seenDurationRatio = model.seenDurationRatio,
    //        };
    //    }
    //}

    public class EmotionsMapper : IEmotionsMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public EmotionsMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public Emotions MapFrom(EmotionsApiModel model)
        {
            return new Emotions
            {
                Appearances = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
                SeenDurationRatio = model.seenDurationRatio,
                Type = model.type,
            };
        }
    }


    public class FramePatternsMapper : IFramePatternsMapper
    {
        private readonly IAppearanceMapper _appearanceMapper;

        public FramePatternsMapper(IAppearanceMapper appearanceMapper)
        {
            _appearanceMapper = appearanceMapper;
        }

        public FramePatterns MapFrom(FramePatternsApiModel model)
        {
            return new FramePatterns
            {
                Id = model.id,
                Name = model.name,
                Appearances = model.appearances?.Select(_appearanceMapper.MapFrom).ToArray(),
            };
        }
    }

    //public class AppearanceMapper : IAppearanceMapper
    //{
    //    public Appearance MapFrom(AppearanceApiModel model)
    //    {
    //        return new Appearance
    //        {
    //            startTime = model.startTime,
    //            endTime = model.endTime,
    //            startSeconds = model.startSeconds,
    //            endSeconds = model.endSeconds,
    //            confidence = model.confidence,
    //        };
    //    }
    //}

    public class BrandMapper : IBrandMapper
    {
        private readonly IInstanceMapper _instanceMapper;

        public BrandMapper(IInstanceMapper instanceMapper)
        {
            _instanceMapper = instanceMapper;
        }

        public Brand MapFrom(BrandApiModel model)
        {
            return new Brand
            {
                Id = model.id,
                Name = model.name,
                Description = model.description,
                Tags = model.tags,
                Confidence = model.confidence,
                Instances = model.instances?.Select(_instanceMapper.MapFrom).ToArray(),
                IsCustom = model.isCustom,
                ReferenceId = model.referenceId,
                ReferenceType = model.referenceType,
                ReferenceUrl = model.referenceUrl,
            };
        }

    }

    public class InstanceMapper : IInstanceMapper
    {
        public Instance MapFrom(InstanceApiModel model)
        {
            return new Instance
            {
                Confidence = model.confidence,
                Start = model.start,
                End = model.end,
                AdjustedEnd = model.adjustedEnd,
                AdjustedStart = model.adjustedStart,
                BrandType = model.brandType,
                InstanceSource = model.instanceSource,
                ThumbnailId = model.thumbnailId,
            };
        }
    }
}



