using System.Text.Json.Serialization;

namespace VideoIndexPoc2.VideoIndexerClient.Model;

public class AccountProperties
{
    [JsonPropertyName("accountId")]
    public string Id { get; set; }
}