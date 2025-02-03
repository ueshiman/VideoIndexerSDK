using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public class ApiAccountProperties
{
    [JsonPropertyName("accountId")]
    public string? Id { get; set; }
}