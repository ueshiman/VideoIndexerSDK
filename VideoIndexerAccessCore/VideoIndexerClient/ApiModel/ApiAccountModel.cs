using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public class ApiAccountModel
{
    [JsonPropertyName("PropertiesModel")]
    public ApiAccountProperties? properties { get; set; }

    [JsonPropertyName("location")]
    public string? location { get; set; }
}
