using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess
{
    public class BrandModelApiAccess
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BrandModelApiAccess> _logger;

        public BrandModelApiAccess(ILogger<BrandModelApiAccess> logger)
        {
            _httpClient = new HttpClient();
            _logger = logger;
        }

        public async Task<HttpResponseMessage> CreateApiBrandModelAsync(string location, string accountId, string? accessToken, ApiBrandModel brand)
        {
            HttpResponseMessage? response;
            try
            {
                var url = $"https://api.videoindexer.ai/{location}/Accounts/{accountId}/Customization/ApiBrandModels" + (string.IsNullOrEmpty(accessToken) ? "" : "?accessToken=" + accessToken);
                var jsonContent = JsonSerializer.Serialize(brand);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                _logger.LogInformation("Sending request to {Url} with payload {Payload}", url, jsonContent);
                response = await _httpClient.PostAsync(url, content);
                _logger.LogInformation("Received response: {StatusCode}", response.StatusCode);
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError("HttpRequestException occurred: {Message}", httpEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred: {Message}", ex.Message);
                throw;
            }

            if (!(!response?.IsSuccessStatusCode ?? false)) return response;
            _logger.LogError("Throwing HttpRequestException due to unsuccessful status code: {StatusCode}", response.StatusCode);
            throw new HttpRequestException($"Request failed with status {response.StatusCode}: {errorContent}");

        }
        }
    }
}
