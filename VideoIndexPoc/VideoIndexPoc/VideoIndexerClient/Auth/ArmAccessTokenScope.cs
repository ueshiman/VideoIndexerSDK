using System.Text.Json.Serialization;

namespace VideoIndexPoc.VideoIndexerClient.Auth
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArmAccessTokenScope
    {
        Account,
        Project,
        Video
    }
}
