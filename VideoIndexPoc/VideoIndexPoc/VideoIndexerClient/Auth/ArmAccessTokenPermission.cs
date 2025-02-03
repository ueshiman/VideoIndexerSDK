using System.Text.Json.Serialization;

namespace VideoIndexPoc.VideoIndexerClient.Auth
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ArmAccessTokenPermission
    {
        Reader,
        Contributor,
        MyAccessAdministrator,
        Owner,
    }
}
