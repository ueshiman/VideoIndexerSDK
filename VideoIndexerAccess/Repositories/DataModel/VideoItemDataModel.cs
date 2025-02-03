using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class VideoItemDataModel
    {
        public string? Partition { get; set; }
        public string? Description { get; set; }
        public string? PrivacyMode { get; set; }
        public string? State { get; set; }
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? UserName { get; set; }
        public DateTime? Created { get; set; }
        public bool? IsOwned { get; set; }
        public bool? IsEditable { get; set; }
        public bool? IsBase { get; set; }
        public decimal? DurationInSeconds { get; set; }
        public string? Duration { get; set; }
        public Summarizedinsights? SummarizedInsights { get; set; }
        public ItemVideo[]? Videos { get; set; }
        public Videosrange[]? VideosRanges { get; set; }
        public Social? Social { get; set; }
    }

    public class Summarizedinsights
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? PrivacyMode { get; set; }
        public Duration? Duration { get; set; }
        public string? ThumbnailVideoId { get; set; }
        public string? ThumbnailId { get; set; }
        public Face[]? Faces { get; set; }
        public Keyword[]? Keywords { get; set; }
        public Sentiment[]? Sentiments { get; set; }
        public Emotions[]? Emotions { get; set; }
        public AudioEffects[]? AudioEffects { get; set; }
        public Label[]? Labels { get; set; }
        public FramePatterns[]? FramePatterns { get; set; }
        public Brand[]? Brands { get; set; }
        public NamedLocation?[]? NamedLocations { get; set; }
        public NamedPeople[]? NamedPeople { get; set; }
        public Statistics? Statistics { get; set; }
        public Topic[]? Topics { get; set; }
    }

    public class Face
    {
        public string? Name { get; set; }
        public int? AppearanceCount { get; set; }
    }

    public class Duration
    {
        public string? Time { get; set; }
        public decimal? Seconds { get; set; }
    }

    public class Statistics
    {
        public int? CorrespondenceCount { get; set; }
        public Speakertalktolistenratio? SpeakerTalkToListenRatio { get; set; }
        public Speakerlongestmonolog? SpeakerLongestMonolog { get; set; }
        public Speakernumberoffragments? SpeakerNumberOfFragments { get; set; }
        public Speakerwordcount? SpeakerWordCount { get; set; }
    }

    public class Speakertalktolistenratio
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerlongestmonolog
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakernumberoffragments
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerwordcount
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Keyword
    {
        public bool? IsTranscript { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
    }

    public class Appearance
    {
        public decimal? Confidence { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public decimal? StartSeconds { get; set; }
        public decimal? EndSeconds { get; set; }
    }

    public class Sentiment
    {
        public string? SentimentKey { get; set; }
        public decimal? SeenDurationRatio { get; set; }
        public Appearance[]? Appearances { get; set; }
    }
    public class Sentiment1
    {
        public int? Id { get; set; }
        public decimal? AverageScore { get; set; }
        public string? SentimentType { get; set; }
        public Instance[]? Instances { get; set; }
    }
        //public class Appearance1
        //{
        //    public string startTime { get; set; }
        //    public string endTime { get; set; }
        //    public int startSeconds { get; set; }
        //    public decimal endSeconds { get; set; }
        //}

        public class Label
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
    }

        //public class Appearance2
        //{
        //    public decimal confidence { get; set; }
        //    public string startTime { get; set; }
        //    public string endTime { get; set; }
        //    public int startSeconds { get; set; }
        //    public int endSeconds { get; set; }
        //}

        public class NamedLocation
    {
        public string? ReferenceId { get; set; }
        public string? ReferenceUrl { get; set; }
        public decimal? Confidence { get; set; }
        public string? Description { get; set; }
        public decimal? SeenDuration { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
        public string[]? Tags { get; set; }
        public bool? IsCustom { get; set; }
        public Instance[]? Instances { get; set; }
    }

        //public class Appearance3
        //{
        //    public string startTime { get; set; }
        //    public string endTime { get; set; }
        //    public decimal startSeconds { get; set; }
        //    public decimal endSeconds { get; set; }
        //}

        public class NamedPeople
    {
        public string? ReferenceId { get; set; }
        public string? ReferenceUrl { get; set; }
        public decimal? Confidence { get; set; }
        public string? Description { get; set; }
        public decimal? SeenDuration { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
        public string[]? Tags { get; set; }
        public bool? IsCustom { get; set; }
        public Instance[]? Instances { get; set; }
    }

        //public class Appearance4
        //{
        //    public string startTime { get; set; }
        //    public string endTime { get; set; }
        //    public decimal startSeconds { get; set; }
        //    public decimal endSeconds { get; set; }
        //}

        public class Topic
    {
        public string? ReferenceUrl { get; set; }
        public string? IptcName { get; set; }
        public string? IabName { get; set; }
        public decimal? Confidence { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
    }

        //public class Appearance5
        //{
        //    public string startTime { get; set; }
        //    public string endTime { get; set; }
        //    public decimal startSeconds { get; set; }
        //    public decimal endSeconds { get; set; }
        //}

        public class ItemVideo
    {
        public string? AccountId { get; set; }
        public string? Id { get; set; }
        public string? State { get; set; }
        public string? ModerationState { get; set; }
        public string? ReviewState { get; set; }
        public string? PrivacyMode { get; set; }
        public string? ProcessingProgress { get; set; }
        public string? FailureMessage { get; set; }
        public string? ExternalId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Metadata { get; set; }
        public Insights? Insights { get; set; }
        public string? ThumbnailId { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public bool? DetectSourceLanguage { get; set; }
        public string? LanguageAutoDetectMode { get; set; }
        public string? SourceLanguage { get; set; }
        public string[]? SourceLanguages { get; set; }
        public string? Language { get; set; }
        public string[]? Languages { get; set; }
        public string? IndexingPreset { get; set; }
        public string? StreamingPreset { get; set; }
        public string? LinguisticModelId { get; set; }
        public string? PersonModelId { get; set; }
        public string? LogoGroupId { get; set; }
        public bool? IsAdult { get; set; }
        public string[]? ExcludedAIs { get; set; }
        public bool? IsSearchable { get; set; }
        public string? PublishedUrl { get; set; }
        public string? PublishedProxyUrl { get; set; }
        public string? ViewToken { get; set; }
    }

    public class Insights
    {
        public string? Version { get; set; }
        public string? Duration { get; set; }
        public string? SourceLanguage { get; set; }
        public string[]? SourceLanguages { get; set; }
        public string? Language { get; set; }
        public string[]? Languages { get; set; }
        public Transcript[]? Transcripts { get; set; }
        public Ocr[]? Ocr { get; set; }
        public Keyword1[]? Keywords { get; set; }
        public Topic1[]? Topics { get; set; }
        public Label1[]? Labels { get; set; }
        public Scene[]? Scenes { get; set; }
        public Shot[]? Shots { get; set; }
        public NamedLocation[]? NamedLocations { get; set; }
        public NamedPeople[]? NamedPeople { get; set; }
        public Sentiment1[]? Sentiments { get; set; }
        public Block[]? Blocks { get; set; }
        public Speaker[]? Speakers { get; set; }
        public Textualcontentmoderation? TextualContentModeration { get; set; }
        public Statistics1? Statistics { get; set; }
    }

    public class Textualcontentmoderation
    {
        public int? Id { get; set; }
        public int? BannedWordsCount { get; set; }
        public decimal? BannedWordsRatio { get; set; }
        public string[]? Instances { get; set; }
    }

    public class Statistics1
    {
        public int? CorrespondenceCount { get; set; }
        public Speakertalktolistenratio1? SpeakerTalkToListenRatio { get; set; }
        public Speakerlongestmonolog1? SpeakerLongestMonolog { get; set; }
        public Speakernumberoffragments1? SpeakerNumberOfFragments { get; set; }
        public Speakerwordcount1? SpeakerWordCount { get; set; }
    }

    public class Speakertalktolistenratio1
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerlongestmonolog1
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakernumberoffragments1
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Speakerwordcount1
    {
        public decimal? _1 { get; set; }
        public decimal? _2 { get; set; }
        public decimal? _3 { get; set; }
    }

    public class Transcript
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public decimal? Confidence { get; set; }
        public int? SpeakerId { get; set; }
        public string? Language { get; set; }
        public Instance[]? Instances { get; set; }
    }

    public class Instance
    {
        public decimal? Confidence { get; set; }
        public string? ThumbnailId { get; set; }
        public string? InstanceSource { get; set; }
        public string? BrandType { get; set; }
        public string? AdjustedStart { get; set; }
        public string? AdjustedEnd { get; set; }
        public string? Start { get; set; }
        public string? End { get; set; }
    }



    public class Ocr
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public decimal? Confidence { get; set; }
        public decimal? Left { get; set; }
        public decimal? Top { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Angle { get; set; }
        public string? Language { get; set; }
        public Instance[]? Instances { get; set; }
    }

    public class Social
    {
        public bool? LikedByUser { get; set; }
        public decimal? Likes { get; set; }
        public decimal? Views { get; set; }
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

        public class Keyword1
    {
        public int? Id { get; set; }
        public string? Text { get; set; }
        public decimal? Confidence { get; set; }
        public string? Language { get; set; }
        public Instance[]? Instances { get; set; }
    }

    public class Block
    {
        public int? Id { get; set; }
        public Instance[]? Instances { get; set; }
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

        public class Topic1
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
        public string? IptcName { get; set; }
        public decimal? Confidence { get; set; }
        public string? IabName { get; set; }
        public string? Language { get; set; }
        public Instance[]? Instances { get; set; }
    }

        //public class Instance3
        //{
        //    public string adjustedStart { get; set; }
        //    public string adjustedEnd { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }
        //}

        public class Label1
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? ReferenceId { get; set; }
        public string? Language { get; set; }
        public Instance[]? Instances { get; set; }
    }

        //public class Instance4
        //{
        //    public decimal confidence { get; set; }
        //    public string adjustedStart { get; set; }
        //    public string adjustedEnd { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }
        //}

        public class Scene
    {
        public int? Id { get; set; }
        public Instance[]? Instances { get; set; }
    }

        //public class Instance5
        //{
        //    public string adjustedStart { get; set; }
        //    public string adjustedEnd { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }
        //}

        public class Shot
    {
        public int? Id { get; set; }
        public string[]? Tags { get; set; }
        public Keyframe[]? KeyFrames { get; set; }
        public Instance[]? Instances { get; set; }
    }

    public class Keyframe
    {
        public int? Id { get; set; }
        public Instance[]? Instances { get; set; }
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

        //public class Namedlocation1
        //{
        //    public int id { get; set; }
        //    public string name { get; set; }
        //    public string referenceId { get; set; }
        //    public string referenceUrl { get; set; }
        //    public string description { get; set; }
        //    public string[] tags { get; set; }
        //    public decimal confidence { get; set; }
        //    public bool isCustom { get; set; }
        //    public Instance[] instances { get; set; }
        //}

        //public class Instance8
        //{
        //    public string adjustedStart { get; set; }
        //    public string adjustedEnd { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }
        //}

        //public class Namedpeople1
        //{
        //    public int id { get; set; }
        //    public string name { get; set; }
        //    public string referenceId { get; set; }
        //    public string referenceUrl { get; set; }
        //    public string description { get; set; }
        //    public string[] tags { get; set; }
        //    public decimal confidence { get; set; }
        //    public bool isCustom { get; set; }
        //    public Instance[] instances { get; set; }
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

        public class Speaker
        {
            public int? Id { get; set; }
            public string? Name { get; set; }
            public Instance[]? Instances { get; set; }
        }

        //public class Instance12
        //{
        //    public string adjustedStart { get; set; }
        //    public string adjustedEnd { get; set; }
        //    public string start { get; set; }
        //    public string end { get; set; }
        //}

        public class Videosrange
    {
        public string? VideoId { get; set; }
        public RangeOfVideo? Range { get; set; }
    }

    public class RangeOfVideo
    {
        public string? Start { get; set; }
        public string? End { get; set; }
    }

    public class Emotions
    {
        public string? Type { get; set; }
        public decimal? SeenDurationRatio { get; set; }
        public Appearance[]? Appearances { get; set; }
    }


    public class AudioEffects
    {
        public string? AudioEffectKey { get; set; }
        public decimal? SeenDurationRatio { get; set; }
        public decimal? SeenDuration { get; set; }
        public Appearance[]? Appearances { get; set; }
    }


    public class FramePatterns
    {
        public string? DisplayName { get; set; }
        public int? Id { get; set; }
        public string? Name { get; set; }
        public Appearance[]? Appearances { get; set; }
    }



    public class Brand
    {
        public int? Id { get; set; }
        public string? ReferenceType { get; set; }
        public string? Name { get; set; }
        public string? ReferenceId { get; set; }
        public string? ReferenceUrl { get; set; }
        public string? Description { get; set; }
        public object[]? Tags { get; set; }
        public decimal? Confidence { get; set; }
        public bool? IsCustom { get; set; }
        public Instance[]? Instances { get; set; }
    }


    }
