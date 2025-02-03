using System.Text.Json.Serialization;

namespace VideoIndexPoc.VideoIndexerClient.Model;

public class AccountProperties
{
    [JsonPropertyName("accountId")]
    public string Id { get; set; }
}