using VideoIndexerAccessCore.VideoIndexerClient.ApiModel;

namespace VideoIndexerAccessCore.VideoIndexerClient.ApiAccess;

public interface IAccountMigrationStatusApiAccess
{
    Task<ApiAccountMigrationStatusModel?> GetAccountMigrationStatusAsync(string location, string accountId, string? accessToken = null);
    ApiAccountMigrationStatusModel? DeserializeResponse(string jsonString);
    Task<string> GetAccountMigrationStatusJsonAsync(string location, string accountId, string? accessToken = null);
}