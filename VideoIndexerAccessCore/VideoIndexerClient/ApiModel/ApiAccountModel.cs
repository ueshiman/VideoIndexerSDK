﻿using System.Text.Json.Serialization;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

public class ApiAccountModel
{
    [JsonPropertyName("properties")]
    public ApiAccountProperties? Properties { get; set; }

    [JsonPropertyName("location")]
    public string? Location { get; set; }
}