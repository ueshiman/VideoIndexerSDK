using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public class ApiVideoModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("state")]
    public string? State { get; set; }
}