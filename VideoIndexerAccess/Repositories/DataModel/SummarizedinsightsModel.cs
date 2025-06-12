using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccess.Repositories.DataModel
{
    public class SummarizedinsightsModel
    {
        public string? Name { get; set; }
        public string? Id { get; set; }
        public string? PrivacyMode { get; set; }
        public DurationApiModel? Duration { get; set; }
        public string? ThumbnailVideoId { get; set; }
        public string? ThumbnailId { get; set; }
        public FaceApiModel[]? Faces { get; set; }
        public KeywordApiModel[]? Keywords { get; set; }
        public SentimentApiModel[]? Sentiments { get; set; }
        public EmotionsApiModel[]? Emotions { get; set; }
        public AudioEffectsApiModel[]? AudioEffects { get; set; }
        public LabelApiModel[]? Labels { get; set; }
        public FramePatternsApiModel[]? FramePatterns { get; set; }
        public BrandApiModel[]? Brands { get; set; }
        public NamedLocationApiModel[]? NamedLocations { get; set; }
        public NamedPeopleApiModel[]? NamedPeople { get; set; }
        public StatisticsApiModel? Statistics { get; set; }
        public TopicApiModel[]? Topics { get; set; }
    }
}
