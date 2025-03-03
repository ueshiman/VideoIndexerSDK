using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public class ApiVideoModel
{
    [JsonPropertyName("id")]
    public string? id { get; set; }

    [JsonPropertyName("state")]
    public string? state { get; set; }
}
