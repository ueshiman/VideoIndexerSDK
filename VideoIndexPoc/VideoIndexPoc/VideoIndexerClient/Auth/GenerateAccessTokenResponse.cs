using System.Text.Json.Serialization;

namespace VideoIndexPoc.VideoIndexerClient.Auth
{
    public class GenerateAccessTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
