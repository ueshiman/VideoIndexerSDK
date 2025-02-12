﻿using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using VideoIndexPoc.VideoIndexerClient.Utils;

namespace VideoIndexPoc.VideoIndexerClient.Auth
{

    public class AccountTokenProviderDynamic : IAccountTokenProviderDynamic
    {
        private static readonly string TenantId = Environment.GetEnvironmentVariable("TENANT_ID");
        private static readonly string ClientId = Environment.GetEnvironmentVariable("CLIENT_ID");
        private static readonly string ClientSecret = Environment.GetEnvironmentVariable("CLIENT_SECRET");

        public async Task<string> GetArmAccessTokenAsync(CancellationToken ct = default)
        {

            var credentials = GetTokenCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { $"{Constants.AzureResourceManager}/.default" });
            var tokenRequestResult = await credentials.GetTokenAsync(tokenRequestContext, ct);
            return tokenRequestResult.Token;
        }

        public string GetArmAccessToken(CancellationToken ct = default)
        {

            var credentials = GetTokenCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { $"{Constants.AzureResourceManager}/.default" });
            var tokenRequestResult = credentials.GetToken(tokenRequestContext, ct);
            return tokenRequestResult.Token;
        }

        public async Task<string> GetAccountAccessTokenAsync(string armAccessToken, ArmAccessTokenPermission permission = ArmAccessTokenPermission.Contributor, ArmAccessTokenScope scope = ArmAccessTokenScope.Account, CancellationToken ct = default)
        {
            var accessTokenRequest = new AccessTokenRequest
            {
                PermissionType = permission,
                Scope = scope
            };

            try
            {
                var jsonRequestBody = JsonSerializer.Serialize(accessTokenRequest);
                Console.WriteLine($"Getting Account access token: {jsonRequestBody}");
                var httpContent = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                // Set request uri
                var requestUri = $"{Constants.AzureResourceManager}/subscriptions/{Constants.SubscriptionId}/resourcegroups/{Constants.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/{Constants.ViAccountName}/generateAccessToken?api-version={Constants.ApiVersion}";
                var client = HttpClientHelper.CreateHttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", armAccessToken);

                var result = await client.PostAsync(requestUri, httpContent, ct);
                result.EnsureSuccessStatusCode();
                var jsonResponseBody = await result.Content.ReadAsStringAsync(ct);
                Console.WriteLine($"Got Account access token: {scope} , {permission}");
                return JsonSerializer.Deserialize<GenerateAccessTokenResponse>(jsonResponseBody)?.AccessToken!;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        private TokenCredential GetTokenCredential()
        {
            if (!string.IsNullOrEmpty(ClientId) && !string.IsNullOrEmpty(ClientSecret))
            {
                return new ClientSecretCredential(TenantId, ClientId, ClientSecret);
            }
            else
            {
                var credentialOptions = TenantId == null ? new DefaultAzureCredentialOptions() : new DefaultAzureCredentialOptions
                {
                    VisualStudioTenantId = TenantId,
                    VisualStudioCodeTenantId = TenantId,
                    SharedTokenCacheTenantId = TenantId,
                    InteractiveBrowserTenantId = TenantId
                };

                return new DefaultAzureCredential(credentialOptions);
            }
        }


    }
}
