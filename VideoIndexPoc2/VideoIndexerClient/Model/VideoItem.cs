using System;

namespace VideoIndexPoc2.VideoIndexerClient.Model
{
    public class VideoItem
    {

        public string partition { get; set; }
        public string description { get; set; }
        public string privacyMode { get; set; }
        public string state { get; set; }
        public string accountId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string userName { get; set; }
        public DateTime created { get; set; }
        public bool isOwned { get; set; }
        public bool isEditable { get; set; }
        public bool isBase { get; set; }
        public decimal durationInSeconds { get; set; }
        public string duration { get; set; }
        public Summarizedinsights summarizedInsights { get; set; }
        public ItemVideo[] videos { get; set; }
        public Videosrange[] videosRanges { get; set; }
        public Social social { get; set; }
    }

    public class Summarizedinsights
    {
        public string name { get; set; }
        public string id { get; set; }
        public string privacyMode { get; set; }
        public Duration duration { get; set; }
        public string thumbnailVideoId { get; set; }
        public string thumbnailId { get; set; }
        public Face[] faces { get; set; }
        public Keyword[] keywords { get; set; }
        public Sentiment[] sentiments { get; set; }
        public Emotions[] emotions { get; set; }
        public AudioEffects[] audioEffects { get; set; }
        public Label[] labels { get; set; }
        public FramePatterns[] framePatterns { get; set; }
        public Brand[] brands { get; set; }
        public Namedlocation[] namedLocations { get; set; }
        public Namedpeople[] namedPeople { get; set; }
        public Statistics statistics { get; set; }
        public Topic[] topics { get; set; }
    }

    public class Face
    {
        public string Name { get; set; }
        public int AppearanceCount { get; set; }
    }

    public class Duration
    {
        public string time { get; set; }
        public decimal seconds { get; set; }
    }

    public class Statistics
    {
        public int correspondenceCount { get; set; }
        public Speakertalktolistenratio speakerTalkToListenRatio { get; set; }
        public Speakerlongestmonolog speakerLongestMonolog { get; set; }
        public Speakernumberoffragments speakerNumberOfFragments { get; set; }
        public Speakerwordcount speakerWordCount { get; set; }
    }

    public class Speakertalktolistenratio
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakerlongestmonolog
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakernumberoffragments
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakerwordcount
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Keyword
    {
        public bool isTranscript { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
    }

    public class Appearance
    {
        public decimal confidence { get; set; }

        public string startTime { get; set; }
        public string endTime { get; set; }
        public decimal startSeconds { get; set; }
        public decimal endSeconds { get; set; }
    }

    public class Sentiment
    {
        public string sentimentKey { get; set; }
        public decimal seenDurationRatio { get; set; }
        public Appearance[] appearances { get; set; }
    }
    public class Sentiment1
    {
        public int id { get; set; }
        public decimal averageScore { get; set; }
        public string sentimentType { get; set; }
        public Instance[] instances { get; set; }
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
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
    }

    //public class Appearance2
    //{
    //    public decimal confidence { get; set; }
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public int startSeconds { get; set; }
    //    public int endSeconds { get; set; }
    //}

    public class Namedlocation
    {
        public string referenceId { get; set; }
        public string referenceUrl { get; set; }
        public decimal confidence { get; set; }
        public string description { get; set; }
        public decimal seenDuration { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
    }

    //public class Appearance3
    //{
    //    public string startTime { get; set; }
    //    public string endTime { get; set; }
    //    public decimal startSeconds { get; set; }
    //    public decimal endSeconds { get; set; }
    //}

    public class Namedpeople
    {
        public string referenceId { get; set; }
        public string referenceUrl { get; set; }
        public decimal confidence { get; set; }
        public string description { get; set; }
        public decimal seenDuration { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
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
        public string referenceUrl { get; set; }
        public string iptcName { get; set; }
        public string iabName { get; set; }
        public decimal confidence { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
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
        public string accountId { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string moderationState { get; set; }
        public string reviewState { get; set; }
        public string privacyMode { get; set; }
        public string processingProgress { get; set; }
        public string failureMessage { get; set; }
        public string externalId { get; set; }
        public string externalUrl { get; set; }
        public string metadata { get; set; }
        public Insights insights { get; set; }
        public string thumbnailId { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public bool detectSourceLanguage { get; set; }
        public string languageAutoDetectMode { get; set; }
        public string sourceLanguage { get; set; }
        public string[] sourceLanguages { get; set; }
        public string language { get; set; }
        public string[] languages { get; set; }
        public string indexingPreset { get; set; }
        public string streamingPreset { get; set; }
        public string linguisticModelId { get; set; }
        public string personModelId { get; set; }
        public string logoGroupId { get; set; }
        public bool isAdult { get; set; }
        public string[] excludedAIs { get; set; }
        public bool isSearchable { get; set; }
        public string publishedUrl { get; set; }
        public string publishedProxyUrl { get; set; }
        public string viewToken { get; set; }
    }

    public class Insights
    {
        public string version { get; set; }
        public string duration { get; set; }
        public string sourceLanguage { get; set; }
        public string[] sourceLanguages { get; set; }
        public string language { get; set; }
        public string[] languages { get; set; }
        public Transcript[] transcript { get; set; }
        public Ocr[] ocr { get; set; }
        public Keyword1[] keywords { get; set; }
        public Topic1[] topics { get; set; }
        public Label1[] labels { get; set; }
        public Scene[] scenes { get; set; }
        public Shot[] shots { get; set; }
        public Namedlocation1[] namedLocations { get; set; }
        public Namedpeople1[] namedPeople { get; set; }
        public Sentiment1[] sentiments { get; set; }
        public Block[] blocks { get; set; }
        public Speaker[] speakers { get; set; }
        public Textualcontentmoderation textualContentModeration { get; set; }
        public Statistics1 statistics { get; set; }
    }

    public class Textualcontentmoderation
    {
        public int id { get; set; }
        public int bannedWordsCount { get; set; }
        public decimal bannedWordsRatio { get; set; }
        public string[] instances { get; set; }
    }

    public class Statistics1
    {
        public int correspondenceCount { get; set; }
        public Speakertalktolistenratio1 speakerTalkToListenRatio { get; set; }
        public Speakerlongestmonolog1 speakerLongestMonolog { get; set; }
        public Speakernumberoffragments1 speakerNumberOfFragments { get; set; }
        public Speakerwordcount1 speakerWordCount { get; set; }
    }

    public class Speakertalktolistenratio1
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakerlongestmonolog1
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakernumberoffragments1
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Speakerwordcount1
    {
        public decimal _1 { get; set; }
        public decimal _2 { get; set; }
        public decimal _3 { get; set; }
    }

    public class Transcript
    {
        public int id { get; set; }
        public string text { get; set; }
        public decimal confidence { get; set; }
        public int speakerId { get; set; }
        public string language { get; set; }
        public Instance[] instances { get; set; }
    }

    public class Instance
    {
        public decimal confidence { get; set; }
        public string thumbnailId { get; set; }
        public string instanceSource { get; set; }
        public string brandType { get; set; }

        public string adjustedStart { get; set; }
        public string adjustedEnd { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }



    public class Ocr
    {
        public int id { get; set; }
        public string text { get; set; }
        public decimal confidence { get; set; }
        public decimal left { get; set; }
        public decimal top { get; set; }
        public decimal width { get; set; }
        public decimal height { get; set; }
        public decimal angle { get; set; }
        public string language { get; set; }
        public Instance[] instances { get; set; }
    }

    public class Social
    {
        public bool likedByUser { get; set; }
        public decimal likes { get; set; }
        public decimal views { get; set; }
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
        public int id { get; set; }
        public string text { get; set; }
        public decimal confidence { get; set; }
        public string language { get; set; }
        public Instance[] instances { get; set; }
    }

    public class Block
    {
        public int id { get; set; }
        public Instance[] instances { get; set; }
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
        public int id { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string referenceType { get; set; }
        public string iptcName { get; set; }
        public decimal confidence { get; set; }
        public string iabName { get; set; }
        public string language { get; set; }
        public Instance[] instances { get; set; }
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
        public int id { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string language { get; set; }
        public Instance[] instances { get; set; }
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
        public int id { get; set; }
        public Instance[] instances { get; set; }
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
        public int id { get; set; }
        public string[] tags { get; set; }
        public Keyframe[] keyFrames { get; set; }
        public Instance[] instances { get; set; }
    }

    public class Keyframe
    {
        public int id { get; set; }
        public Instance[] instances { get; set; }
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

    public class Namedlocation1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string referenceUrl { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
        public decimal confidence { get; set; }
        public bool isCustom { get; set; }
        public Instance[] instances { get; set; }
    }

    public class Instance8
    {
        public string adjustedStart { get; set; }
        public string adjustedEnd { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Namedpeople1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string referenceUrl { get; set; }
        public string description { get; set; }
        public string[] tags { get; set; }
        public decimal confidence { get; set; }
        public bool isCustom { get; set; }
        public Instance[] instances { get; set; }
    }

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
        public int id { get; set; }
        public string name { get; set; }
        public Instance[] instances { get; set; }
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
        public string videoId { get; set; }
        public Range range { get; set; }
    }

    public class Range
    {
        public string start { get; set; }
        public string end { get; set; }
    }

    public class Emotions
    {
        public string type { get; set; }
        public decimal seenDurationRatio { get; set; }
        public Appearance[] appearances { get; set; }
    }


    public class AudioEffects
    {
        public string audioEffectKey { get; set; }
        public decimal seenDurationRatio { get; set; }
        public decimal seenDuration { get; set; }
        public Appearance[] appearances { get; set; }
    }


    public class FramePatterns
    {
        public string displayName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public Appearance[] appearances { get; set; }
    }



    public class Brand
    {
        public int id { get; set; }
        public string referenceType { get; set; }
        public string name { get; set; }
        public string referenceId { get; set; }
        public string referenceUrl { get; set; }
        public string description { get; set; }
        public object[] tags { get; set; }
        public decimal confidence { get; set; }
        public bool isCustom { get; set; }
        public Instance[] instances { get; set; }
    }





}