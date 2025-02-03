using System.Text.Json.Serialization;

namespace VideoIndexPoc2.VideoIndexerClient.Model;

public class Video
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("state")]
    public string State { get; set; }
}