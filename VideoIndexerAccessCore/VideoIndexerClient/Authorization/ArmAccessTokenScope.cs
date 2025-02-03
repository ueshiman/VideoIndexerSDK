using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArmAccessTokenScope
    {
        Account,
        Project,
        Video
    }
}
