namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel
{
    public class VideoItemApiModel
    {
        public string? partition { get; set; }
        public string? description { get; set; }
        public string? privacyMode { get; set; }
        public string? state { get; set; }
        public string? accountId { get; set; }
        public string? id { get; set; }
        public string? name { get; set; }
        public string? userName { get; set; }
        public DateTime? created { get; set; }
        public bool? isOwned { get; set; }
        public bool? isEditable { get; set; }
        public bool? isBase { get; set; }
        public decimal? durationInSeconds { get; set; }
        public string? duration { get; set; }
        public SummarizedinsightsApiModel? summarizedInsights { get; set; }
        public ItemVideoApiModel[]? videos { get; set; }
        public VideosrangeApiModel[]? videosRanges { get; set; }
        public SocialApiModel? social { get; set; }
    }

    public class SummarizedinsightsApiModel
    {
        public string? name { get; set; }
        public string? id { get; set; }
        public string? privacyMode { get; set; }
        public DurationApiModel? duration { get; set; }
        public string? thumbnailVideoId { get; set; }
        public string? thumbnailId { get; set; }
        public FaceApiModel[]? faces { get; set; }
        public KeywordApiModel[]? keywords { get; set; }
        public SentimentApiModel[]? sentiments { get; set; }
        public EmotionsApiModel[]? emotions { get; set; }
        public AudioEffectsApiModel[]? audioEffects { get; set; }
        public LabelApiModel[]? labels { get; set; }
        public FramePatternsApiModel[]? framePatterns { get; set; }
        public BrandApiModel[]? brands { get; set; }
        public NamedLocationApiModel[]? namedLocations { get; set; }
        public NamedPeopleApiModel[]? namedPeople { get; set; }
        public StatisticsApiModel? statistics { get; set; }
        public TopicApiModel[]? topics { get; set; }
    }

    public class FaceApiModel
    {
        public string? Name { get; set; }
        public int? AppearanceCount { get; set; }
    }

    public class DurationApiModel
    {
        public string? time { get; set; }
        public decimal? seconds { get; set; }
    }

    public class StatisticsApiModel
    {
        public int? correspondenceCount { get; set; }
        public SpeakertalktolistenratioApiModel? speakerTalkToListenRatio { get; set; }
        public SpeakerlongestmonologApiModel? speakerLongestMonolog { get; set; }
        public SpeakernumberoffragmentsApiModel? speakerNumberOfFragments { get; set; }
        public SpeakerwordcountApiModel? speakerWordCount { get; set; }
    }

    public class SpeakertalktolistenratioApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class SpeakerlongestmonologApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class SpeakernumberoffragmentsApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class SpeakerwordcountApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class KeywordApiModel
    {
        public bool? isTranscript { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }

    public class AppearanceApiModel
    {
        public decimal? confidence { get; set; }
        public string? startTime { get; set; }
        public string? endTime { get; set; }
        public decimal? startSeconds { get; set; }
        public decimal? endSeconds { get; set; }
    }

    public class SentimentApiModel
    {
        public string? sentimentKey { get; set; }
        public decimal? seenDurationRatio { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }
    public class Sentiment1ApiModel
    {
        public int? id { get; set; }
        public decimal? averageScore { get; set; }
        public string? sentimentType { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }
    //public class Appearance1
    //{
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public int startSeconds { get; set; }
    //    public decimal endSeconds { get; set; }
    //}

    public class LabelApiModel
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }

    //public class Appearance2
    //{
    //    public decimal confidence { get; set; }
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public int startSeconds { get; set; }
    //    public int endSeconds { get; set; }
    //}

    public class NamedLocationApiModel
    {
        public string? referenceId { get; set; }
        public string? referenceUrl { get; set; }
        public decimal? confidence { get; set; }
        public string? description { get; set; }
        public decimal? seenDuration { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
        public string[]? tags { get; set; }
        public bool? isCustom { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Appearance3
    //{
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public decimal startSeconds { get; set; }
    //    public decimal endSeconds { get; set; }
    //}

    public class NamedPeopleApiModel
    {
        public string? referenceId { get; set; }
        public string? referenceUrl { get; set; }
        public decimal? confidence { get; set; }
        public string? description { get; set; }
        public decimal? seenDuration { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
        public string[]? tags { get; set; }
        public bool? isCustom { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Appearance4
    //{
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public decimal startSeconds { get; set; }
    //    public decimal endSeconds { get; set; }
    //}

    public class TopicApiModel
    {
        public string? referenceUrl { get; set; }
        public string? iptcName { get; set; }
        public string? iabName { get; set; }
        public decimal? confidence { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }

    //public class Appearance5
    //{
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public decimal startSeconds { get; set; }
    //    public decimal endSeconds { get; set; }
    //}

    public class ItemVideoApiModel
    {
        public string? accountId { get; set; }
        public string? id { get; set; }
        public string? state { get; set; }
        public string? moderationState { get; set; }
        public string? reviewState { get; set; }
        public string? privacyMode { get; set; }
        public string? processingProgress { get; set; }
        public string? failureMessage { get; set; }
        public string? externalId { get; set; }
        public string? externalUrl { get; set; }
        public string? metadata { get; set; }
        public InsightsApiModel? insights { get; set; }
        public string? thumbnailId { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public bool? detectSourceLanguage { get; set; }
        public string? languageAutoDetectMode { get; set; }
        public string? sourceLanguage { get; set; }
        public string[]? sourceLanguages { get; set; }
        public string? language { get; set; }
        public string[]? languages { get; set; }
        public string? indexingPreset { get; set; }
        public string? streamingPreset { get; set; }
        public string? linguisticModelId { get; set; }
        public string? personModelId { get; set; }
        public string? logoGroupId { get; set; }
        public bool? isAdult { get; set; }
        public string[]? excludedAIs { get; set; }
        public bool? isSearchable { get; set; }
        public string? publishedUrl { get; set; }
        public string? publishedProxyUrl { get; set; }
        public string? viewToken { get; set; }
    }

    public class InsightsApiModel
    {
        public string? version { get; set; }
        public string? duration { get; set; }
        public string? sourceLanguage { get; set; }
        public string[]? sourceLanguages { get; set; }
        public string? language { get; set; }
        public string[]? languages { get; set; }
        public TranscriptApiModel[]? transcript { get; set; }
        public OcrApiModel[]? ocr { get; set; }
        public Keyword1ApiModel[]? keywords { get; set; }
        public Topic1ApiModel[]? topics { get; set; }
        public Label1ApiModel[]? labels { get; set; }
        public SceneApiModel[]? scenes { get; set; }
        public ShotApiModel[]? shots { get; set; }
        public NamedLocationApiModel[]? namedLocations { get; set; }
        public NamedPeopleApiModel[]? namedPeople { get; set; }
        public Sentiment1ApiModel[]? sentiments { get; set; }
        public BlockApiModel[]? blocks { get; set; }
        public SpeakerApiModel[]? speakers { get; set; }
        public TextualcontentModerationApiModel? textualContentModeration { get; set; }
        public Statistics1ApiModel? statistics { get; set; }
    }

    public class TextualcontentModerationApiModel
    {
        public int? id { get; set; }
        public int? bannedWordsCount { get; set; }
        public decimal? bannedWordsRatio { get; set; }
        public string[]? instances { get; set; }
    }

    public class Statistics1ApiModel
    {
        public int? correspondenceCount { get; set; }
        public Speakertalktolistenratio1ApiModel? speakerTalkToListenRatio { get; set; }
        public Speakerlongestmonolog1ApiModel? speakerLongestMonolog { get; set; }
        public Speakernumberoffragments1ApiModel? speakerNumberOfFragments { get; set; }
        public Speakerwordcount1ApiModel? speakerWordCount { get; set; }
    }

    public class Speakertalktolistenratio1ApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerlongestmonolog1ApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakernumberoffragments1ApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerwordcount1ApiModel
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class TranscriptApiModel
    {
        public int? id { get; set; }
        public string? text { get; set; }
        public decimal? confidence { get; set; }
        public int? speakerId { get; set; }
        public string? language { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    public class InstanceApiModel
    {
        public decimal? confidence { get; set; }
        public string? thumbnailId { get; set; }
        public string? instanceSource { get; set; }
        public string? brandType { get; set; }
        public string? adjustedStart { get; set; }
        public string? adjustedEnd { get; set; }
        public string? start { get; set; }
        public string? end { get; set; }
    }



    public class OcrApiModel
    {
        public int? id { get; set; }
        public string? text { get; set; }
        public decimal? confidence { get; set; }
        public decimal? left { get; set; }
        public decimal? top { get; set; }
        public decimal? width { get; set; }
        public decimal? height { get; set; }
        public decimal? angle { get; set; }
        public string? language { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    public class SocialApiModel
    {
        public bool? likedByUser { get; set; }
        public decimal? likes { get; set; }
        public decimal? views { get; set; }
    }

    //public class Sentiment1
    //{
    //    public int id { get; set; }
    //    public int score { get; set; }
    //    public int sentimentType { get; set; }
    //    public Instance[] instances { get; set; }
    //}


    //public class Instance1
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class Keyword1ApiModel
    {
        public int? id { get; set; }
        public string? text { get; set; }
        public decimal? confidence { get; set; }
        public string? language { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    public class BlockApiModel
    {
        public int? id { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Block
    //{
    //    public int id { get; set; }
    //    public Instance[] instances { get; set; }
    //}

    //public class Instance2
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class Topic1ApiModel
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? referenceId { get; set; }
        public string? referenceType { get; set; }
        public string? iptcName { get; set; }
        public decimal? confidence { get; set; }
        public string? iabName { get; set; }
        public string? language { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Instance3
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class Label1ApiModel
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public string? referenceId { get; set; }
        public string? language { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Instance4
    //{
    //    public decimal confidence { get; set; }
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class SceneApiModel
    {
        public int? id { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Instance5
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class ShotApiModel
    {
        public int? id { get; set; }
        public string[]? tags { get; set; }
        public KeyframeApiModel[]? keyFrames { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    public class KeyframeApiModel
    {
        public int? id { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Instance6
    //{
    //    public string thumbnailId { get; set; }
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    //public class Instance7
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    //public class Namedlocation1ApiModel
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string referenceId { get; set; }
    //    public string referenceUrl { get; set; }
    //    public string description { get; set; }
    //    public string[] tags { get; set; }
    //    public decimal confidence { get; set; }
    //    public bool isCustom { get; set; }
    //    public InstanceApiModel[] instances { get; set; }
    //}

    //public class Instance8
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    //public class NamedPeople1ApiModel
    //{
    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public string referenceId { get; set; }
    //    public string referenceUrl { get; set; }
    //    public string description { get; set; }
    //    public string[] tags { get; set; }
    //    public decimal confidence { get; set; }
    //    public bool isCustom { get; set; }
    //    public InstanceApiModel[] instances { get; set; }
    //}

    //public class Instance9
    //{
    //    public string instanceSource { get; set; }
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    //public class Sentiment1
    //{
    //    public int id { get; set; }
    //    public decimal averageScore { get; set; }
    //    public string sentimentType { get; set; }
    //    public Instance[] instances { get; set; }
    //}

    //public class Instance10
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}



    //public class Instance11
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class SpeakerApiModel
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }

    //public class Instance12
    //{
    //    public string adjustedStart { get; set; }
    //    public string adjustedEnd { get; set; }
    //    public string start { get; set; }
    //    public string end { get; set; }
    //}

    public class VideosrangeApiModel
    {
        public string? videoId { get; set; }
        public RangeApiModel? range { get; set; }
    }

    public class RangeApiModel
    {
        public string? start { get; set; }
        public string? end { get; set; }
    }

    public class EmotionsApiModel
    {
        public string? type { get; set; }
        public decimal? seenDurationRatio { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }


    public class AudioEffectsApiModel
    {
        public string? audioEffectKey { get; set; }
        public decimal? seenDurationRatio { get; set; }
        public decimal? seenDuration { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }


    public class FramePatternsApiModel
    {
        public string? displayName { get; set; }
        public int? id { get; set; }
        public string? name { get; set; }
        public AppearanceApiModel[]? appearances { get; set; }
    }



    public class BrandApiModel
    {
        public int? id { get; set; }
        public string? referenceType { get; set; }
        public string? name { get; set; }
        public string? referenceId { get; set; }
        public string? referenceUrl { get; set; }
        public string? description { get; set; }
        public object[]? tags { get; set; }
        public decimal? confidence { get; set; }
        public bool? isCustom { get; set; }
        public InstanceApiModel[]? instances { get; set; }
    }





}