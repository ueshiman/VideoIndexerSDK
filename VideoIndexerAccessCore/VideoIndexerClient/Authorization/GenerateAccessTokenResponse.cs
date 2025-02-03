using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.Authorization
{
    public class GenerateAccessTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }
    }
}
