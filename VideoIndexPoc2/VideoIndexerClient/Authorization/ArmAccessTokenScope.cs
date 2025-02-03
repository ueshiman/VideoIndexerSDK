using System.Text.Json.Serialization;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArmAccessTokenScope
    {
        Account,
        Project,
        Video
    }
}
