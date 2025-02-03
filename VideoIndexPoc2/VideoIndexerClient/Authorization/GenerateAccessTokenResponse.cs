using System.Text.Json.Serialization;

namespace VideoIndexPoc2.VideoIndexerClient.Authorization
{
    public class GenerateAccessTokenResponse
    {
        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }
    }
}
